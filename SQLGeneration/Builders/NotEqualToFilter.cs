﻿using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a comparison between two items for inequality.
    /// </summary>
    public class NotEqualToFilter : OrderFilter
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
        /// Gets the operator that will compare the left and right hand values.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string containing the name of the operation that compares the left and right hand sides.</returns>
        protected override string GetComparisonOperator(CommandOptions options)
        {
            // <Left> "<>" <Right>
            return "<>";
        }
    }
}