using System;
using System.Collections.Generic;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Represents an attempt to parse an expression.
    /// </summary>
    public interface IParseAttempt
    {
        /// <summary>
        /// Gets the tokens that were collected during the attempt.
        /// </summary>
        List<string> Tokens { get; }

        /// <summary>
        /// Sets the handle for the given token.
        /// </summary>
        /// <param name="result">The results of the parse for the token.</param>
        /// <param name="token">The token found by the parser.</param>
        void SetTokenHandler(MatchResult result, string token);

        /// <summary>
        /// Sets the handle for the given expression type, if it is specified.
        /// </summary>
        /// <param name="expressionType">The type of the expression to run the handler for.</param>
        /// <param name="result">The results of the parse for the expression.</param>
        void SetHandler(string expressionType, MatchResult result);

        /// <summary>
        /// Attempts to get a token of the given type.
        /// </summary>
        /// <param name="tokenName">The type of the token to attempt to retrieve.</param>
        /// <returns>The result of the search.</returns>
        TokenResult GetToken(string tokenName);

        /// <summary>
        /// Creates an attempt to parse a child expression.
        /// </summary>
        /// <returns>A new attempt object.</returns>
        IParseAttempt Attempt();

        /// <summary>
        /// Accepts the attempt as a successful parse, joining the given attempt's tokens
        /// with the current attempt's.
        /// </summary>
        /// <param name="attempt">The child attempt to accept.</param>
        void Accept(IParseAttempt attempt);

        /// <summary>
        /// Rejects the attempt as a failed parse, returning the attempt's token
        /// to the token stream.
        /// </summary>
        void Reject();
    }
}
