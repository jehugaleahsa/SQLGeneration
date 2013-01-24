using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a join between two tables, joins or sub-queries.
    /// </summary>
    public abstract class Join : IJoin
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

        string IJoinItem.GetDeclaration(BuilderContext context, IFilterGroup where)
        {
            StringBuilder result = new StringBuilder();
            if (WrapInParentheses ?? context.Options.WrapJoinsInParentheses)
            {
                result.Append("(");
            }
            string leftHand = _leftHand.GetDeclaration(context, where);
            BuilderContext next = context;
            if (context.Options.IndentJoinItems)
            {
                next = next.Indent();
            }
            string rightHand = _rightHand.GetDeclaration(next, where);
            result.Append(combine(next, leftHand, rightHand));
            result.Append(' ');
            result.Append(GetOnExpression(context));
            if (WrapInParentheses ?? context.Options.WrapJoinsInParentheses)
            {
                result.Append(")");
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <returns>The generated text.</returns>
        protected abstract string GetOnExpression(BuilderContext context);

        /// <summary>
        /// Combines the left and right items with the type of join.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <param name="leftHand">The left item.</param>
        /// <param name="rightHand">The right item.</param>
        /// <returns>A string combining the left and right items with a join.</returns>
        private string combine(BuilderContext context, string leftHand, string rightHand)
        {
            StringBuilder result = new StringBuilder();
            result.Append(leftHand);
            if (context.Options.OneJoinItemPerLine)
            {
                result.AppendLine();
                result.Append(context.GetIndentationText());
            }
            else
            {
                result.Append(' ');
                if (context.Options.AliasJoinItemsUsingAs)
                {
                    result.Append("AS ");
                }
            }
            result.Append(GetJoinName(context));
            result.Append(' ');
            result.Append(rightHand);
            return result.ToString();
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected abstract string GetJoinName(BuilderContext context);
    }
}
