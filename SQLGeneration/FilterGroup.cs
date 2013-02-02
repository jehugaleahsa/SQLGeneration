using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterExpression(CommandOptions options)
        {
            // <FilterList> => <Filter> [ {"AND"|"OR"} <FilterList> ]
            if (_filters.Count == 0)
            {
                throw new SQLGenerationException(Resources.EmptyFilterGroup);
            }
            return buildFilterList(options, 0);
        }

        private IEnumerable<string> buildFilterList(CommandOptions options, int filterIndex)
        {
            if (filterIndex == _filters.Count - 1)
            {
                IFilter current = _filters[filterIndex];
                foreach (string token in current.GetFilterExpression(options))
                {
                    yield return token;
                }
            }
            else
            {
                IFilter current = _filters[filterIndex];
                IFilter next = _filters[filterIndex + 1];

                foreach (string token in current.GetFilterExpression(options))
                {
                    yield return token;
                }
                ConjunctionConverter converter = new ConjunctionConverter();
                yield return converter.ToToken(next.Conjunction);
                foreach (string token in buildFilterList(options, filterIndex + 1))
                {
                    yield return token;
                }
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
