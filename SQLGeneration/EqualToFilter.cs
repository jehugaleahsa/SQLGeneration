using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison between two items for equality.
    /// </summary>
    public class EqualToFilter : ComparisonFilter
    {
        /// <summary>
        /// Creates a new EqualToFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public EqualToFilter(IFilterItem leftHand, IFilterItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <returns>A string representing the filter.</returns>
        protected override string Combine(string leftHand, string rightHand)
        {
            return leftHand + " = " + rightHand;
        }
    }
}
