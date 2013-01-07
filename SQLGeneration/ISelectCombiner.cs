using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Performs a set operation on the results of two queries.
    /// </summary>
    public interface ISelectCombiner : ICommandBuilder, IJoinItem, IProjectionItem, IValueProvider
    {
        /// <summary>
        /// Gets the queries that are to be combined.
        /// </summary>
        IEnumerable<ISelectBuilder> Queries
        {
            get;
        }

        /// <summary>
        /// Adds the query to the combination.
        /// </summary>
        /// <param name="query">The query to add.</param>
        void AddQuery(ISelectBuilder query);

        /// <summary>
        /// Removes the query from the combination.
        /// </summary>
        /// <param name="query">The query to remove.</param>
        /// <returns>True if the query is removed; otherwise, false.</returns>
        bool RemoveQuery(ISelectBuilder query);
    }
}
