using System;
using SQLGeneration.Expressions;
using Expressions = SQLGeneration.Expressions;

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
            // <Arithmetic> => [ "(" ] <Left> <Op> <Right> [ ")" ]
            Expression arithmeticExpression = new Expression(ExpressionItemType.Arithmetic);
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                arithmeticExpression.AddItem(new Token("(", TokenType.LeftParenthesis));
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
            arithmeticExpression.AddItem(formatter.GetUnaliasedReference(_leftHand));
            arithmeticExpression.AddItem(GetOperatorName(options));
            arithmeticExpression.AddItem(formatter.GetUnaliasedReference(_rightHand));
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                arithmeticExpression.AddItem(new Token(")", TokenType.RightParenthesis));
            }
            expression.AddItem(arithmeticExpression);
        }

        /// <summary>
        /// Gets the token representing the arithmetic operator.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The token representing the arithmetic operator.</returns>
        protected abstract Token GetOperatorName(CommandOptions options);
    }
}
