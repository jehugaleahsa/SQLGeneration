using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison that checks that an item is between two values.
    /// </summary>
    public class BetweenFilter : Filter, IBetweenFilter
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
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <returns>A string representing the filter.</returns>
        protected override string GetFilterText()
        {
            StringBuilder result = new StringBuilder(_value.GetFilterItemText());
            result.Append(" BETWEEN ");
            ProjectionItemFormatter formatter = new ProjectionItemFormatter();
            result.Append(_lowerBound.GetFilterItemText());
            result.Append(" AND ");
            result.Append(_upperBound.GetFilterItemText());
            return result.ToString();
        }
    }
}
