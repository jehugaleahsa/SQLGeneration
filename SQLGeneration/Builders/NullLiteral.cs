using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the literal NULL.
    /// </summary>
    public class NullLiteral : Literal
    {
        /// <summary>
        /// Initializes a new instance of a NullLiteral.
        /// </summary>
        public NullLiteral()
        {
        }

        /// <summary>
        /// Gets a string representing the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        protected override TokenStream GetTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Null, "NULL"));
            return stream;            
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected override void OnAccept(BuilderVisitor visitor)
        {
            visitor.VisitNullLiterator(this);
        }
    }
}
