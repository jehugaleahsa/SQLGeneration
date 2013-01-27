using System;
using System.Collections.Generic;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Represents a sequence of tokens making up a SQL statement expression.
    /// </summary>
    public class Expression : IExpressionItem
    {
        private static readonly IExpressionItem none = new Expression();

        private readonly List<IExpressionItem> items;

        /// <summary>
        /// Gets an empty expression that should be ignored.
        /// </summary>
        public static IExpressionItem None
        {
            get { return none; }
        }

        /// <summary>
        /// Initializes a new instance of an Expression.
        /// </summary>
        public Expression()
        {
            items = new List<IExpressionItem>();
        }

        /// <summary>
        /// Gets or sets the expression's parent expression.
        /// </summary>
        public Expression Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Adds the given expression or token to the expression.
        /// </summary>
        /// <param name="item">The expression or token to add.</param>
        /// <returns>The updated expression.</returns>
        public Expression AddItem(IExpressionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Parent = this;
            items.Add(item);
            return this;
        }

        /// <summary>
        /// Visits the current expression item.
        /// </summary>
        /// <param name="visiter">A function that will be passed a token when it is encountered.</param>
        public void Visit(Action<Token> visiter)
        {
            foreach (IExpressionItem item in items)
            {
                item.Visit(visiter);
            }
        }

        /// <summary>
        /// Creates a new expression where each given expression or token is separated by the given token.
        /// </summary>
        /// <param name="separator">The token to separate the expressions with.</param>
        /// <param name="expressions">The tokens or expressions to separate.</param>
        /// <returns>An expression separating the given expressions with the given token.</returns>
        public static IExpressionItem Join(IExpressionItem separator, IEnumerable<IExpressionItem> expressions)
        {
            using (IEnumerator<IExpressionItem> enumerator = expressions.GetEnumerator())
            {
                Expression expression = new Expression();
                if (enumerator.MoveNext())
                {
                    expression.AddItem(enumerator.Current);
                }
                while (enumerator.MoveNext())
                {
                    expression.AddItem(separator);
                    expression.AddItem(enumerator.Current);
                }
                return expression;
            }
        }
    }
}
