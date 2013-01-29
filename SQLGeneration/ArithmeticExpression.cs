using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an arithmetic expression in a command.
    /// </summary>
    public abstract class ArithmeticExpression : IArithmetic
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

        void IProjectionItem.GetProjectionExpression(Expression expression, CommandOptions options)
        {
            getExpression(expression, options);
        }

        void IFilterItem.GetFilterExpression(Expression expression, CommandOptions options)
        {
            getExpression(expression, options);
        }

        void IGroupByItem.GetGroupByExpression(Expression expression, CommandOptions options)
        {
            getExpression(expression, options);
        }

        private void getExpression(Expression expression, CommandOptions options)
        {
            // [ "(" ] <Left> <Op> <Right> [ ")" ]
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                expression.AddItem(new Token("("));
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
            IExpressionItem leftHand = formatter.GetUnaliasedReference(_leftHand);
            IExpressionItem rightHand = formatter.GetUnaliasedReference(_rightHand);
            IExpressionItem combined = Combine(options, leftHand, rightHand);
            expression.AddItem(combined);
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                expression.AddItem(new Token(")"));
            }
        }

        /// <summary>
        /// Combines with the left hand operand with the right hand operand using the operation.
        /// </summary>
        /// <param name="leftHand">The left hand operand.</param>
        /// <param name="rightHand">The right hand operand.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The left and right hand operands combined using the operation.</returns>
        protected abstract IExpressionItem Combine(CommandOptions options, IExpressionItem leftHand, IExpressionItem rightHand);
    }
}
