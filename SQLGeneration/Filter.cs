using System;
using SQLGeneration.Expressions;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter in a where clause.
    /// </summary>
    public abstract class Filter : IFilter
    {
        private Conjunction _conjunction;

        /// <summary>
        /// Initializes a new instance of a Filter.
        /// </summary>
        protected Filter()
        {
        }

        /// <summary>
        /// Gets or sets how the filter is combined with others.
        /// </summary>
        public Conjunction Conjunction
        {
            get
            {
                return _conjunction;
            }
            set
            {
                if (!Enum.IsDefined(typeof(Conjunction), value))
                {
                    throw new ArgumentException(Resources.UnknownConjunction, "value");
                }
                _conjunction = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to wrap the filter in parentheses.
        /// </summary>
        public bool? WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the text for the filter expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The filter text.</returns>
        public IExpressionItem GetFilterExpression(CommandOptions options)
        {
            // <Filter> => [ "(" ] <Filter> [ ")" ]
            Expression expression = new Expression(ExpressionItemType.Filter);
            bool wrapInParentheses = ShouldWrapInParentheses(options);
            if (wrapInParentheses)
            {
                expression.AddItem(new Token("(", TokenType.LeftParenthesis));
            }
            GetInnerFilterExpression(expression, options);
            if (wrapInParentheses)
            {
                expression.AddItem(new Token(")", TokenType.RightParenthesis));
            }
            return expression;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="expression">The filter expression being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected abstract void GetInnerFilterExpression(Expression expression, CommandOptions options);

        /// <summary>
        /// Determines whether the filter should be surrounded by parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>True if the filter should be surround by parentheses; otherwise, false.</returns>
        protected virtual bool ShouldWrapInParentheses(CommandOptions options)
        {
            return WrapInParentheses ?? options.WrapFiltersInParentheses;
        }
    }
}
