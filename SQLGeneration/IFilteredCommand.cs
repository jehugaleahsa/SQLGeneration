using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a command that can be filtered.
    /// </summary>
    public interface IFilteredCommand
    {
        /// <summary>
        /// Gets the filters in the where clause.
        /// </summary>
        IEnumerable<IFilter> Where
        {
            get;
        }

        /// <summary>
        /// Adds the filter to the where clause.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        void AddWhere(IFilter filter);

        /// <summary>
        /// Removes the filter from the where clause.
        /// </summary>
        /// <param name="filter">The filter to remove.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        bool RemoveWhere(IFilter filter);
    }
}
