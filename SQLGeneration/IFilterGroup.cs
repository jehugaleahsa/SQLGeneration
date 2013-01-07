using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Groups related filters together.
    /// </summary>
    public interface IFilterGroup : IFilter
    {
        /// <summary>
        /// Gets the filters in the filter group.
        /// </summary>
        IEnumerable<IFilter> Filters
        {
            get;
        }

        /// <summary>
        /// Adds the filter to the group.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        void AddFilter(IFilter filter);

        /// <summary>
        /// Removes the filter from the group.
        /// </summary>
        /// <param name="filter">The filter to remove.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        bool RemoveFilter(IFilter filter);

        /// <summary>
        /// Gets whether there are any filters.
        /// </summary>
        bool HasFilters
        {
            get;
        }
    }
}
