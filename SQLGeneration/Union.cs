using System;

namespace SQLGeneration
{
    /// <summary>
    /// Unions the items from the first query to the second.
    /// </summary>
    public class Union : SelectCombiner
    {
        /// <summary>
        /// Creates a new Union.
        /// </summary>
        public Union()
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <returns>The text used to combine two queries.</returns>
        protected override string GetCombinationString()
        {
            return "UNION";
        }
    }
}
