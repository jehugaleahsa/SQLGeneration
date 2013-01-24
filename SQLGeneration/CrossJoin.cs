using System;

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
        /// <returns>The generated text.</returns>
        protected override string GetOnExpression(BuilderContext context)
        {
            return String.Empty;
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override string GetJoinName(BuilderContext context)
        {
            return "CROSS JOIN";
        }
    }
}
