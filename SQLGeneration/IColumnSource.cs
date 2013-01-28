using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can have columns.
    /// </summary>
    public interface IColumnSource
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
        Column CreateColumn(string columnName);

        /// <summary>
        /// Creates a new column under the table with the given alias.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        Column CreateColumn(string columnName, string alias);

        /// <summary>
        /// Represents a string that represents the item outside of the declaration.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string referencing the item.</returns>
        IExpressionItem GetReferenceExpression(CommandOptions options);
    }
}
