using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a filter that checks that a value is less than, greater than or equal to.
    /// </summary>
    public abstract class OrderFilter : Filter, IComparisonFilter
    {
        private readonly IFilterItem _leftHand;
        private readonly IFilterItem _rightHand;

        /// <summary>
        /// Initializes a new instance of a OrderFilter.
        /// </summary>
        /// <param name="leftHand">The left hand side of the comparison.</param>
        /// <param name="rightHand">The right hand side of the comparison.</param>
        protected OrderFilter(IFilterItem leftHand, IFilterItem rightHand)
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
        protected override TokenStream GetInnerFilterTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(_leftHand.GetFilterTokens(options));
            stream.Add(GetComparisonOperator(options));
            stream.AddRange(_rightHand.GetFilterTokens(options));
            return stream;
        }

        /// <summary>
        /// Gets the operator that will compare the left and right hand values.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string containing the name of the operation that compares the left and right hand sides.</returns>
        protected abstract TokenResult GetComparisonOperator(CommandOptions options);
    }
}
