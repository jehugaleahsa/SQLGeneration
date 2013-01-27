using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a join between two tables, joins or sub-queries.
    /// </summary>
    public abstract class Join : IJoinItem
    {
        private readonly IJoinItem _leftHand;
        private readonly IJoinItem _rightHand;

        /// <summary>
        /// Initializes a new instance of a InnerJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        protected Join(IJoinItem leftHand, IJoinItem rightHand)
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
        /// Gets or sets whether the join should be wrapped in parentheses.
        /// </summary>
        public bool? WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the item on the left hand side of the join.
        /// </summary>
        public IJoinItem LeftHand
        {
            get
            {
                return _leftHand;
            }
        }

        /// <summary>
        /// Gets the item on the right hand side of the join.
        /// </summary>
        public IJoinItem RightHand
        {
            get
            {
                return _rightHand;
            }
        }

        IExpressionItem IJoinItem.GetDeclarationExpression(CommandOptions options, FilterGroup where)
        {
            Expression expression = new Expression();
            if (WrapInParentheses ?? options.WrapJoinsInParentheses)
            {
                expression.AddItem(new Token("("));
            }
            IExpressionItem leftHand = _leftHand.GetDeclarationExpression(options, where);
            IExpressionItem rightHand = _rightHand.GetDeclarationExpression(options, where);
            IExpressionItem combined = combine(options, leftHand, rightHand);
            expression.AddItem(combined);
            expression.AddItem(GetOnExpression(options));
            if (WrapInParentheses ?? options.WrapJoinsInParentheses)
            {
                expression.AddItem(new Token(")"));
            }
            return expression;
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <param name="options">The configuration settings to use.</param>
        /// <returns>The generated text.</returns>
        protected abstract IExpressionItem GetOnExpression(CommandOptions options);

        /// <summary>
        /// Combines the left and right items with the type of join.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <param name="leftHand">The left item.</param>
        /// <param name="rightHand">The right item.</param>
        /// <returns>A string combining the left and right items with a join.</returns>
        private IExpressionItem combine(CommandOptions options, IExpressionItem leftHand, IExpressionItem rightHand)
        {
            Expression expression = new Expression();
            expression.AddItem(leftHand);
            expression.AddItem(GetJoinNameExpression(options));
            expression.AddItem(rightHand);
            return expression;
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected abstract IExpressionItem GetJoinNameExpression(CommandOptions options);
    }
}
