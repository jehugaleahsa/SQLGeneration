using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in a join statement.
    /// </summary>
    public interface IJoinItem
    {
        /// <summary>
        /// Gets or sets an alias for the item.
        /// </summary>
        string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new column under the table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        IColumn CreateColumn(string columnName);

        /// <summary>
        /// Gets a string that declares the item.
        /// </summary>
        /// <param name="where">The where clause of the query.</param>
        /// <returns>A string declaring the item.</returns>
        string GetDeclaration(IFilterGroup where);

        /// <summary>
        /// Represents a string that represents the item outside of the declaration.
        /// </summary>
        /// <returns>A string referencing the item.</returns>
        string GetReference();
    }
}
