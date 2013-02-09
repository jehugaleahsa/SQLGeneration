using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the addition of two items in a command.
    /// </summary>
    public class Addition : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a Addition.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public Addition(IProjectionItem leftHand, IProjectionItem rightHand)
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
            return "+";
        }
    }
}
