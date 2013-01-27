﻿using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison where the left hand item is greater than the right hand item.
    /// </summary>
    public class GreaterThanFilter : ComparisonFilter
    {
        /// <summary>
        /// Initializes a new instance of a GreaterThanFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public GreaterThanFilter(IFilterItem leftHand, IFilterItem rightHand)
            : base(leftHand, rightHand)
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
            // <left> ">" <Right>
            expression.AddItem(leftHand);
            expression.AddItem(new Token(">"));
            expression.AddItem(rightHand);
        }
    }
}
