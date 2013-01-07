using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of an update statement.
    /// </summary>
    public interface IUpdateBuilder : INonQueryBuilder, IFilteredCommand
    {
        /// <summary>
        /// Gets the table that is being updated.
        /// </summary>
        ITable Table
        {
            get;
        }

        /// <summary>
        /// Gets the columns that are being set.
        /// </summary>
        IEnumerable<ISetter> Setters
        {
            get;
        }

        /// <summary>
        /// Adds the setter to the update statement.
        /// </summary>
        /// <param name="setter">The setter to add.</param>
        void AddSetter(ISetter setter);

        /// <summary>
        /// Removes the setter from the update statement.
        /// </summary>
        /// <param name="setter">The setter to remove.</param>
        /// <returns>True if the setter is removed; otherwise, false.</returns>
        bool RemoveSetter(ISetter setter);
    }
}
