using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an arithmetic expression in a command.
    /// </summary>
    public abstract class ArithmeticExpression : IArithmeticExpression
    {
        private readonly IProjectionItem _leftHand;
        private readonly IProjectionItem _rightHand;

        /// <summary>
        /// Initializes a new instance of a ArithmeticExpression.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        protected ArithmeticExpression(IProjectionItem leftHand, IProjectionItem rightHand)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            _leftHand = leftHand;
            _rightHand = rightHand;
        }

        /// <summary>
        /// Gets or sets whether to wrap the expression in parentheses.
        /// </summary>
        public bool? WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the left hand operand of the expression.
        /// </summary>
        public IProjectionItem LeftHand
        {
            get
            {
                return _leftHand;
            }
        }

        /// <summary>
        /// Gets the right hand operand of the expression.
        /// </summary>
        public IProjectionItem RightHand
        {
            get
            {
                return _rightHand;
            }
        }

        /// <summary>
        /// Gets or sets the alias for the arithmetic expression.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return getExpressionText(context);
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return getExpressionText(context);
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return getExpressionText(context);
        }

        private string getExpressionText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (WrapInParentheses ?? context.Options.WrapArithmeticExpressionsInParentheses)
            {
                result.Append("(");
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(context);
            string leftHand = formatter.GetUnaliasedReference(_leftHand);
            string rightHand = formatter.GetUnaliasedReference(_rightHand);
            result.Append(Combine(context, leftHand, rightHand));
            if (WrapInParentheses ?? context.Options.WrapArithmeticExpressionsInParentheses)
            {
                result.Append(")");
            }
            return result.ToString();
        }

        /// <summary>
        /// Combines with the left hand operand with the right hand operand using the operation.
        /// </summary>
        /// <param name="leftHand">The left hand operand.</param>
        /// <param name="rightHand">The right hand operand.</param>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The left and right hand operands combined using the operation.</returns>
        protected abstract string Combine(BuilderContext context, string leftHand, string rightHand);
    }
}
