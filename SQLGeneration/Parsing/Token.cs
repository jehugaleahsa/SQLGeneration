using System;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Represents a token within an expression.
    /// </summary>
    public sealed class Token : IExpressionItem
    {
        /// <summary>
        /// Initializes a new instance of a Token.
        /// </summary>
        /// <param name="tokenName">The type of the token that is expected.</param>
        internal Token(string tokenName)
            : this(tokenName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of a Token.
        /// </summary>
        /// <param name="tokenName">The type of the token that is expected.</param>
        /// <param name="expectedValue">The expected value of the token.</param>
        internal Token(string tokenName, string expectedValue)
        {
            TokenType = tokenName;
            ExpectedValue = expectedValue;
        }

        /// <summary>
        /// Gets the type of the token that is expected.
        /// </summary>
        public string TokenType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the expected value of the token.
        /// </summary>
        public string ExpectedValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Attempts to match the expression item with the values returned by the parser.
        /// </summary>
        /// <param name="parser">The parser currently iterating over the token source.</param>
        /// <param name="depth"></param>
        /// <returns>The results of the match.</returns>
        public MatchResult Match(Parser parser, int depth)
        {
            TokenResult tokenResult = parser.GetToken(TokenType);
            MatchResult matchResult = new MatchResult(tokenResult.IsMatch);
            matchResult.Context = tokenResult;
            if (tokenResult.IsMatch)
            {
                Console.Out.WriteLine(tokenResult.Value);
            }
            return matchResult;
        }
    }
}
