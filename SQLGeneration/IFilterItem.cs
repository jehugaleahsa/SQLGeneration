using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in a filter.
    /// </summary>
    public interface IFilterItem
    {
        /// <summary>
        /// Gets a string representing the item.
        /// </summary>
        string GetFilterItemText();
    }
}
