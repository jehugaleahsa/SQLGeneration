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
        private readonly ITokenSource tokenSource;
        private readonly Grammar grammar;
        private readonly Dictionary<string, Func<MatchResult, object>> handlerLookup;
        private readonly Stack<List<string>> tokenStack;

        /// <summary>
        /// Initializes a new instance of a Parser.
        /// </summary>
        /// <param name="tokenSource">The source of tokens.</param>
        /// <param name="grammar">The grammar to use.</param>
        public Parser(ITokenSource tokenSource, Grammar grammar)
        {
            if (tokenSource == null)
            {
                throw new ArgumentNullException("tokenSource");
            }
            if (grammar == null)
            {
                throw new ArgumentNullException("grammar");
            }
            this.tokenSource = tokenSource;
            this.grammar = grammar;
            this.handlerLookup = new Dictionary<string, Func<MatchResult, object>>();
            this.tokenStack = new Stack<List<string>>();
            this.tokenStack.Push(new List<string>());
        }

        /// <summary>
        /// Attempts to get a token of the given type.
        /// </summary>
        /// <param name="tokenName">The type of the token to attempt to retrieve.</param>
        /// <returns>The result of the search.</returns>
        internal TokenResult GetToken(string tokenName)
        {
            TokenResult result = tokenSource.GetToken(tokenName);
            if (result.Value != null)
            {
                List<string> topTokens = tokenStack.Peek();
                topTokens.Add(result.Value);
            }
            return result;
        }

        /// <summary>
        /// Creates a logical sequence of tokens that can be rolled back
        /// if the containing expression fails to match.
        /// </summary>
        internal void StartTransaction()
        {
            tokenStack.Push(new List<string>());
        }

        /// <summary>
        /// Indicates that the tokens in the current expression matched
        /// the expected syntax and should be included in the parent
        /// expressions token list.
        /// </summary>
        internal void Commit()
        {
            List<string> oldTopTokens = tokenStack.Pop();
            List<string> newTopTokens = tokenStack.Peek();
            newTopTokens.AddRange(oldTopTokens);
        }

        /// <summary>
        /// Indicates that the tokens in the current expression did
        /// not match the expected syntax and should be restored to
        /// the token source.
        /// </summary>
        internal void Rollback()
        {
            List<string> oldTopTokens = tokenStack.Pop();
            int index = oldTopTokens.Count;
            while (index != 0)
            {
                --index;
                tokenSource.PutBack(oldTopTokens[index]);
            }
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
        /// <returns>The result of the handler registered for the starting item.</returns>
        public object Parse(string expressionType)
        {
            Expression expression = grammar.Expression(expressionType);
            MatchResult result = expression.Match(this, 0);
            Func<MatchResult, object> handler;
            if (handlerLookup.TryGetValue(expressionType, out handler))
            {
                return handler(result);
            }
            return null;
        }
    }
}
