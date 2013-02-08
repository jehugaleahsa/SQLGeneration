using System;
using System.Collections.Generic;
using System.Linq;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a grouping of filters.
    /// </summary>
    public class FilterGroup : Filter
    {
        private readonly List<Tuple<IFilter, Conjunction>> _filters;

        /// <summary>
        /// Initializes a new instance of a FilterGroup.
        /// </summary>
        public FilterGroup(Conjunction conjunction = Conjunction.And, params IFilter[] filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }
            _filters = new List<Tuple<IFilter, Conjunction>>();
            foreach (IFilter filter in filters)
            {
                _filters.Add(Tuple.Create(filter, conjunction));
            }
        }

        /// <summary>
        /// Gets the filters in the filter group.
        /// </summary>
        public IEnumerable<IFilter> Filters
        {
            get
            {
                return _filters.Select(pair => pair.Item1).ToArray();
            }
        }

        /// <summary>
        /// Adds the filter to the group.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        /// <param name="conjunction">Specifies whether to AND or OR the filter with the other filters in the group.</param>
        public void AddFilter(IFilter filter, Conjunction conjunction = Conjunction.And)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            _filters.Add(Tuple.Create(filter, conjunction));
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
            return _filters.RemoveAll(pair => pair.Item1 == filter) != 0;
        }

        /// <summary>
        /// Gets whether there are any filters in the group.
        /// </summary>
        public bool HasFilters
        {
            get { return _filters.Count > 0; }
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override IEnumerable<string> GetInnerFilterTokens(CommandOptions options)
        {
            // <FilterList> => <Filter> [ {"AND"|"OR"} <FilterList> ]
            using (IEnumerator<Tuple<IFilter, Conjunction>> enumerator = _filters.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new SQLGenerationException(Resources.EmptyFilterGroup);
                }
                TokenStream stream = new TokenStream();
                stream.AddRange(enumerator.Current.Item1.GetFilterTokens(options));
                while (enumerator.MoveNext())
                {
                    ConjunctionConverter converter = new ConjunctionConverter();
                    stream.Add(converter.ToToken(enumerator.Current.Item2));
                    stream.AddRange(enumerator.Current.Item1.GetFilterTokens(options));
                }
                return stream;
            }
        }

        /// <summary>
        /// Determines whether the filter should be surrounded by parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>True if the filter should be surround by parentheses; otherwise, false.</returns>
        protected override bool ShouldWrapInParentheses(CommandOptions options)
        {
            return (WrapInParentheses ?? false) || (options.WrapFiltersInParentheses && _filters.Any(pair => pair.Item2 == Conjunction.Or));
        }
    }
}
