using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Properties;
using SQLGeneration.Parsing;
using System.Globalization;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a select statement.
    /// </summary>
    public class SelectBuilder : ISelectBuilder, IFilteredCommand
    {
        private readonly List<IJoinItem> _from;
        private readonly List<AliasedProjection> _projection;
        private readonly HashSet<string> projectionNames;
        private readonly FilterGroup _where;
        private readonly List<OrderBy> _orderBy;
        private readonly List<IGroupByItem> _groupBy;
        private readonly FilterGroup _having;
        private readonly SourceCollection sources;

        /// <summary>
        /// Initializes a new instance of a SelectBuilder.
        /// </summary>
        public SelectBuilder()
        {
            _from = new List<IJoinItem>();
            _projection = new List<AliasedProjection>();
            projectionNames = new HashSet<string>();
            _where = new FilterGroup();
            _orderBy = new List<OrderBy>();
            _groupBy = new List<IGroupByItem>();
            _having = new FilterGroup();
            sources = new SourceCollection();
        }

        /// <summary>
        /// Gets or sets whether to return distinct results.
        /// </summary>
        public bool IsDistinct
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the TOP clause.
        /// </summary>
        public Top Top
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the items that are part of the projection.
        /// </summary>
        public IEnumerable<AliasedProjection> Projection
        {
            get 
            {
                return new ReadOnlyCollection<AliasedProjection>(_projection);
            }
        }

        /// <summary>
        /// Adds a projection item to the projection.
        /// </summary>
        /// <param name="item">The projection item to add.</param>
        /// <param name="alias">The alias to refer to the item with.</param>
        /// <returns>The item that was added.</returns>
        public AliasedProjection AddProjection(IProjectionItem item, string alias = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            AliasedProjection projection = new AliasedProjection(item, alias);
            string name = projection.GetProjectionName();
            if (name != null)
            {
                if (projectionNames.Contains(name))
                {
                    string message = String.Format(CultureInfo.CurrentCulture, Resources.DuplicateProjectionName, name);
                    throw new SQLGenerationException(message);
                }
                projectionNames.Add(name);
            }
            _projection.Add(projection);
            return projection;
        }

        /// <summary>
        /// Removes the projection item from the projection.
        /// </summary>
        /// <param name="projection">The projection item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveProjection(AliasedProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }
            return _projection.Remove(projection);
        }

        /// <summary>
        /// Gets the tables, joins or sub-queries that are projected from.
        /// </summary>
        public IEnumerable<IJoinItem> From
        {
            get { return new ReadOnlyCollection<IJoinItem>(_from); }
        }

        /// <summary>
        /// Gets the sources that have been added to the builder.
        /// </summary>
        public SourceCollection Sources
        {
            get { return sources; }
        }

        /// <summary>
        /// Adds the given table to the FROM clause.
        /// </summary>
        /// <param name="table">The table to add.</param>
        /// <param name="alias">The optional alias to give the table within the SELECT statement.</param>
        /// <returns>An object to support aliasing the table and defining columns.</returns>
        public AliasedSource AddTable(Table table, string alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            AliasedSource source = new AliasedSource(table, alias);
            sources.AddSource(source.GetSourceName(), source);
            _from.Add(source.Source);
            return source;
        }

        /// <summary>
        /// Adds the given SELECT statement to the FROM clause.
        /// </summary>
        /// <param name="builder">The SELECT statement to add.</param>
        /// <param name="alias">The optional alias to give the SELECT statement within the SELECT statement.</param>
        /// <returns>An object to support aliasing the SELECT statement and defining columns.</returns>
        public AliasedSource AddTable(ISelectBuilder builder, string alias = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            AliasedSource source = new AliasedSource(builder, alias);
            sources.AddSource(source.GetSourceName(), source);
            _from.Add(source.Source);
            return source;
        }

        /// <summary>
        /// Adds the given join to the FROM clause.
        /// </summary>
        /// <param name="join">The join to add.</param>
        public void AddJoin(Join join)
        {
            if (join == null)
            {
                throw new ArgumentNullException("join");
            }
            sources.AddSources(join.Sources);
            _from.Add(join);
        }

        /// <summary>
        /// Removes the given table or SELECT statement from the FROM clause.
        /// </summary>
        /// <param name="source">The table or SELECT statement to remove.</param>
        /// <returns>True if the table or SELECT statement was found and removed; otherwise, false.</returns>
        public bool RemoveSource(AliasedSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("joinItem");
            }
            string sourceName = source.GetSourceName();
            if (sourceName == null)
            {
                return _from.Remove(source.Source);
            }
            if (sources.Exists(sourceName) && _from.Remove(source.Source))
            {
                sources.Remove(sourceName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the given join from the FROM clause.
        /// </summary>
        /// <param name="join">The join to remove.</param>
        /// <returns>True if the item was found and removed; otherwise, false.</returns>
        public bool RemoveJoin(Join join)
        {
            if (join == null)
            {
                throw new ArgumentNullException("joinItem");
            }
            return _from.Remove(join);
        }

        /// <summary>
        /// Gets the items used to sort the results.
        /// </summary>
        public IEnumerable<OrderBy> OrderBy
        {
            get { return new ReadOnlyCollection<OrderBy>(_orderBy); }
        }

        /// <summary>
        /// Adds a sort criteria to the query.
        /// </summary>
        /// <param name="item">The sort criteria to add.</param>
        public void AddOrderBy(OrderBy item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _orderBy.Add(item);
        }

        /// <summary>
        /// Removes the sort criteria from the query.
        /// </summary>
        /// <param name="item">The order by item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveOrderBy(OrderBy item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _orderBy.Remove(item);
        }

        /// <summary>
        /// Gets the items that the query is grouped by.
        /// </summary>
        public IEnumerable<IGroupByItem> GroupBy
        {
            get { return new ReadOnlyCollection<IGroupByItem>(_groupBy); }
        }

        /// <summary>
        /// Adds the item to the group by clause of the query.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddGroupBy(IGroupByItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _groupBy.Add(item);
        }

        /// <summary>
        /// Removes the item from the group by clause of the query.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveGroupBy(IGroupByItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _groupBy.Remove(item);
        }

        /// <summary>
        /// Gets the filters in the filter group.
        /// </summary>
        public IEnumerable<IFilter> Where
        {
            get { return _where.Filters; }
        }

        /// <summary>
        /// Adds the filter to the where clause.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        /// <param name="conjunction">Specifies whether to AND or OR that </param>
        public void AddWhere(IFilter filter, Conjunction conjunction)
        {
            _where.AddFilter(filter, conjunction);
        }

        /// <summary>
        /// Removes the filter from the where clause.
        /// </summary>
        /// <param name="filter">The filter to remove.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        public bool RemoveWhere(IFilter filter)
        {
            return _where.RemoveFilter(filter);
        }

        /// <summary>
        /// Gets the filters in the having clause.
        /// </summary>
        public IEnumerable<IFilter> Having
        {
            get { return _having.Filters; }
        }

        /// <summary>
        /// Adds the filter to the having clause.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        /// <param name="conjunction">Specifies whether to use AND or OR when testing the filter.</param>
        public void AddHaving(IFilter filter, Conjunction conjunction)
        {
            _having.AddFilter(filter, conjunction);
        }

        /// <summary>
        /// Removes the filter from the having clause.
        /// </summary>
        /// <param name="filter">The filter to remove.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        public bool RemoveHaving(IFilter filter)
        {
            return _having.RemoveFilter(filter);
        }

        /// <summary>
        /// Gets the SQL that represents the query.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        public IEnumerable<string> GetCommandTokens(CommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            options = options.Clone();
            options.IsSelect = true;
            options.IsInsert = false;
            options.IsUpdate = false;
            options.IsDelete = false;
            return getCommandTokens(options);
        }

        private IEnumerable<string> getCommandTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("SELECT");
            if (IsDistinct)
            {
                stream.Add("DISTINCT");
            }
            if (Top != null)
            {
                stream.AddRange(Top.GetTopTokens(options));
            }
            stream.AddRange(buildProjection(options));
            stream.AddRange(buildFrom(options));
            if (_where.HasFilters)
            {
                stream.Add("WHERE");
                stream.AddRange(((IFilter)_where).GetFilterTokens(options));
            }
            stream.AddRange(buildGroupBy(options));
            if (_having.HasFilters)
            {
                stream.Add("HAVING");
                stream.AddRange(((IFilter)_having).GetFilterTokens(options));
            }
            stream.AddRange(buildOrderBy(options));
            return stream;
        }

        private IEnumerable<string> buildProjection(CommandOptions options)
        {
            using (IEnumerator<AliasedProjection> enumerator = _projection.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new SQLGenerationException(Resources.NoProjections);
                }
                TokenStream stream = new TokenStream();
                stream.AddRange(enumerator.Current.GetDeclarationTokens(options));
                while (enumerator.MoveNext())
                {
                    stream.Add(",");
                    stream.AddRange(enumerator.Current.GetDeclarationTokens(options));
                }
                return stream;
            }
        }

        private IEnumerable<string> buildFrom(CommandOptions options)
        {
            using (IEnumerator<IJoinItem> enumerator = _from.GetEnumerator())
            {
                TokenStream stream = new TokenStream();
                if (enumerator.MoveNext())
                {
                    stream.Add("FROM");
                    stream.AddRange(enumerator.Current.GetDeclarationTokens(options));
                    while (enumerator.MoveNext())
                    {
                        stream.Add(",");
                        stream.AddRange(enumerator.Current.GetDeclarationTokens(options));
                    }
                }
                return stream;
            }
        }

        private IEnumerable<string> buildGroupBy(CommandOptions options)
        {
            using (IEnumerator<IGroupByItem> enumerator = _groupBy.GetEnumerator())
            {
                TokenStream stream = new TokenStream();
                if (enumerator.MoveNext())
                {
                    stream.Add("GROUP BY");
                    stream.AddRange(enumerator.Current.GetGroupByTokens(options));
                    while (enumerator.MoveNext())
                    {
                        stream.Add(",");
                        stream.AddRange(enumerator.Current.GetGroupByTokens(options));
                    }
                }
                return stream;
            }
        }

        private IEnumerable<string> buildOrderBy(CommandOptions options)
        {
            using (IEnumerator<OrderBy> enumerator = _orderBy.GetEnumerator())
            {
                TokenStream stream = new TokenStream();
                if (enumerator.MoveNext())
                {
                    stream.Add("ORDER BY");
                    stream.AddRange(enumerator.Current.GetOrderByTokens(options));
                    while (enumerator.MoveNext())
                    {
                        stream.Add(",");
                        stream.AddRange(enumerator.Current.GetOrderByTokens(options));
                    }
                }
                return stream;
            }
        }

        IEnumerable<string> IJoinItem.GetDeclarationTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(getSelectContentTokens(options));
            return stream;
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getSelectContentTokens(options);
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getSelectContentTokens(options);
        }

        IEnumerable<string> getSelectContentTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("(");
            stream.AddRange(getCommandTokens(options));
            stream.Add(")");
            return stream;
        }

        string IRightJoinItem.GetSourceName()
        {
            return null;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        bool IRightJoinItem.IsQuery
        {
            get { return true; }
        }

        bool IValueProvider.IsQuery
        {
            get { return true; }
        }
    }
}
