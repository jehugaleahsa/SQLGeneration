using System;
using System.Collections.Generic;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Represents a sequence of tokens and sub-expressions.
    /// </summary>
    public sealed class ExpressionDefinition
    {
        private readonly List<ExpressionItem> items;

        /// <summary>
        /// Initializes a new instance of an ExpressionDefinition.
        /// </summary>
        internal ExpressionDefinition()
        {
            items = new List<ExpressionItem>();
        }

        /// <summary>
        /// Indicates that the given item is the next expected, giving it a
        /// name and specifying whether it is required.
        /// </summary>
        /// <param name="itemName">The name that the token will be identified with in the outer expression.</param>
        /// <param name="isRequired">Indicates whether the token is required in the expression.</param>
        /// <param name="item">The expression item to add to the sequence.</param>
        /// <returns>The updated expression.</returns>
        public ExpressionDefinition Add(string itemName, bool isRequired, IExpressionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            items.Add(new ExpressionItem(itemName, isRequired, item));
            return this;
        }

        /// <summary>
        /// Indicates that the given sub-expression is the next expected, giving it a
        /// name and specifying whether it is required.
        /// </summary>
        /// <param name="itemName">The name that the token will be identified with in the outer expression.</param>
        /// <param name="isRequired">Indicates whether the token is required in the expression.</param>
        /// <param name="definition">The definition for the sub-expression.</param>
        /// <returns>The updated expression.</returns>
        public ExpressionDefinition Add(string itemName, bool isRequired, ExpressionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }
            IExpressionItem item = new Expression(definition);
            items.Add(new ExpressionItem(itemName, isRequired, item));
            return this;
        }

        /// <summary>
        /// Gets the items making up the expression.
        /// </summary>
        internal IEnumerable<ExpressionItem> Items
        {
            get { return items; }
        }
    }
}
