using System;

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
        /// Combines with the left hand operand with the right hand operand using the operation.
        /// </summary>
        /// <param name="leftHand">The left hand operand.</param>
        /// <param name="rightHand">The right hand operand.</param>
        /// <returns>The left and right hand operands combined using the operation.</returns>
        protected override string Combine(string leftHand, string rightHand)
        {
            return leftHand + " - " + rightHand;
        }
    }
}
