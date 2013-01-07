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
        string GetFullText();
    }
}
