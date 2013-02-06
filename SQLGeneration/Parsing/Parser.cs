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
        private readonly Dictionary<string, Action<MatchResult, object>> handlerLookup;
        private Action<string, object> tokenHandler;

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
            this.handlerLookup = new Dictionary<string, Action<MatchResult, object>>();
        }

        /// <summary>
        /// Registers the given handler to run when a token is matched.
        /// </summary>
        /// <param name="handler">The function to call when a token is matched.</param>
        public void RegisterTokenHandle(Action<string, object> handler)
        {
            tokenHandler = handler;
        }

        /// <summary>
        /// Registers the given handler to run when the given expression type is matched.
        /// </summary>
        /// <param name="expressionType">The type of the expression item to handle.</param>
        /// <param name="handler">The function to call when the expression type is matched.</param>
        /// <remarks>If a handler is already registered, it will be replaced.</remarks>
        public void RegisterHandler(string expressionType, Action<MatchResult, object> handler)
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
        /// <param name="context">The initial context to pass to the expression handlers.</param>
        public void Parse(string expressionType, ITokenSource tokenSource, object context)
        {
            if (tokenSource == null)
            {
                throw new ArgumentNullException("tokenSource");
            }
            if (!hasAllhandlers())
            {
                throw new SQLGenerationException(Resources.MissingHandlers);
            }
            Expression expression = grammar.Expression(expressionType);
            ParseAttempt attempt = new ParseAttempt(this, tokenSource);
            MatchResult result = expression.Match(attempt, String.Empty);
            string token = tokenSource.GetToken();
            if (token != null)
            {
                if (result.IsMatch)
                {
                    result.GetContext(context);
                }
                string message = String.Format(Resources.UnexpectedToken, token);
                throw new SQLGenerationException(message);
            }
            result.GetContext(context);
        }

        private bool hasAllhandlers()
        {
            foreach (ExpressionDefinition definition in grammar.Definitions)
            {
                if (!handlerLookup.ContainsKey(definition.ExpressionType))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Attempts to run the handle for a token.
        /// </summary>
        /// <param name="result">The result that the handler should be run for.</param>
        /// <param name="token">The token that was found by the parser.</param>
        private void SetTokenHandler(MatchResult result, string token)
        {
            if (result.IsMatch)
            {
                result.Handler = (r, c) => tokenHandler(token, c);
            }
        }

        /// <summary>
        /// Attempts to run the handle for the given expression type.
        /// </summary>
        /// <param name="expressionType">The type of the expression to run the handler for.</param>
        /// <param name="result">The result that the handler should be run for.</param>
        private void SetHandler(string expressionType, MatchResult result)
        {
            Action<MatchResult, object> handler;
            if (result.IsMatch && expressionType != null && handlerLookup.TryGetValue(expressionType, out handler))
            {
                result.Handler = handler;
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
            /// Sets the handle for the token.
            /// </summary>
            /// <param name="result">The results of the parse for the token.</param>
            /// <param name="token">The token found by the parser.</param>
            public void SetTokenHandler(MatchResult result, string token)
            {
                parser.SetTokenHandler(result, token);
            }

            /// <summary>
            /// Sets the handle for the given expression type, if it is specified.
            /// </summary>
            /// <param name="expressionType">The type of the expression to run the handler for.</param>
            /// <param name="result">The results of the parse for the expression.</param>
            public void SetHandler(string expressionType, MatchResult result)
            {
                parser.SetHandler(expressionType, result);
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
