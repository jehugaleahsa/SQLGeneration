using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison where the left hand item is greater than or equal to the right hand item.
    /// </summary>
    public class LikeFilter : BinaryFilter
    {
        /// <summary>
        /// Initializes a new instance of a LikeFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public LikeFilter(IFilterItem leftHand, StringLiteral rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the operator that will compare the left and right hand values.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string containing the name of the operation that compares the left and right hand sides.</returns>
        protected override Token GetCombinerName(CommandOptions options)
        {
            // <Left> "LIKE" <StringLiteral>
            return new Token("LIKE", TokenType.ComparisonOperator);
        }
    }
}
