using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an inner join in a select statement.
    /// </summary>
    public class InnerJoin : Join, IInnerJoin
    {
        /// <summary>
        /// Initializes a new instance of a InnerJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        public InnerJoin(IJoinItem leftHand, IJoinItem rightHand)
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
            return leftHand + " INNER JOIN " + rightHand;
        }
    }
}
