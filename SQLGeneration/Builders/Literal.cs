using System;
using SQLGeneration.Parsing;

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

        TokenStream IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return GetTokens(options);
        }

        /// <summary>
        /// Gets a string representing the item in a declaration, without the alias.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        protected abstract TokenStream GetTokens(CommandOptions options);

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        TokenStream IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return GetTokens(options);
        }

        TokenStream IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return GetTokens(options);
        }

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            OnAccept(visitor);
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected abstract void OnAccept(BuilderVisitor visitor);
    }
}
