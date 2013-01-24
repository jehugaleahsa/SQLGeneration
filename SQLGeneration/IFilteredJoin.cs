using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a join that is filtered with an ON expression.
    /// </summary>
    public interface IFilteredJoin : IJoin
    {
        /// <summary>
        /// Gets the filters by which the left and right hand items are joined.
        /// </summary>
        IEnumerable<IFilter> On
        {
            get;
        }

        /// <summary>
        /// Adds a condition by which the items are joined.
        /// </summary>
        /// <param name="filter">The join condition.</param>
        void AddFilter(IFilter filter);

        /// <summary>
        /// Removes a condition by which the items are joined.
        /// </summary>
        /// <param name="filter">The join condition.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        bool RemoveFilter(IFilter filter);
    }
}
