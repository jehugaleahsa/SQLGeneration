using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter where the values on the left hand must be in the values on the right hand.
    /// </summary>
    public class InFilter : Filter
    {
        private readonly IFilterItem leftHand;
        private readonly IValueProvider values;

        /// <summary>
        /// Initializes a new instance of a InFilter.
        /// </summary>
        /// <param name="leftHand">The left hand value that must exist in the list of values.</param>
        /// <param name="values">The list of values the left hand must exist in.</param>
        public InFilter(IFilterItem leftHand, IValueProvider values)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            this.leftHand = leftHand;
            this.values = values;
        }

        /// <summary>
        /// Gets the left hand operand of the filter.
        /// </summary>
        public IFilterItem LeftHand
        {
            get { return leftHand; }
        }

        /// <summary>
        /// Gets the value provider.
        /// </summary>
        public IValueProvider Values
        {
            get { return values; }
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterExpression(CommandOptions options)
        {
            // <InFilter> => <Left> "IN" <Right>
            foreach (string token in leftHand.GetFilterExpression(options))
            {
                yield return token;
            }
            yield return "IN";
            foreach (string token in values.GetFilterExpression(options))
            {
                yield return token;
            }
        }
    }
}
