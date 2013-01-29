using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents the substraction of two items in a command.
    /// </summary>
    public class MinusExpression : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a MinusExpression.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public MinusExpression(IProjectionItem leftHand, IProjectionItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the token representing the arithmetic operator.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The token representing the arithmetic operator.</returns>
        protected override Token GetOperatorName(CommandOptions options)
        {
            // <Left> "-" <Right>
            return new Token("-");
        }
    }
}
