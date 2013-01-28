using System;
using System.Text;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an inner join in a select statement.
    /// </summary>
    public class InnerJoin : FilteredJoin
    {
        /// <summary>
        /// Initializes a new instance of a InnerJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        public InnerJoin(IJoinItem leftHand, IRightJoinItem rightHand)
            : base(leftHand, rightHand, new IFilter[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of a InnerJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        /// <param name="filters">The filters to join to the join items on.</param>
        public InnerJoin(IJoinItem leftHand, IRightJoinItem rightHand, params IFilter[] filters)
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
            // { "JOIN" | "INNER JOIN" }
            StringBuilder result = new StringBuilder();
            if (options.VerboseInnerJoin)
            {
                result.Append("INNER ");
            }
            result.Append("JOIN");
            return new Token(result.ToString());
        }
    }
}
