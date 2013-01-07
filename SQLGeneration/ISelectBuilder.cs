using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a select statement.
    /// </summary>
    public interface ISelectBuilder : ICommandBuilder, IFilteredCommand, IJoinItem, IProjectionItem, IValueProvider
    {
        /// <summary>
        /// Gets or sets whether to select only unique results.
        /// </summary>
        bool IsDistinct
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of items to restrict the results to.
        /// </summary>
        ITop Top
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the items that are part of the projection.
        /// </summary>
        IEnumerable<IProjectionItem> Projection
        {
            get;
        }

        /// <summary>
        /// Adds a projection item to the projection.
        /// </summary>
        /// <param name="item">The projection item to add.</param>
        /// <returns>The item that was added.</returns>
        void AddProjection(IProjectionItem item);

        /// <summary>
        /// Removes the projection item from the projection.
        /// </summary>
        /// <param name="item">The projection item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        bool RemoveProjection(IProjectionItem item);

        /// <summary>
        /// Gets the tables, joins or sub-queries that are projected from.
        /// </summary>
        IEnumerable<IJoinItem> From
        {
            get;
        }

        /// <summary>
        /// Adds the given join item to the from clause.
        /// </summary>
        /// <param name="joinItem">The join item to add.</param>
        void AddJoinItem(IJoinItem joinItem);

        /// <summary>
        /// Removes the given join item from the from clause.
        /// </summary>
        /// <param name="joinItem">The join item to remove.</param>
        bool RemoveJoinItem(IJoinItem joinItem);

        /// <summary>
        /// Gets the items used to sort the results.
        /// </summary>
        IEnumerable<IOrderBy> OrderBy
        {
            get;
        }

        /// <summary>
        /// Adds a sort criteria to the query.
        /// </summary>
        /// <param name="item">The sort criteria to add.</param>
        void AddOrderBy(IOrderBy item);

        /// <summary>
        /// Removes the sort criteria from the query.
        /// </summary>
        /// <param name="item">The order by item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        bool RemoveOrderBy(IOrderBy item);

        /// <summary>
        /// Gets the items that the query is grouped by.
        /// </summary>
        IEnumerable<IGroupByItem> GroupBy
        {
            get;
        }

        /// <summary>
        /// Adds the item to the group by clause of the query.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void AddGroupBy(IGroupByItem item);

        /// <summary>
        /// Removes the item from the group by clause of the query.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        bool RemoveGroupBy(IGroupByItem item);

        /// <summary>
        /// Gets a filter to apply to the aggregate functions.
        /// </summary>
        IFilterGroup Having
        {
            get;
        }
    }
}
