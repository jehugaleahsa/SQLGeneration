using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a filter in a where clause.
    /// </summary>
    public abstract class Filter : IFilter
    {
        /// <summary>
        /// Initializes a new instance of a Filter.
        /// </summary>
        protected Filter()
        {
        }

        /// <summary>
        /// Gets or sets whether to wrap the filter in parentheses.
        /// </summary>
        public bool? WrapInParentheses
        {
            get;
            set;
        }

        TokenStream IFilter.GetFilterTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            bool wrapInParentheses = ShouldWrapInParentheses(options);
            if (wrapInParentheses)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.LeftParenthesis, "("));
            }
            stream.AddRange(GetInnerFilterTokens(options));
            if (wrapInParentheses)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.RightParenthesis, ")"));
            }
            return stream;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected abstract TokenStream GetInnerFilterTokens(CommandOptions options);

        /// <summary>
        /// Determines whether the filter should be surrounded by parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>True if the filter should be surround by parentheses; otherwise, false.</returns>
        protected virtual bool ShouldWrapInParentheses(CommandOptions options)
        {
            return WrapInParentheses ?? options.WrapFiltersInParentheses;
        }
    }
}
