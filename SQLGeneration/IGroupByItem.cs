using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in a group by clause in a select statement.
    /// </summary>
    public interface IGroupByItem
    {
        /// <summary>
        /// Gets a string representation of the group by.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        IExpressionItem GetGroupByExpression(CommandOptions options);
    }
}
