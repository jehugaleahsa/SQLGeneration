using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a condition to a where clause.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Gets or sets how the filter is combined with others.
        /// </summary>
        Conjunction Conjunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to wrap the filter in parentheses.
        /// </summary>
        bool? WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a string representation of the filter.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        IEnumerable<string> GetFilterExpression(CommandOptions options);
    }
}
