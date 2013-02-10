using System;
using System.Collections.Generic;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Helps create literals.
    /// </summary>
    public abstract class Literal : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Initializes a new instance of a Literal.
        /// </summary>
        protected Literal()
        {
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return GetTokens(options);
        }

        /// <summary>
        /// Gets a string representing the item in a declaration, without the alias.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        protected abstract IEnumerable<string> GetTokens(CommandOptions options);

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return GetTokens(options);
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return GetTokens(options);
        }
    }
}
