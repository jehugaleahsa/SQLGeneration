using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Expressions;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a grouping of filters.
    /// </summary>
    public class FilterGroup : Filter
    {
        private readonly List<IFilter> _filters;

        /// <summary>
        /// Initializes a new instance of a FilterGroup.
        /// </summary>
        public FilterGroup()
        {
            _filters = new List<IFilter>();
        }

        /// <summary>
        /// Gets the filters in the filter group.
        /// </summary>
        public IEnumerable<IFilter> Filters
        {
            get
            {
                return new ReadOnlyCollection<IFilter>(_filters);
            }
        }

        /// <summary>
        /// Adds the filter to the group.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        public void AddFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            _filters.Add(filter);
        }

        /// <summary>
        /// Removes the filter from the group.
        /// </summary>
        /// <param name="filter">The filter to remove.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        public bool RemoveFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return _filters.Remove(filter);
        }

        /// <summary>
        /// Gets whether there are any filters in the group.
        /// </summary>
        public bool HasFilters
        {
            get
            {
                return _filters.Count > 0;
            }
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="expression">The filter expression being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override void GetInnerFilterExpression(Expression expression, CommandOptions options)
        {
            // <Filter> [ {"AND"|"OR"} <Filter> ]
            if (_filters.Count == 0)
            {
                throw new SQLGenerationException(Resources.EmptyFilterGroup);
            }
            expression.AddItem(buildFilterTree(options, 0));
        }

        private IExpressionItem buildFilterTree(CommandOptions options, int filterIndex)
        {
            if (filterIndex == _filters.Count - 1)
            {
                IFilter current = _filters[filterIndex];
                return current.GetFilterExpression(options);
            }
            else
            {
                IFilter current = _filters[filterIndex];
                IFilter next = _filters[filterIndex + 1];

                IExpressionItem left = current.GetFilterExpression(options);
                IExpressionItem right = buildFilterTree(options, filterIndex + 1);

                ConjunctionConverter converter = new ConjunctionConverter();
                Expression filterExpression = new Expression();
                filterExpression.AddItem(left);
                filterExpression.AddItem(converter.ToToken(next.Conjunction));
                filterExpression.AddItem(right);
                return filterExpression;
            }
        }

        /// <summary>
        /// Determines whether the filter should be surrounded by parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>True if the filter should be surround by parentheses; otherwise, false.</returns>
        protected override bool ShouldWrapInParentheses(CommandOptions options)
        {
            return (WrapInParentheses ?? false) || (options.WrapFiltersInParentheses && _filters.Any(filter => filter.Conjunction == Conjunction.Or));
        }
    }
}
