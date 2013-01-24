using System;

namespace SQLGeneration
{
    /// <summary>
    /// Removes the item from the second query from the first query.
    /// </summary>
    public class Minus : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a Minus.
        /// </summary>
        public Minus()
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected override string GetCombinationString(BuilderContext context)
        {
            return "MINUS";
        }
    }
}
