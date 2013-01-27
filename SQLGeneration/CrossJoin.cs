using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a cross join.
    /// </summary>
    public class CrossJoin : Join
    {
        /// <summary>
        /// Initializes a new instance of a CrossJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        public CrossJoin(IJoinItem leftHand, IJoinItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <param name="expression">The expression currently being built.</param>
        /// <param name="options">The configuration settings to use.</param>
        /// <returns>The generated text.</returns>
        protected override void GetOnExpression(Expression expression, CommandOptions options)
        {
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override IExpressionItem GetJoinNameExpression(CommandOptions options)
        {
            return new Token("CROSS JOIN");
        }
    }
}
