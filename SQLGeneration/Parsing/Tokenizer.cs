using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SQLGeneration.Properties;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Generates a series of tokens.
    /// </summary>
    public abstract class Tokenizer : ITokenRegistry
    {
        private readonly Dictionary<string, TokenTypeDefinition> definitionLookup;
        private Regex regex;

        /// <summary>
        /// Initializes a new instance of a Tokenizer.
        /// </summary>
        protected Tokenizer()
        {
            definitionLookup = new Dictionary<string, TokenTypeDefinition>();
        }

        /// <summary>
        /// Associates the given token name to the regular expression that
        /// tokens of that type are expected to match.
        /// </summary>
        /// <param name="tokenName">The type of the token to associate the regular expression with.</param>
        /// <param name="regex">The regular expression that the token is expected match.</param>
        /// <param name="ignoreCase">Specifies whether the regex should be case-sensitive.</param>
        /// <remarks>
        /// Multiple regular expressions can be registered to the same token name.
        /// They will be tried in the order that they are defined.
        /// </remarks>
        public void Define(string tokenName, string regex, bool ignoreCase = false)
        {
            TokenTypeDefinition definition;
            if (!definitionLookup.TryGetValue(tokenName, out definition))
            {
                definition = new TokenTypeDefinition(tokenName);
                definitionLookup.Add(tokenName, definition);
            }
            Expression expression = new Expression()
            {
                Regex = regex,
                IgnoreCase = ignoreCase,
            };
            definition.Expressions.Add(expression);
            regex = null;
        }

        /// <summary>
        /// Gets whether a token with the given name has been registered.
        /// </summary>
        /// <param name="tokenName">The name of the token to search for.</param>
        /// <returns>True if the token has been registered; otherwise, false.</returns>
        public bool Exists(string tokenName)
        {
            return definitionLookup.ContainsKey(tokenName);
        }

        private bool Match(string tokenName, string token)
        {
            Regex regex = getRegex();
            Match match = regex.Match(token);
            return match.Success && match.Groups[tokenName].Success;
        }

        private Regex getRegex()
        {
            if (regex == null)
            {
                string pattern = String.Join("|", definitionLookup.Keys.Select(tokenName => getTokenTypeRegex(definitionLookup[tokenName])));
                regex = new Regex(pattern, RegexOptions.Compiled);
            }
            return regex;
        }

        private string getTokenTypeRegex(TokenTypeDefinition definition)
        {
            StringBuilder regexBuilder = new StringBuilder();
            regexBuilder.Append("(?<");
            regexBuilder.Append(definition.Type);
            regexBuilder.Append(">");
            string combined = String.Join("|", definition.Expressions.Select(regex => getExpressionRegex(regex)));
            regexBuilder.Append(combined);
            regexBuilder.Append(")");
            return regexBuilder.ToString();
        }

        private string getExpressionRegex(Expression expression)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(^");
            if (expression.IgnoreCase)
            {
                builder.Append("(?i)");
            }
            builder.Append(expression.Regex);
            if (expression.IgnoreCase)
            {
                builder.Append("(?-i)");
            }
            builder.Append("$)");
            return builder.ToString();
        }

        /// <summary>
        /// Creates a stream of tokens that are verified against the token definitions.
        /// </summary>
        /// <param name="tokenStream">The stream of tokens.</param>
        /// <returns>The new token source.</returns>
        public ITokenSource CreateTokenSource(IEnumerable<string> tokenStream)
        {
            return new TokenSource(this, tokenStream);
        }

        private sealed class TokenTypeDefinition
        {
            public TokenTypeDefinition(string type)
            {
                Type = type;
                Expressions = new List<Expression>();
            }

            public string Type { get; private set; }

            public List<Expression> Expressions { get; private set; }
        }

        private sealed class Expression
        {
            public string Regex { get; set; }

            public bool IgnoreCase { get; set; }
        }

        private sealed class TokenSource : ITokenSource
        {
            private readonly Tokenizer tokenizer;
            private readonly IEnumerator<string> tokenEnumerator;
            private readonly LinkedList<string> undoBuffer;

            /// <summary>
            /// Initializes a new instance of a TokenSource.
            /// </summary>
            /// <param name="tokenizer">The tokenizer containing the token definitions.</param>
            /// <param name="tokenStream">A stream of tokens.</param>
            public TokenSource(Tokenizer tokenizer, IEnumerable<string> tokenStream)
            {
                this.tokenizer = tokenizer;
                tokenEnumerator = tokenStream.GetEnumerator();
                undoBuffer = new LinkedList<string>();
            }

            /// <summary>
            /// Attempts to retrieve a token matching the definition associated
            /// with the given name.
            /// </summary>
            /// <param name="tokenName">The name of the token to try to retrieve.</param>
            /// <returns>
            /// A result object describing whether the match was a success and what value 
            /// was found.
            /// </returns>
            public TokenResult GetToken(string tokenName)
            {
                if (!tokenizer.Exists(tokenName))
                {
                    throw new ArgumentException(Resources.UnknownTokenType, "tokenName");
                }
                string token = GetToken();
                bool isMatch = token == null ? false : tokenizer.Match(tokenName, token);
                return new TokenResult(tokenName, isMatch, token);
            }

            /// <summary>
            /// Attempts to retrieve the next token.
            /// </summary>
            /// <returns>The next token -or- null if there are no more tokens.</returns>
            public string GetToken()
            {
                if (undoBuffer.Count == 0)
                {
                    if (!tokenEnumerator.MoveNext())
                    {
                        return null;
                    }
                    return tokenEnumerator.Current;
                }
                else
                {
                    string token = undoBuffer.First.Value;
                    undoBuffer.RemoveFirst();
                    return token;
                }
            }

            /// <summary>
            /// Restores the given token to the front of the token stream.
            /// </summary>
            /// <param name="token">The token result containing the token to restore.</param>
            public void PutBack(string token)
            {
                undoBuffer.AddFirst(token);
            }
        }
    }
}
