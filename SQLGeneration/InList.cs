using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a list of values that can appear in an 'in' comparison.
    /// </summary>
    public class InList : IInList
    {
        private readonly List<IProjectionItem> _values;

        /// <summary>
        /// Creates a new InList.
        /// </summary>
        public InList()
        {
            _values = new List<IProjectionItem>();
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

        string IFilterItem.GetFilterItemText()
        {
            ProjectionItemFormatter formatter = new ProjectionItemFormatter();
            return "(" + String.Join(", ", from value in _values select formatter.GetUnaliasedReference(value)) + ")";
        }
    }
}
