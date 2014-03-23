using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Generates the intersection among all of the queries.
    /// </summary>
    public class Intersect : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a Intersect.
        /// </summary>
        /// <param name="leftHand">The left hand SELECT command.</param>
        /// <param name="rightHand">The right hand SELECT command.</param>
        public Intersect(ISelectBuilder leftHand, ISelectBuilder rightHand)
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
            return new TokenResult(SqlTokenRegistry.Intersect, "INTERSECT");
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected override void OnAccept(BuilderVisitor visitor)
        {
            visitor.VisitIntersect(this);
        }
    }
}
