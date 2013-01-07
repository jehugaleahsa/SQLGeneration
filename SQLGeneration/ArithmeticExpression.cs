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
        private bool _wrapInParentheses;
        private string _alias;

        /// <summary>
        /// Creates a new ArithmeticExpression.
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
            _wrapInParentheses = true;
        }

        /// <summary>
        /// Gets or sets whether to wrap the expression in parentheses.
        /// </summary>
        public bool WrapInParentheses
        {
            get
            {
                return _wrapInParentheses;
            }
            set
            {
                _wrapInParentheses = value;
            }
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
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        string IProjectionItem.GetFullText()
        {
            StringBuilder result = new StringBuilder();
            if (_wrapInParentheses)
            {
                result.Append("(");
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter();
            string leftHand = formatter.GetUnaliasedReference(_leftHand);
            string rightHand = formatter.GetUnaliasedReference(_rightHand);
            result.Append(Combine(leftHand, rightHand));
            if (_wrapInParentheses)
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
        /// <returns>The left and right hand operands combined using the operation.</returns>
        protected abstract string Combine(string leftHand, string rightHand);

        string IFilterItem.GetFilterItemText()
        {
            StringBuilder result = new StringBuilder();
            if (_wrapInParentheses)
            {
                result.Append("(");
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter();
            string leftHand = formatter.GetUnaliasedReference(_leftHand);
            string rightHand = formatter.GetUnaliasedReference(_rightHand);
            result.Append(Combine(leftHand, rightHand));
            if (_wrapInParentheses)
            {
                result.Append(")");
            }
            return result.ToString();
        }

        string IGroupByItem.GetGroupByItemText()
        {
            StringBuilder result = new StringBuilder();
            if (_wrapInParentheses)
            {
                result.Append("(");
            }
            ProjectionItemFormatter formatter = new ProjectionItemFormatter();
            string leftHand = formatter.GetUnaliasedReference(_leftHand);
            string rightHand = formatter.GetUnaliasedReference(_rightHand);
            result.Append(Combine(leftHand, rightHand));
            if (_wrapInParentheses)
            {
                result.Append(")");
            }
            return result.ToString();
        }
    }
}
