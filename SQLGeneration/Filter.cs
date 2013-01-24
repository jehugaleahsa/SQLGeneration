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

        /// <summary>
        /// Initializes a new instance of a Filter.
        /// </summary>
        protected Filter()
        {
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
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to wrap the filter in parentheses.
        /// </summary>
        public bool? WrapInParentheses
        {
            get;
            set;
        }

        string IFilter.GetFilterText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (Not)
            {
                result.Append("NOT ");
            }
            bool wrapInParentheses = ShouldWrapInParentheses(context);
            if (Not || wrapInParentheses)
            {
                result.Append("(");
            }
            result.Append(GetFilterText(context));
            if (Not || wrapInParentheses)
            {
                if (context.Options.OneFilterPerLine)
                {
                    result.Append(context.GetIndentationText());
                }
                result.Append(")");
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected abstract string GetFilterText(BuilderContext context);

        /// <summary>
        /// Determines whether the filter should be surrounded by parentheses.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>True if the filter should be surround by parentheses; otherwise, false.</returns>
        protected virtual bool ShouldWrapInParentheses(BuilderContext context)
        {
            return WrapInParentheses ?? context.Options.WrapFiltersInParentheses;
        }
    }
}
