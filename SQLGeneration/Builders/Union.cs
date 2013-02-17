using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Unions the items from the first query to the second.
    /// </summary>
    public class Union : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a Union.
        /// </summary>
        /// <param name="leftHand">The left hand SELECT command.</param>
        /// <param name="rightHand">The right hand SELECT command.</param>
        public Union(ISelectBuilder leftHand, ISelectBuilder rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected override TokenResult GetCombinationType(CommandOptions options)
        {
            return new TokenResult(SqlTokenRegistry.Union, "UNION");
        }
    }
}
