using System;
using System.Collections.Generic;

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
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string declaring the item.</returns>
        IEnumerable<string> GetDeclarationExpression(CommandOptions options);
    }
}
