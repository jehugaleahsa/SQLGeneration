using System;
using System.Text;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
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
        internal FullOuterJoin(Join leftHand, AliasedSource rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override TokenResult GetJoinType(CommandOptions options)
        {
            StringBuilder result = new StringBuilder("FULL ");
            if (options.VerboseOuterJoin)
            {
                result.Append("OUTER ");
            }
            result.Append("JOIN");
            return new TokenResult(SqlTokenRegistry.FullOuterJoin, result.ToString());
        }
    }
}
