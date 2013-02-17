using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a filter that see that a value is not equal to all or some of the values.
    /// </summary>
    public class NotEqualToQuantifierFilter : QuantifierFilter
    {
        /// <summary>
        /// Initializes a new insstance of an NotEqualToQuantifierFilter.
        /// </summary>
        /// <param name="leftHand">The value being compared to the set of values.</param>
        /// <param name="quantifier">The quantifier to use to compare the value to the set of values.</param>
        /// <param name="valueProvider">The source of values.</param>
        public NotEqualToQuantifierFilter(IFilterItem leftHand, Quantifier quantifier, IValueProvider valueProvider)
            : base(leftHand, quantifier, valueProvider)
        {
        }

        /// <summary>
        /// Gets the comparison operator applied to the value set.
        /// </summary>
        /// <param name="options">The configuration settings to use when building the command.</param>
        /// <returns>The token representing the comparison operator.</returns>
        protected override TokenResult GetComparisonOperator(CommandOptions options)
        {
            return new TokenResult(SqlTokenRegistry.NotEqualTo,  "<>");
        }
    }
}
