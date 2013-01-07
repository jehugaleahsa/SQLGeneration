using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of an insert statement.
    /// </summary>
    public interface IInsertBuilder : INonQueryBuilder
    {
        /// <summary>
        /// Gets the table that is being inserted into.
        /// </summary>
        ITable Table
        {
            get;
        }

        /// <summary>
        /// Gets the columns being inserted into.
        /// </summary>
        IEnumerable<IColumn> Columns
        {
            get;
        }

        /// <summary>
        /// Adds the column to the insert statement.
        /// </summary>
        /// <param name="column">The column to add.</param>
        void AddColumn(IColumn column);

        /// <summary>
        /// Removes the column from the insert statement.
        /// </summary>
        /// <param name="column">The column to remove.</param>
        /// <returns>True if the column was removed; otherwise, false.</returns>
        bool RemoveColumn(IColumn column);

        /// <summary>
        /// Gets the list of values or select statement that populates the insert.
        /// </summary>
        IValueProvider Values
        {
            get;
        }
    }
}
