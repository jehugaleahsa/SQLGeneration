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
        /// Creates a new column under the table with the given alias.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        IColumn CreateColumn(string columnName, string alias);

        /// <summary>
        /// Gets a string that declares the item.
        /// </summary>
        /// <param name="where">The where clause of the query.</param>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>A string declaring the item.</returns>
        string GetDeclaration(BuilderContext context, IFilterGroup where);

        /// <summary>
        /// Represents a string that represents the item outside of the declaration.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>A string referencing the item.</returns>
        string GetReference(BuilderContext context);
    }
}
