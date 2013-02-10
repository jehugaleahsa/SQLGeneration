using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a comparison where the left hand item is less than the right hand item.
    /// </summary>
    public class LessThanFilter : OrderFilter
    {
        /// <summary>
        /// Initializes a new instance of a LessThanFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public LessThanFilter(IFilterItem leftHand, IFilterItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the operator that will compare the left and right hand values.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string containing the name of the operation that compares the left and right hand sides.</returns>
        protected override string GetComparisonOperator(CommandOptions options)
        {
            return "<";
        }
    }
}
