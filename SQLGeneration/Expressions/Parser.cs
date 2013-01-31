using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Supports navigating an expression tree, generating text along the way.
    /// </summary>
    public class Parser
    {
        private readonly IExpressionItem item;
        private readonly IEnumerator<IExpressionItem> enumerator;
        private LinkedList<IExpressionItem> putBack;

        /// <summary>
        /// Initializes a new instance of a Parser.
        /// </summary>
        /// <param name="item">The expression item to parse.</param>
        public Parser(IExpressionItem item)
        {
            this.item = item;
            this.putBack = new LinkedList<IExpressionItem>();
            if (this.item.Type == ExpressionItemType.Token)
            {
                this.enumerator = null;
                this.putBack.AddLast(item);
            }
            else
            {
                Expression expression = (Expression)item;
                this.enumerator = expression.Items.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets the expression item the parser is parsing.
        /// </summary>
        public IExpressionItem ExpressionItem 
        {
            get { return item; }
        }

        /// <summary>
        /// Puts a value back to be parsed later.
        /// </summary>
        /// <param name="items">The items to put back.</param>
        public void PutBack(IEnumerable<IExpressionItem> items)
        {
            Stack<IExpressionItem> reversed = new Stack<IExpressionItem>(items);
            foreach (IExpressionItem item in reversed)
            {
                putBack.AddFirst(item);
            }
        }

        /// <summary>
        /// Attempts to grab a token from the current expression.
        /// </summary>
        /// <param name="expected">The expected token.</param>
        /// <returns>The extracted token.</returns>
        public Parser GetNextToken(string expected)
        {
            Parser parser = GetNextParser(ExpressionItemType.Token);
            if (parser != null)
            {
                Token token = (Token)parser.ExpressionItem;
                if (token.Value != expected)
                {
                    return null;
                }
            }
            return parser;
        }

        /// <summary>
        /// Attempts to grab a parser for a sub-expression from the current expression.
        /// </summary>
        /// <param name="expectedTypes">The expected type of types of of the sub-expression.</param>
        /// <returns>A parser for the sub-expression.</returns>
        public Parser GetNextParser(params ExpressionItemType[] expectedTypes)
        {
            IExpressionItem next;
            if (putBack.Count > 0)
            {
                next = putBack.First.Value;
                if (!expectedTypes.Contains(next.Type))
                {
                    return null;
                }
                putBack.RemoveFirst();
                return new Parser(next);
            }
            if (!enumerator.MoveNext())
            {
                return null;
            }
            next = enumerator.Current;
            if (!expectedTypes.Contains(next.Type))
            {
                putBack.AddLast(enumerator.Current);
                return null;
            }
            return new Parser(next);
        }
    }
}
