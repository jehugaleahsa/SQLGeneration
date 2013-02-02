using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds all of the items from the first query to the second.
    /// </summary>
    public class UnionAll : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a UnionAll.
        /// </summary>
        /// <param name="leftHand">The left hand SELECT command.</param>
        /// <param name="rightHand">The right hand SELECT command.</param>
        public UnionAll(ISelectBuilder leftHand, ISelectBuilder rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected override string GetCombinationName(CommandOptions options)
        {
            // "UNION ALL"
            return "UNION ALL";
        }
    }
}
