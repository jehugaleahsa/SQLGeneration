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
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        string GetFilterItemText(BuilderContext context);
    }
}
