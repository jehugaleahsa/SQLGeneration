using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Logically negates another filter.
    /// </summary>
    public class NotFilter : Filter
    {
        private readonly IFilter filter;

        /// <summary>
        /// Initializes a new instance of a NotFilter.
        /// </summary>
        /// <param name="filter">The filter to negate.</param>
        public NotFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            this.filter = filter;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterExpression(CommandOptions options)
        {
            // "NOT" "(" <Filter> ")"
            yield return "NOT";
            yield return "(";
            foreach (string token in filter.GetFilterExpression(options))
            {
                yield return token;
            }
            yield return ")";
        }
    }
}
