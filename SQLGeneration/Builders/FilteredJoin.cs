using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a join that is filtered with an ON expression.
    /// </summary>
    public abstract class FilteredJoin : BinaryJoin
    {
        private FilterGroup on;

        /// <summary>
        /// Initializes a new instance of a FilteredJoin.
        /// </summary>
        /// <param name="left">The left hand item in the join.</param>
        /// <param name="right">The right hand item in the join.</param>
        protected FilteredJoin(Join left, AliasedSource right)
            : base(left, right)
        {
            on = new FilterGroup();
        }

        /// <summary>
        /// Sets the condition on which the source is joined with the other tables.
        /// </summary>
        /// <param name="filterGenerator">A function that creates the join.</param>
        /// <returns>The current join.</returns>
        public Join On(Func<Join, IFilter> filterGenerator)
        {
            if (filterGenerator == null)
            {
                throw new ArgumentNullException("filterGenerator");
            }
            FilterGroup newGroup = new FilterGroup(Conjunction.And);
            IFilter filter = filterGenerator(this);
            newGroup.AddFilter(filter);
            on = newGroup;
            return this;
        }

        /// <summary>
        /// Gets the filters by which the left and right hand items are joined.
        /// </summary>
        public IEnumerable<IFilter> OnFilters
        {
            get { return on.Filters; }
        }

        /// <summary>
        /// Gets the filter group.
        /// </summary>
        internal FilterGroup OnFilterGroup
        {
            get { return on; }
        }

        /// <summary>
        /// Adds the filter to the group.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        public void AddOnFilter(IFilter filter)
        {
            on.AddFilter(filter);
        }

        /// <summary>
        /// Removes the filter from the group.
        /// </summary>
        /// <param name="filter">The filter to remove.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        public bool RemoveOnFilter(IFilter filter)
        {
            return on.RemoveFilter(filter);
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <param name="options">The configuration settings to use.</param>
        /// <returns>The generated text.</returns>
        protected override TokenStream GetOnTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.On, "ON"));
            stream.AddRange(((IFilter)on).GetFilterTokens(options));
            return stream;
        }
    }
}
