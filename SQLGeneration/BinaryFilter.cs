using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter that compares two values together.
    /// </summary>
    public abstract class BinaryFilter : Filter, IComparisonFilter
    {
        private readonly IFilterItem _leftHand;
        private readonly IFilterItem _rightHand;

        /// <summary>
        /// Initializes a new instance of a BinaryFilter.
        /// </summary>
        /// <param name="leftHand">The left hand side of the comparison.</param>
        /// <param name="rightHand">The right hand side of the comparison.</param>
        protected BinaryFilter(IFilterItem leftHand, IFilterItem rightHand)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            _leftHand = leftHand;
            _rightHand = rightHand;
        }

        /// <summary>
        /// Gets the left hand operand of the filter.
        /// </summary>
        public IFilterItem LeftHand
        {
            get { return _leftHand; }
        }

        /// <summary>
        /// Gets the right hand operand of the comparison.
        /// </summary>
        public IFilterItem RightHand
        {
            get { return _rightHand; }
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterTokens(CommandOptions options)
        {
            // <Binary> => <Left> <Op> <Right>
            TokenStream stream = new TokenStream();
            stream.AddRange(_leftHand.GetFilterTokens(options));
            stream.Add(GetCombinerName(options));
            stream.AddRange(_rightHand.GetFilterTokens(options));
            return stream;
        }

        /// <summary>
        /// Gets the operator that will compare the left and right hand values.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string containing the name of the operation that compares the left and right hand sides.</returns>
        protected abstract string GetCombinerName(CommandOptions options);
    }
}
