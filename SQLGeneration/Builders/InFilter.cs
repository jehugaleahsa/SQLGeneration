using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a filter where the values on the left hand must be in the values on the right hand.
    /// </summary>
    public class InFilter : Filter
    {
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
            LeftHand = leftHand;
            Values = values;
        }

        /// <summary>
        /// Gets or sets whether to negate the comparison.
        /// </summary>
        public bool Not
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the left hand operand of the filter.
        /// </summary>
        public IFilterItem LeftHand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value provider.
        /// </summary>
        public IValueProvider Values
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterTokens(CommandOptions options)
        {
            // <InFilter> => <Left> [ "NOT" ] "IN" <ValueList>
            TokenStream stream = new TokenStream();
            stream.AddRange(LeftHand.GetFilterTokens(options));
            if (Not)
            {
                stream.Add("NOT");
            }
            stream.Add("IN");
            stream.AddRange(Values.GetFilterTokens(options));
            return stream;
        }
    }
}
