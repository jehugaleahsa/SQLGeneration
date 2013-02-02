using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a list of values that can appear in an 'in' comparison.
    /// </summary>
    public class ValueList : IValueProvider
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

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            // <ValueList> => "(" [ <ProjectionReference> [ "," <ValueList> ] ] ")"
            yield return "(";
            if (_values.Count > 0)
            {
                foreach (string token in buildValueList(options, 0))
                {
                    yield return token;
                }
            }
            yield return ")";
        }

        private IEnumerable<string> buildValueList(CommandOptions options, int valueIndex)
        {
            if (valueIndex == _values.Count - 1)
            {
                IProjectionItem current = _values[valueIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                foreach (string token in formatter.GetUnaliasedReference(current))
                {
                    yield return token;
                }
            }
            else
            {
                IProjectionItem current = _values[valueIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                foreach (string token in formatter.GetUnaliasedReference(current))
                {
                    yield return token;
                }
                yield return ",";
                foreach (string token in buildValueList(options, valueIndex + 1))
                {
                    yield return token;
                }
            }
        }

        bool IValueProvider.IsQuery
        {
            get { return false; }
        }
    }
}
