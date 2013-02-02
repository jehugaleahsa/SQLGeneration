using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a join between two tables, joins or sub-queries.
    /// </summary>
    public abstract class Join : IJoinItem
    {
        private readonly IJoinItem _leftHand;
        private readonly IRightJoinItem _rightHand;

        /// <summary>
        /// Initializes a new instance of a InnerJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item or join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        protected Join(IJoinItem leftHand, IRightJoinItem rightHand)
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
        /// Gets the table on the right hand side of the join.
        /// </summary>
        public IRightJoinItem RightHand
        {
            get
            {
                return _rightHand;
            }
        }

        IEnumerable<string> IJoinItem.GetDeclarationExpression(CommandOptions options)
        {
            // <Join> => [ "(" ] <Left> <Combiner> <Right> [ "ON" <Filter> ] [ ")" ]
            if (WrapInParentheses ?? options.WrapJoinsInParentheses)
            {
                yield return "(";
            }
            foreach (string token in _leftHand.GetDeclarationExpression(options))
            {
                yield return token;
            }
            yield return GetJoinNameExpression(options);
            foreach (string token in _rightHand.GetDeclarationExpression(options))
            {
                yield return token;
            }
            foreach (string token in GetOnExpression(options))
            {
                yield return token;
            }
            if (WrapInParentheses ?? options.WrapJoinsInParentheses)
            {
                yield return ")";
            }
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <param name="options">The configuration settings to use.</param>
        /// <returns>The generated text.</returns>
        protected abstract IEnumerable<string> GetOnExpression(CommandOptions options);

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected abstract string GetJoinNameExpression(CommandOptions options);
    }
}
