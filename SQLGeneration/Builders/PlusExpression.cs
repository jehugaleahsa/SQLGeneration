using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the addition of two items in a command.
    /// </summary>
    public class PlusExpression : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a PlusExpression.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public PlusExpression(IProjectionItem leftHand, IProjectionItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the token representing the arithmetic operator.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The token representing the arithmetic operator.</returns>
        protected override string GetOperator(CommandOptions options)
        {
            // <Left> "+" <Right>
            return "+";
        }
    }
}
