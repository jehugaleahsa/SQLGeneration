using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of the values being inserted in an insert statement.
    /// </summary>
    public interface IInList : IValueProvider
    {
        /// <summary>
        /// Gets the values being provided.
        /// </summary>
        IEnumerable<IProjectionItem> Values
        {
            get;
        }

        /// <summary>
        /// Adds the given projection item to the values list.
        /// </summary>
        /// <param name="item">The value to add.</param>
        void AddValue(IProjectionItem item);

        /// <summary>
        /// Adds the given projection item from the values list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        bool RemoveValue(IProjectionItem item);
    }
}
