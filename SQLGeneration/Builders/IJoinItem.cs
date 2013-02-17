using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents an item that can appear in a join statement.
    /// </summary>
    public interface IJoinItem
    {
        /// <summary>
        /// Gets a string that declares the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string declaring the item.</returns>
        TokenStream GetDeclarationTokens(CommandOptions options);
    }
}
