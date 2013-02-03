using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Provides convenience methods for building streams of tokens.
    /// </summary>
    public sealed class TokenStream : IEnumerable<string>
    {
        private IEnumerable<string> tokens;

        /// <summary>
        /// Initializes a new instance of a TokenStream.
        /// </summary>
        public TokenStream()
        {
            tokens = Enumerable.Empty<string>();
        }

        /// <summary>
        /// Adds a token to the stream.
        /// </summary>
        /// <param name="token">The token to add.</param>
        /// <returns>The current token stream.</returns>
        public TokenStream Add(string token)
        {
            tokens = tokens.Concat(Enumerable.Empty<string>().DefaultIfEmpty(token));
            return this;
        }

        /// <summary>
        /// Adds the given tokens to the stream.
        /// </summary>
        /// <param name="tokens">The tokens to add.</param>
        /// <returns>The current token stream.</returns>
        public TokenStream AddRange(IEnumerable<string> tokens)
        {
            this.tokens = this.tokens.Concat(tokens);
            return this;
        }

        /// <summary>
        /// Gets the tokens that are in the stream.
        /// </summary>
        /// <returns>The stream of tokens.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
