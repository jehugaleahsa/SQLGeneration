using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SQLGeneration.Properties;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Generates a series of tokens.
    /// </summary>
    public abstract class Tokenizer : ITokenRegistry, ITokenSource
    {
        private readonly Dictionary<string, List<Regex>> tokenLookup;
        private readonly IEnumerator<string> tokenEnumerator;
        private readonly LinkedList<string> undoBuffer;

        /// <summary>
        /// Initializes a new instance of a Tokenizer.
        /// </summary>
        /// <param name="tokenStream">The stream of tokens.</param>
        protected Tokenizer(IEnumerable<string> tokenStream)
        {
            tokenLookup = new Dictionary<string, List<Regex>>();
            tokenEnumerator = tokenStream.GetEnumerator();
            undoBuffer = new LinkedList<string>();
        }

        /// <summary>
        /// Associates the given token name to the regular expression that
        /// tokens of that type are expected to match.
        /// </summary>
        /// <param name="tokenName">The type of the token to associate the regular expression with.</param>
        /// <param name="regex">The regular expression that the token is expected match.</param>
        /// <param name="options">Additional regular expression options to apply.</param>
        /// <remarks>
        /// Multiple regular expressions can be registered to the same token name.
        /// They will be tried in the order that they are defined.
        /// </remarks>
        public void Define(string tokenName, string regex, RegexOptions options = RegexOptions.None)
        {
            List<Regex> checks;
            if (!tokenLookup.TryGetValue(tokenName, out checks))
            {
                checks = new List<Regex>();
                tokenLookup.Add(tokenName, checks);
            }
            Regex check = new Regex("^" + regex + "$", RegexOptions.Compiled & options);
            checks.Add(check);
        }

        /// <summary>
        /// Gets whether a token with the given name has been registered.
        /// </summary>
        /// <param name="tokenName">The name of the token to search for.</param>
        /// <returns>True if the token has been registered; otherwise, false.</returns>
        public bool Exists(string tokenName)
        {
            return tokenLookup.ContainsKey(tokenName);
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
            List<Regex> checks;
            if (!tokenLookup.TryGetValue(tokenName, out checks))
            {
                throw new ArgumentException(Resources.UnknownTokenType, "tokenName");
            }
            string token;
            if (undoBuffer.Count == 0)
            {
                if (!tokenEnumerator.MoveNext())
                {
                    return new TokenResult(tokenName, false, null);
                }
                token = tokenEnumerator.Current;
            }
            else
            {
                token = undoBuffer.First.Value;
                undoBuffer.RemoveFirst();
            }
            foreach (Regex check in checks)
            {
                if (check.IsMatch(token))
                {
                    return new TokenResult(tokenName, true, token);
                }
            }
            return new TokenResult(tokenName, false, token);
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
