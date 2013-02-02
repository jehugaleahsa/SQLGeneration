using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison that checks that an item is between two values.
    /// </summary>
    public class BetweenFilter : Filter
    {
        private readonly IFilterItem _value;
        private readonly IFilterItem _lowerBound;
        private readonly IFilterItem _upperBound;

        /// <summary>
        /// Initializes a new instance of a NullFilter.
        /// </summary>
        /// <param name="value">The item to check whether is within a range.</param>
        /// <param name="lowerBound">The smallest value the item can be.</param>
        /// <param name="upperBound">The largest value the item can be.</param>
        public BetweenFilter(IFilterItem value, IFilterItem lowerBound, IFilterItem upperBound)
        {
            if (value == null)
            {
                throw new ArgumentNullException("item");
            }
            if (lowerBound == null)
            {
                throw new ArgumentNullException("lowerBound");
            }
            if (upperBound == null)
            {
                throw new ArgumentNullException("upperBound");
            }
            _value = value;
            _lowerBound = lowerBound;
            _upperBound = upperBound;
        }

        /// <summary>
        /// Gets the value being checked.
        /// </summary>
        public IFilterItem Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the least the value can be.
        /// </summary>
        public IFilterItem LowerBound
        {
            get { return _lowerBound; }
        }

        /// <summary>
        /// Gets the most the value can be.
        /// </summary>
        public IFilterItem UpperBound
        {
            get { return _upperBound; }
        }

        /// <summary>
        /// Gets or sets whether to negate the results of the filter.
        /// </summary>
        public bool Not
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterExpression(CommandOptions options)
        {
            // <Between> => <Value> [ "NOT" ] "BETWEEN" <Lower> "AND" <Upper>
            foreach (string token in _value.GetFilterExpression(options))
            {
                yield return token;
            }
            if (Not)
            {
                yield return "NOT";
            }
            yield return "BETWEEN";
            foreach (string token in _lowerBound.GetFilterExpression(options))
            {
                yield return token;
            }
            yield return "AND";
            foreach (string token in _upperBound.GetFilterExpression(options))
            {
                yield return token;
            }
        }
    }
}
