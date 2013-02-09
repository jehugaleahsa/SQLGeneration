using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the substraction of two items in a command.
    /// </summary>
    public class Subtraction : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a Subtraction.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public Subtraction(IProjectionItem leftHand, IProjectionItem rightHand)
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
            return "-";
        }
    }
}
