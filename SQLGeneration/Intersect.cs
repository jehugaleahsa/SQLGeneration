using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Generates the intersection among all of the queries.
    /// </summary>
    public class Intersect : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a Intersect.
        /// </summary>
        public Intersect()
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected override IExpressionItem GetCombinationExpression(CommandOptions options)
        {
            // "INTERSECT"
            return new Token("INTERSECT");
        }
    }
}
