using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a list of values that can appear in an 'in' comparison.
    /// </summary>
    public class ValueList : IValueList
    {
        private readonly List<IProjectionItem> _values;

        /// <summary>
        /// Initializes a new instance of a InList.
        /// </summary>
        public ValueList()
        {
            _values = new List<IProjectionItem>();
        }

        /// <summary>
        /// Initializes a new instance of a InList.
        /// </summary>
        /// <param name="values">The values to add to the list.</param>
        public ValueList(params IProjectionItem[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (values.Contains(null))
            {
                throw new ArgumentNullException("values");
            }
            _values = new List<IProjectionItem>(values);
        }

        /// <summary>
        /// Gets the values being provided.
        /// </summary>
        public IEnumerable<IProjectionItem> Values
        {
            get { return new ReadOnlyCollection<IProjectionItem>(_values); }
        }

        /// <summary>
        /// Adds the given projection item to the values list.
        /// </summary>
        /// <param name="item">The value to add.</param>
        public void AddValue(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _values.Add(item);
        }

        /// <summary>
        /// Adds the given projection item from the values list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveValue(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _values.Remove(item);
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder("(");
            StringBuilder separatorBuilder = new StringBuilder(",");
            if (context.Options.OneValueListItemPerLine)
            {
                result.AppendLine();
                separatorBuilder.AppendLine();
            }
            else
            {
                separatorBuilder.Append(' ');
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(context);
            IEnumerable<string> values = _values.Select(value => formatter.GetUnaliasedReference(value));
            if (context.Options.OneValueListItemPerLine && context.Options.IndentValueListItems)
            {
                string indentationText = context.Indent().GetIndentationText();
                values = values.Select(value => indentationText + value);
            }
            string separated = String.Join(separatorBuilder.ToString(), values);
            result.Append(separated);
            if (context.Options.OneValueListItemPerLine)
            {
                result.AppendLine();
            }
            result.Append(')');
            return result.ToString();
        }

        bool IValueProvider.IsQuery
        {
            get { return false; }
        }
    }
}
