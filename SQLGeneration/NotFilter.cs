using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Logically negates another filter.
    /// </summary>
    public class NotFilter : Filter
    {
        private readonly IFilter filter;

        /// <summary>
        /// Initializes a new instance of a NotFilter.
        /// </summary>
        /// <param name="filter">The filter to negate.</param>
        public NotFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.filter = filter;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="expression">The filter expression being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override void GetInnerFilterExpression(Expression expression, CommandOptions options)
        {
            // "NOT" "(" <Filter> ")"
            Expression filterExpression = new Expression(ExpressionItemType.NotFilter);
            filterExpression.AddItem(new Token("NOT"));
            filterExpression.AddItem(new Token("("));
            filterExpression.AddItem(filter.GetFilterExpression(options));
            filterExpression.AddItem(new Token(")"));
            expression.AddItem(filterExpression);
        }
    }
}
