using System;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a delete statement.
    /// </summary>
    public interface IDeleteBuilder : INonQueryBuilder, IFilteredCommand
    {
        /// <summary>
        /// Gets the table being deleted from.
        /// </summary>
        ITable Table
        {
            get;
        }
    }
}
