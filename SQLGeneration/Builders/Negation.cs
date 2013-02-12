using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Negates an arithmetic expression.
    /// </summary>
    public class Negation : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Initializes a new instance of a Negation.
        /// </summary>
        /// <param name="item">The item to negate.</param>
        public Negation(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            Item = item;
        }

        /// <summary>
        /// Gets the item that will be negated.
        /// </summary>
        public IProjectionItem Item 
        { 
            get; 
            private set; 
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        private IEnumerable<string> getTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("-");
            bool wrapInParentheses = shouldWrapInParentheses(options);
            if (wrapInParentheses)
            {
                stream.Add("(");
            }
            stream.AddRange(Item.GetProjectionTokens(options));
            if (wrapInParentheses)
            {
                stream.Add(")");
            }
            return stream;
        }

        private bool shouldWrapInParentheses(CommandOptions options)
        {
            ArithmeticExpression expression = Item as ArithmeticExpression;
            if (expression == null || (expression.WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses))
            {
                return false;
            }
            return true;
        }
    }
}
