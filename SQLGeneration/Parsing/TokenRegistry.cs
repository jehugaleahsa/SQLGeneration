using System;
using System.Collections.Generic;
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
        private readonly List<string> tokenNames;
        private readonly Dictionary<string, TokenDefinition> definitionLookup;
        private Dictionary<string, Regex> checks;

        /// <summary>
        /// Initializes a new instance of a TokenRegistry.
        /// </summary>
        protected TokenRegistry()
        {
            tokenNames = new List<string>();
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
            tokenNames.Add(tokenName);
            TokenDefinition definition = new TokenDefinition()
            {
                Type = tokenName,
                Regex = regex,
                IgnoreCase = ignoreCase,
            };
            definitionLookup.Add(tokenName, definition);
        }

        /// <summary>
        /// Gets whether a token with the given name has been registered.
        /// </summary>
        /// <param name="tokenName">The name of the token to search for.</param>
        /// <returns>True if the token has been registered; otherwise, false.</returns>
        public bool IsRegistered(string tokenName)
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
            Dictionary<string, Regex> checks = getRegex();
            foreach (string tokenName in tokenNames)
            {
                Regex regex = checks[tokenName];
                Match match = regex.Match(input, index);
                if (match.Success)
                {
                    index = match.Index + match.Length;
                    return match.Groups["Token"].Value;
                }
            }
            index = input.Length;
            return null;
        }

        private bool Match(string token, out string tokenName)
        {
            Dictionary<string, Regex> checks = getRegex();
            foreach (string name in tokenNames)
            {
                Regex regex = checks[name];
                Match match = regex.Match(token, 0);
                if (match.Success)
                {
                    tokenName = name;
                    return true;
                }
            }
            tokenName = null;
            return false;
        }

        private Dictionary<string, Regex> getRegex()
        {
            if (checks == null)
            {
                checks = new Dictionary<string, Regex>();
                foreach (string tokenName in definitionLookup.Keys)
                {
                    string pattern = getTokenRegex(definitionLookup[tokenName]);
                    Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                    checks.Add(tokenName, regex);
                }
            }
            return checks;
        }

        private string getTokenRegex(TokenDefinition definition)
        {
            StringBuilder regexBuilder = new StringBuilder();
            regexBuilder.Append(@"\G\s*(?<Token>");
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

        private IEnumerable<string> tokenize(string commandText)
        {
            int index = 0;
            while (index != commandText.Length)
            {
                string token = GetToken(commandText, ref index);
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
            private readonly Stack<TokenResult> undoBuffer;

            /// <summary>
            /// Initializes a new instance of a TokenSource.
            /// </summary>
            /// <param name="registry">The registry containing the token definitions.</param>
            /// <param name="tokenStream">A stream of tokens.</param>
            public TokenSource(TokenRegistry registry, IEnumerable<string> tokenStream)
            {
                this.registry = registry;
                tokenEnumerator = tokenStream.GetEnumerator();
                undoBuffer = new Stack<TokenResult>();
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
                if (!registry.IsRegistered(tokenName))
                {
                    throw new ArgumentException(Resources.UnknownTokenType, "tokenName");
                }
                if (undoBuffer.Count == 0)
                {
                    if (!tokenEnumerator.MoveNext())
                    {
                        return new TokenResult(null, false, null);
                    }
                    string token = tokenEnumerator.Current;
                    string name;
                    bool isMatch = registry.Match(token, out name);
                    isMatch &= tokenName == name;
                    return new TokenResult(name, isMatch, token);
                }
                else
                {
                    TokenResult tokenResult = undoBuffer.Pop();
                    bool isMatch = tokenResult.Name == tokenName;
                    return new TokenResult(tokenResult.Name, isMatch, tokenResult.Value);
                }
            }

            /// <summary>
            /// Gets whether the token source has more items.
            /// </summary>
            public bool HasMore
            {
                get
                {
                    if (undoBuffer.Count > 0)
                    {
                        return true;
                    }
                    if (tokenEnumerator.MoveNext())
                    {
                        string token = tokenEnumerator.Current;
                        string name;
                        bool isMatch = registry.Match(token, out name);
                        TokenResult result = new TokenResult(name, isMatch, token);
                        undoBuffer.Push(result);
                        return true;
                    }
                    return false;
                }
            }

            /// <summary>
            /// Restores the given token to the front of the token stream.
            /// </summary>
            /// <param name="result">The token to restore.</param>
            public void PutBack(TokenResult result)
            {
                undoBuffer.Push(result);
            }
        }
    }
}
