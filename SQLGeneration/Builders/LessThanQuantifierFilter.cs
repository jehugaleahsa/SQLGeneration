using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a filter that see that a value is less than all or some of the values.
    /// </summary>
    public class LessThanQuantifierFilter : QuantifierFilter
    {
        /// <summary>
        /// Initializes a new insstance of an LessThanQuantifierFilter.
        /// </summary>
        /// <param name="leftHand">The value being compared to the set of values.</param>
        /// <param name="quantifier">The quantifier to use to compare the value to the set of values.</param>
        /// <param name="valueProvider">The source of values.</param>
        public LessThanQuantifierFilter(IFilterItem leftHand, Quantifier quantifier, IValueProvider valueProvider)
            : base(leftHand, quantifier, valueProvider)
        {
        }

        /// <summary>
        /// Gets the comparison operator applied to the value set.
        /// </summary>
        /// <param name="options">The configuration settings to use when building the command.</param>
        /// <returns>The token representing the comparison operator.</returns>
        protected override string GetComparisonOperator(CommandOptions options)
        {
            return "<";
        }
    }
}
