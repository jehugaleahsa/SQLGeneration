using System;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Extracts tokens from a source.
    /// </summary>
    public interface ITokenSource
    {
        /// <summary>
        /// Attempts to retrieve a token matching the definition associated
        /// with the given name.
        /// </summary>
        /// <param name="tokenName">The name of the token to try to retrieve.</param>
        /// <returns>
        /// A result object describing whether the match was a success and what value 
        /// was found.
        /// </returns>
        TokenResult GetToken(string tokenName);

        /// <summary>
        /// Restores the given token to the front of the token stream.
        /// </summary>
        /// <param name="token">The token result containing the token to restore.</param>
        void PutBack(string token);
    }
}
