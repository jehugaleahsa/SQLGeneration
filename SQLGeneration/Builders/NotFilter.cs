using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
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
        protected override TokenStream GetInnerFilterTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Not, "NOT"));
            bool wrapInParenthesis = shouldWrapInParenthesis(options);
            if (wrapInParenthesis)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.LeftParenthesis, "("));
            }
            stream.AddRange(filter.GetFilterTokens(options));
            if (wrapInParenthesis)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.RightParenthesis, ")"));
            }
            return stream;
        }

        private bool shouldWrapInParenthesis(CommandOptions options)
        {
            FilterGroup group = filter as FilterGroup;
            if (group == null || group.Count == 1 || (group.WrapInParentheses ?? options.WrapFiltersInParentheses))
            {
                return false;
            }
            return true;
        }
    }
}
