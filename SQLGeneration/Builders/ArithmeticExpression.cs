using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents an arithmetic expression in a command.
    /// </summary>
    public abstract class ArithmeticExpression : IProjectionItem, IFilterItem, IGroupByItem
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

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        private IEnumerable<string> getTokens(CommandOptions options)
        {
            // <Arithmetic> => [ "(" ] <Left> <Op> <Right> [ ")" ]
            TokenStream stream = new TokenStream();
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                stream.Add("(");
            }
            stream.AddRange(_leftHand.GetProjectionTokens(options));
            stream.Add(GetOperator(options));
            stream.AddRange(_rightHand.GetProjectionTokens(options));
            if (WrapInParentheses ?? options.WrapArithmeticExpressionsInParentheses)
            {
                stream.Add(")");
            }
            return stream;
        }

        /// <summary>
        /// Gets the token representing the arithmetic operator.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The token representing the arithmetic operator.</returns>
        protected abstract string GetOperator(CommandOptions options);
    }
}
