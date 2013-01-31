using System;
using System.Text;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a full-outer join in a select statement.
    /// </summary>
    public class FullOuterJoin : FilteredJoin
    {
        /// <summary>
        /// Initializes a new instance of a FullOuterJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand table in the join.</param>
        public FullOuterJoin(IJoinItem leftHand, Table rightHand)
            : base(leftHand, rightHand, new IFilter[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of a FullOuterJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand table in the join.</param>
        /// <param name="filters">The filters to join to the join items on.</param>
        public FullOuterJoin(IJoinItem leftHand, Table rightHand, params IFilter[] filters)
            : base(leftHand, rightHand, filters)
        {
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override Token GetJoinNameExpression(CommandOptions options)
        {
            // { "FULL OUTER JOIN" | "FULL JOIN" }
            StringBuilder result = new StringBuilder("FULL ");
            if (options.VerboseOuterJoin)
            {
                result.Append("OUTER ");
            }
            result.Append("JOIN");
            return new Token(result.ToString(), TokenType.JoinType);
        }
    }
}
