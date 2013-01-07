using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison between two items for inequality.
    /// </summary>
    public class NotEqualToFilter : ComparisonFilter
    {
        /// <summary>
        /// Initializes a new instance of a NotEqualToFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public NotEqualToFilter(IFilterItem leftHand, IFilterItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <returns>A string representing the filter.</returns>
        protected override string Combine(string leftHand, string rightHand)
        {
            return leftHand + " <> " + rightHand;
        }
    }
}
