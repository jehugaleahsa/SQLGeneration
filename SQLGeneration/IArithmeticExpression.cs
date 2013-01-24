using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds an arithmetical expression to a command.
    /// </summary>
    public interface IArithmeticExpression : IArithmetic
    {
        /// <summary>
        /// Gets or sets whether to wrap the expression in parentheses.
        /// </summary>
        bool? WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the left hand operand of the expression.
        /// </summary>
        IProjectionItem LeftHand
        {
            get;
        }

        /// <summary>
        /// Gets the right hand operand of the expression.
        /// </summary>
        IProjectionItem RightHand
        {
            get;
        }
    }
}
