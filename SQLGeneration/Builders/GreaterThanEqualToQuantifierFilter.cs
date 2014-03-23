using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a filter that see that a value is greater than or equal to all or some of the values.
    /// </summary>
    public class GreaterThanEqualToQuantifierFilter : QuantifierFilter
    {
        /// <summary>
        /// Initializes a new insstance of an GreaterThanEqualToQuantifierFilter.
        /// </summary>
        /// <param name="leftHand">The value being compared to the set of values.</param>
        /// <param name="quantifier">The quantifier to use to compare the value to the set of values.</param>
        /// <param name="valueProvider">The source of values.</param>
        public GreaterThanEqualToQuantifierFilter(IFilterItem leftHand, Quantifier quantifier, IValueProvider valueProvider)
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
            return new TokenResult(SqlTokenRegistry.GreaterThanEqualTo, ">=");
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected override void OnAccept(BuilderVisitor visitor)
        {
            visitor.VisitGreaterThanEqualToQuantifierFilter(this);
        }
    }
}
