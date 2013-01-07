using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a join between two joinable items to the command.
    /// </summary>
    public interface IJoin : IJoinItem
    {
        /// <summary>
        /// Gets or sets whether the join should be wrapped in parentheses.
        /// </summary>
        bool WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the item on the left hand side of the join.
        /// </summary>
        IJoinItem LeftHand
        {
            get;
        }

        /// <summary>
        /// Gets the item on the right hand side of the join.
        /// </summary>
        IJoinItem RightHand
        {
            get;
        }

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
