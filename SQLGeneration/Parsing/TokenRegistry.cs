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
    public abstract class TokenRegistry : ITokenRegistry
    {
        private readonly Dictionary<string, TokenDefinition> definitionLookup;
        private Regex regex;

        /// <summary>
        /// Initializes a new instance of a TokenRegistry.
        /// </summary>
        protected TokenRegistry()
        {
            definitionLookup = new Dictionary<string, TokenDefinition>();
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
            if (definitionLookup.ContainsKey(tokenName))
            {
                string message = String.Format(Resources.DuplicateTokenDefinition, tokenName);
                throw new SQLGenerationException(message);
            }
            TokenDefinition definition = new TokenDefinition()
            {
                Type = tokenName,
                Regex = regex,
                IgnoreCase = ignoreCase,
            };
            definitionLookup.Add(tokenName, definition);
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

        /// <summary>
        /// Extracts the next token from the given input string, starting at the given index.
        /// </summary>
        /// <param name="input">The input string to get the next token from.</param>
        /// <param name="index">The index into the string to start searching for a token.</param>
        /// <returns>The extracted token -or- null if no token is found.</returns>
        private string GetToken(string input, ref int index)
        {
            Regex regex = getRegex();
            Match match = regex.Match(input, index);
            if (match.Success)
            {
                index = match.Index + match.Length;
                return match.Value;
            }
            else
            {
                index = input.Length;
                return null;
            }
        }

        private bool Match(string tokenName, string token)
        {
            Regex regex = getRegex();
            Match match = regex.Match(token, 0);
            return match.Success && match.Groups[tokenName].Success;
        }

        private Regex getRegex()
        {
            if (regex == null)
            {
                string pattern = String.Join("|", definitionLookup.Keys.Select(tokenName => getTokenRegex(definitionLookup[tokenName])));
                regex = new Regex(@"\G" + pattern, RegexOptions.Compiled);
            }
            return regex;
        }

        private string getTokenRegex(TokenDefinition definition)
        {
            StringBuilder regexBuilder = new StringBuilder();
            regexBuilder.Append("(?<");
            regexBuilder.Append(definition.Type);
            regexBuilder.Append(">");
            if (definition.IgnoreCase)
            {
                regexBuilder.Append("(?i)");
            }
            regexBuilder.Append(definition.Regex);
            if (definition.IgnoreCase)
            {
                regexBuilder.Append("(?-i)");
            }
            regexBuilder.Append(")");
            return regexBuilder.ToString();
        }

        private sealed class TokenDefinition
        {
            public string Type { get; set; }

            public string Regex { get; set; }

            public bool IgnoreCase { get; set; }
        }

        /// <summary>
        /// Creates a stream of tokens by tokenizing the given string,
        /// verifying the tokens against the token definitions.
        /// </summary>
        /// <param name="commandText">The input stream containing the tokens.</param>
        /// <returns>The new token source.</returns>
        public ITokenSource CreateTokenSource(string commandText)
        {
            return new TokenSource(this, tokenize(commandText));
        }

        private IEnumerable<string> tokenize(string input)
        {
            int index = 0;
            while (index != input.Length)
            {
                string token = GetToken(input, ref index);
                if (token == null)
                {
                    yield break;
                }
                yield return token;
            }
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

        private sealed class TokenSource : ITokenSource
        {
            private readonly TokenRegistry registry;
            private readonly IEnumerator<string> tokenEnumerator;
            private readonly LinkedList<string> undoBuffer;

            /// <summary>
            /// Initializes a new instance of a TokenSource.
            /// </summary>
            /// <param name="registry">The registry containing the token definitions.</param>
            /// <param name="tokenStream">A stream of tokens.</param>
            public TokenSource(TokenRegistry registry, IEnumerable<string> tokenStream)
            {
                this.registry = registry;
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
                if (!registry.Exists(tokenName))
                {
                    throw new ArgumentException(Resources.UnknownTokenType, "tokenName");
                }
                string token = GetToken();
                bool isMatch = token == null ? false : registry.Match(tokenName, token);
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
