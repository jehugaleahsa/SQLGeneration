using System;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a table name.
    /// </summary>
    public interface ITable : IColumnSource
    {
        /// <summary>
        /// Gets or sets the schema the table belongs to.
        /// </summary>
        ISchema Schema
        {
            get;
        }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
