using System;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a column name.
    /// </summary>
    public interface IColumn : IProjectionItem, IGroupByItem, IFilterItem
    {
        /// <summary>
        /// Gets the join item that the column belongs to.
        /// </summary>
        IColumnSource Source
        {
            get;
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
