using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison where the left hand item is less than the right hand item.
    /// </summary>
    public class LessThanFilter : ComparisonFilter
    {
        /// <summary>
        /// Initializes a new instance of a LessThanFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public LessThanFilter(IFilterItem leftHand, IFilterItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <param name="leftHand">The left hand side of the comparison.</param>
        /// <param name="rightHand">The right hand side of the comparison.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IExpressionItem Combine(CommandOptions options, IExpressionItem leftHand, IExpressionItem rightHand)
        {
            Expression expression = new Expression();
            expression.AddItem(leftHand);
            expression.AddItem(new Token("<"));
            expression.AddItem(rightHand);
            return expression;
        }
    }
}
