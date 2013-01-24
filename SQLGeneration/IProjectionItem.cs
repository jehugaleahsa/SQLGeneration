using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in the projection-clause of a select statement.
    /// </summary>
    public interface IProjectionItem
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
        /// Gets a string representing the item in a declaration, without the alias.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        string GetFullText(BuilderContext context);
    }
}
