using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in a join statement.
    /// </summary>
    public interface IJoinItem
    {
        /// <summary>
        /// Gets a string that declares the item.
        /// </summary>
        /// <param name="where">The where clause of the query.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string declaring the item.</returns>
        IExpressionItem GetDeclarationExpression(CommandOptions options, FilterGroup where);
    }
}
