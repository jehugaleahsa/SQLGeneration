using System;
using System.Text;

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
        internal InnerJoin(Join leftHand, AliasedSource rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override string GetJoinType(CommandOptions options)
        {
            // { "JOIN" | "INNER JOIN" }
            StringBuilder result = new StringBuilder();
            if (options.VerboseInnerJoin)
            {
                result.Append("INNER ");
            }
            result.Append("JOIN");
            return result.ToString();
        }
    }
}
