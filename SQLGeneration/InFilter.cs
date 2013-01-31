using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter where the values on the left hand must be in the values on the right hand.
    /// </summary>
    public class InFilter : Filter
    {
        private readonly IFilterItem leftHand;
        private readonly IValueProvider values;

        /// <summary>
        /// Initializes a new instance of a InFilter.
        /// </summary>
        /// <param name="leftHand">The left hand value that must exist in the list of values.</param>
        /// <param name="values">The list of values the left hand must exist in.</param>
        public InFilter(IFilterItem leftHand, IValueProvider values)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            this.leftHand = leftHand;
            this.values = values;
        }

        /// <summary>
        /// Gets the left hand operand of the filter.
        /// </summary>
        public IFilterItem LeftHand
        {
            get { return leftHand; }
        }

        /// <summary>
        /// Gets the value provider.
        /// </summary>
        public IValueProvider Values
        {
            get { return values; }
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="expression">The filter expression being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override void GetInnerFilterExpression(Expression expression, CommandOptions options)
        {
            // <InFilter> => <Left> "IN" <Right>
            Expression inExpression = new Expression(ExpressionItemType.InFilter);
            leftHand.GetFilterExpression(inExpression, options);
            inExpression.AddItem(new Token("IN", TokenType.Keyword));
            values.GetFilterExpression(inExpression, options);
            expression.AddItem(inExpression);
        }
    }
}
