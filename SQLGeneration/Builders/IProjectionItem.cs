using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents an item that can appear in the projection-clause of a select statement.
    /// </summary>
    public interface IProjectionItem : IVisitableBuilder
    {
        /// <summary>
        /// Gets a string representing the item in a declaration, without the alias.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        TokenStream GetProjectionTokens(CommandOptions options);

        /// <summary>
        /// Gets the name that a projection will be referred to with.
        /// </summary>
        /// <returns>The name -or- null if it doesn't have a name.</returns>
        string GetProjectionName();
    }
}
