using System;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter in a where clause.
    /// </summary>
    public abstract class Filter : IFilter
    {
        private Conjunction _conjunction;
        private bool _not;
        private bool _wrapInParentheses;

        /// <summary>
        /// Creates a new Filter.
        /// </summary>
        protected Filter()
        {
            _wrapInParentheses = true;
        }

        /// <summary>
        /// Gets or sets how the filter is combined with others.
        /// </summary>
        public Conjunction Conjunction
        {
            get
            {
                return _conjunction;
            }
            set
            {
                if (!Enum.IsDefined(typeof(Conjunction), value))
                {
                    throw new ArgumentException(Resources.UnknownConjunction, "value");
                }
                _conjunction = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to apply a not to the expression.
        /// </summary>
        public bool Not
        {
            get
            {
                return _not;
            }
            set
            {
                _not = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to wrap the filter in parentheses.
        /// </summary>
        public bool WrapInParentheses
        {
            get
            {
                return _wrapInParentheses;
            }
            set
            {
                _wrapInParentheses = value;
            }
        }

        string IFilter.GetFilterText()
        {
            StringBuilder result = new StringBuilder();
            if (_not)
            {
                result.Append("NOT (");
            }
            else if (_wrapInParentheses)
            {
                result.Append("(");
            }
            result.Append(GetFilterText());
            if (_not || _wrapInParentheses)
            {
                result.Append(")");
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <returns>A string representing the filter.</returns>
        protected abstract string GetFilterText();
    }
}
