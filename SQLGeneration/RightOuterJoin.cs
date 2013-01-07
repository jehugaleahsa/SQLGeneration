using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an right-outer join in a select statement.
    /// </summary>
    public class RightOuterJoin : Join, IRightOuterJoin
    {
        /// <summary>
        /// Creates a new RightOuterJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        public RightOuterJoin(IJoinItem leftHand, IJoinItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Combines the left and right items with the type of join.
        /// </summary>
        /// <param name="leftHand">The left item.</param>
        /// <param name="rightHand">The right item.</param>
        /// <returns>A string combining the left and right items with a join.</returns>
        protected override string Combine(string leftHand, string rightHand)
        {
            return leftHand + " RIGHT OUTER JOIN " + rightHand;
        }
    }
}
