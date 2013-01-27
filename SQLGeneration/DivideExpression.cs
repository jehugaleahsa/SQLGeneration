using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents the division of two items in a command.
    /// </summary>
    public class DivideExpression : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a DivideExpression.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public DivideExpression(IProjectionItem leftHand, IProjectionItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Combines with the left hand operand with the right hand operand using the operation.
        /// </summary>
        /// <param name="leftHand">The left hand operand.</param>
        /// <param name="rightHand">The right hand operand.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The left and right hand operands combined using the operation.</returns>
        protected override IExpressionItem Combine(CommandOptions options, IExpressionItem leftHand, IExpressionItem rightHand)
        {
            Expression expression = new Expression();
            expression.AddItem(leftHand);
            expression.AddItem(new Token("/"));
            expression.AddItem(rightHand);
            return expression;
        }
    }
}
