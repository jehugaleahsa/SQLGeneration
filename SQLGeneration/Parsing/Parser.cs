using System;
using System.Collections.Generic;
using SQLGeneration.Properties;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Parses a sequence of tokens using a grammar, applying actions to matching sequences.
    /// </summary>
    public sealed class Parser
    {
        private readonly Grammar grammar;
        private readonly Dictionary<string, Func<MatchResult, object>> handlerLookup;

        /// <summary>
        /// Initializes a new instance of a Parser.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        public Parser(Grammar grammar)
        {
            if (grammar == null)
            {
                throw new ArgumentNullException("grammar");
            }
            this.grammar = grammar;
            this.handlerLookup = new Dictionary<string, Func<MatchResult, object>>();
        }

        /// <summary>
        /// Registers the given handler to run when the given expression type is matched.
        /// </summary>
        /// <param name="expressionType">The type of the expression item to handle.</param>
        /// <param name="handler">The function to call when the expression type is matched.</param>
        /// <remarks>If a handler is already registered, it will be replaced.</remarks>
        public void RegisterHandler(string expressionType, Func<MatchResult, object> handler)
        {
            if (String.IsNullOrWhiteSpace(expressionType))
            {
                throw new ArgumentException(Resources.BlankItemName, "itemName");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            handlerLookup[expressionType] = handler;
        }

        /// <summary>
        /// Parses the given token source using the specified grammar, starting with
        /// expression with the given name.
        /// </summary>
        /// <param name="expressionType">The type of the expression to start parsing.</param>
        /// <param name="tokenSource">The source of tokens.</param>
        /// <returns>The result of the handler registered for the starting item.</returns>
        public object Parse(string expressionType, ITokenSource tokenSource)
        {
            if (tokenSource == null)
            {
                throw new ArgumentNullException("tokenSource");
            }
            Expression expression = grammar.Expression(expressionType);
            ParseAttempt attempt = new ParseAttempt(this, tokenSource);
            MatchResult result = expression.Match(attempt, String.Empty);
            return result.Context;
        }

        /// <summary>
        /// Attempts to run the handle for the given expression type.
        /// </summary>
        /// <param name="expressionType">The type of the expression to run the handler for.</param>
        /// <param name="result">The result that the handler should be run for.</param>
        internal void RunHandler(string expressionType, MatchResult result)
        {
            Func<MatchResult, object> handler;
            if (result.IsMatch && expressionType != null && handlerLookup.TryGetValue(expressionType, out handler))
            {
                result.Context = handler(result);
            }
        }

        private sealed class ParseAttempt : IParseAttempt
        {
            private readonly Parser parser;
            private readonly ITokenSource tokenSource;
            private readonly List<string> tokens;

            /// <summary>
            /// Initializes a new instance of a ParseAttempt.
            /// </summary>
            /// <param name="parser">The parser containing</param>
            /// <param name="tokenSource">An object to retrieve the sequence of tokens from.</param>
            public ParseAttempt(Parser parser, ITokenSource tokenSource)
            {
                this.parser = parser;
                this.tokenSource = tokenSource;
                this.tokens = new List<string>();
            }

            /// <summary>
            /// Gets the tokens collected during the attempt.
            /// </summary>
            public List<string> Tokens
            {
                get { return tokens; }
            }

            /// <summary>
            /// Runs the handle for the given expression type, if it is specified.
            /// </summary>
            /// <param name="expressionType">The type of the expression to run the handler for.</param>
            /// <param name="result">The results of the parse for the expression.</param>
            public void RunHandler(string expressionType, MatchResult result)
            {
                parser.RunHandler(expressionType, result);
            }

            /// <summary>
            /// Attempts to get a token of the given type.
            /// </summary>
            /// <param name="tokenName">The type of the token to attempt to retrieve.</param>
            /// <returns>The result of the search.</returns>
            public TokenResult GetToken(string tokenName)
            {
                TokenResult result = tokenSource.GetToken(tokenName);
                if (result.Value != null)
                {
                    tokens.Add(result.Value);
                }
                return result;
            }

            /// <summary>
            /// Creates an attempt to parse a child expression.
            /// </summary>
            /// <returns>A new attempt object.</returns>
            public IParseAttempt Attempt()
            {
                return new ParseAttempt(parser, tokenSource);
            }

            /// <summary>
            /// Accepts the attempt as a successful parse, joining the given attempt's tokens
            /// with the current attempt's.
            /// </summary>
            /// <param name="attempt">The child attempt to accept.</param>
            public void Accept(IParseAttempt attempt)
            {
                tokens.AddRange(attempt.Tokens);
            }

            /// <summary>
            /// Rejects the attempt as a failed parse, returning the attempt's token
            /// to the token stream.
            /// </summary>
            public void Reject()
            {
                int index = tokens.Count;
                while (index != 0)
                {
                    --index;
                    tokenSource.PutBack(tokens[index]);
                }
            }
        }
    }
}
