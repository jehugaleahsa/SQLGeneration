using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a SQL statement.
    /// </summary>
    public interface ICommand : IVisitableBuilder
    {
        /// <summary>
        /// Gets the expression making up the command.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expressions.</returns>
        TokenStream GetCommandTokens(CommandOptions options);
    }
}
