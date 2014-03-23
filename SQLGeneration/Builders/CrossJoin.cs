using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a cross join.
    /// </summary>
    public class CrossJoin : BinaryJoin
    {
        /// <summary>
        /// Initializes a new instance of a CrossJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand table in the join.</param>
        internal CrossJoin(Join leftHand, AliasedSource rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <param name="options">The configuration settings to use.</param>
        /// <returns>The generated text.</returns>
        protected override TokenStream GetOnTokens(CommandOptions options)
        {
            return new TokenStream();
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override TokenResult GetJoinType(CommandOptions options)
        {
            return new TokenResult(SqlTokenRegistry.CrossJoin, "CROSS JOIN");
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected override void OnAccept(BuilderVisitor visitor)
        {
            visitor.VisitCrossJoin(this);
        }
    }
}
