using System;
using System.Collections.Generic;
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
        protected override IEnumerable<string> GetTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("NULL");
            return stream;            
        }
    }
}
