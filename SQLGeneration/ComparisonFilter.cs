using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter that compares two values together.
    /// </summary>
    public abstract class ComparisonFilter : Filter, IComparisonFilter
    {
        private readonly IFilterItem _leftHand;
        private readonly IFilterItem _rightHand;

        /// <summary>
        /// Initializes a new instance of a ComparisonFilter.
        /// </summary>
        /// <param name="leftHand">The left hand side of the comparison.</param>
        /// <param name="rightHand">The right hand side of the comparison.</param>
        protected ComparisonFilter(IFilterItem leftHand, IFilterItem rightHand)
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
        /// Gets a string representing the filter.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override sealed IExpressionItem GetInnerFilterExpression(CommandOptions options)
        {
            IExpressionItem leftHand = _leftHand.GetFilterExpression(options);
            IExpressionItem rightHand = _rightHand.GetFilterExpression(options);
            return Combine(options, leftHand, rightHand);
        }

        /// <summary>
        /// Combines the left and right hand operands with the operation.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <param name="leftHand">The left hand operand.</param>
        /// <param name="rightHand">The right hand operand.</param>
        /// <returns>A string combining the left and right hand operands with the operation.</returns>
        protected abstract IExpressionItem Combine(CommandOptions options, IExpressionItem leftHand, IExpressionItem rightHand);
    }
}
