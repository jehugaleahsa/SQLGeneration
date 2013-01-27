using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a SQL statement.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the expression making up the command.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expressions.</returns>
        IExpressionItem GetCommandExpression(CommandOptions options);
    }
}
