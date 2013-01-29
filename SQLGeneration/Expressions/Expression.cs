using System;
using System.Collections.Generic;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Represents a sequence of tokens making up a SQL statement expression.
    /// </summary>
    public class Expression : IExpressionItem
    {
        private static readonly IExpressionItem none = new Expression(ExpressionItemType.None);

        private readonly List<IExpressionItem> items;
        private readonly ExpressionItemType type;

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
        /// <param name="type">The type of the expression.</param>
        public Expression(ExpressionItemType type)
        {
            items = new List<IExpressionItem>();
            this.type = type;
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
        /// Gets the type of the expression.
        /// </summary>
        public ExpressionItemType Type
        {
            get { return type; }
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
            if (item.Type != ExpressionItemType.None)
            {
                item.Parent = this;
                items.Add(item);
            }
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
    }
}
