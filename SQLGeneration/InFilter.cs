using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter where the values on the left hand must be in the values on the right hand.
    /// </summary>
    public class InFilter : ComparisonFilter
    {
        /// <summary>
        /// Initializes a new instance of a InFilter.
        /// </summary>
        /// <param name="leftHand">The left hand value that must exist in the list of values.</param>
        /// <param name="values">The list of values the left hand must exist in.</param>
        public InFilter(IFilterItem leftHand, IValueProvider values)
            : base(leftHand, values)
        {
        }

        /// <summary>
        /// Combines the left and right hand operands with the operation.
        /// </summary>
        /// <param name="expression">The filter expression being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <param name="leftHand">The left hand operand.</param>
        /// <param name="rightHand">The right hand operand.</param>
        /// <returns>A string combining the left and right hand operands with the operation.</returns>
        protected override void Combine(Expression expression, CommandOptions options, IExpressionItem leftHand, IExpressionItem rightHand)
        {
            // <Left> "IN" <ValueList>
            expression.AddItem(leftHand);
            expression.AddItem(new Token("IN"));
            expression.AddItem(rightHand);
        }
    }
}
