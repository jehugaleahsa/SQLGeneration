using System;
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

        TokenStream IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        TokenStream IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        TokenStream IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        private TokenStream getTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.MinusOperator, "-"));
            bool wrapInParentheses = shouldWrapInParentheses(options);
            if (wrapInParentheses)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.LeftParenthesis, "("));
            }
            stream.AddRange(Item.GetProjectionTokens(options));
            if (wrapInParentheses)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.RightParenthesis, ")"));
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
