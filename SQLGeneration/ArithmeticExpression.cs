using System;
using System.Collections.Generic;

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

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            return getExpression(options);
        }

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            return getExpression(options);
        }

        IEnumerable<string> IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            return getExpression(options);
        }

        private IEnumerable<string> getExpression(CommandOptions options)
        {
            // <Arithmetic> => [ "(" ] <Left> <Op> <Right> [ ")" ]
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                yield return "(";
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
            foreach (string token in formatter.GetUnaliasedReference(_leftHand))
            {
                yield return token;
            }
            yield return GetOperatorName(options);
            foreach (string token in formatter.GetUnaliasedReference(_rightHand))
            {
                yield return token;
            }
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                yield return ")";
            }
        }

        /// <summary>
        /// Gets the token representing the arithmetic operator.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The token representing the arithmetic operator.</returns>
        protected abstract string GetOperatorName(CommandOptions options);
    }
}
