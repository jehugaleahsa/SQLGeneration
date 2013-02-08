using System;
using System.Collections.Generic;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents an item that can appear in a filter.
    /// </summary>
    public interface IFilterItem
    {
        /// <summary>
        /// Gets a string representing the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        IEnumerable<string> GetFilterTokens(CommandOptions options);
    }
}
