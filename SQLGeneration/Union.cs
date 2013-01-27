using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Unions the items from the first query to the second.
    /// </summary>
    public class Union : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a Union.
        /// </summary>
        public Union()
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected override IExpressionItem GetCombinationExpression(CommandOptions options)
        {
            return new Token("UNION");
        }
    }
}
