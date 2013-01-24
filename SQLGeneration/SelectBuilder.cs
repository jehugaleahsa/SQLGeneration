using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a select statement.
    /// </summary>
    public class SelectBuilder : ISelectBuilder
    {
        private string _alias;
        private readonly List<IJoinItem> _from;
        private bool _isDistinct;
        private ITop _top;
        private readonly List<IProjectionItem> _projection;
        private readonly IFilterGroup _where;
        private readonly List<IOrderBy> _orderBy;
        private readonly List<IGroupByItem> _groupBy;
        private readonly IFilterGroup _having;

        /// <summary>
        /// Initializes a new instance of a QueryBuilder.
        /// </summary>
        public SelectBuilder()
        {
            _from = new List<IJoinItem>();
            _projection = new List<IProjectionItem>();
            _where = new FilterGroup();
            _orderBy = new List<IOrderBy>();
            _groupBy = new List<IGroupByItem>();
            _having = new FilterGroup();
        }

        /// <summary>
        /// Gets or sets the alias of the command.
        /// </summary>
        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to return distinct results.
        /// </summary>
        public bool IsDistinct
        {
            get
            {
                return _isDistinct;
            }
            set
            {
                _isDistinct = value;
            }
        }

        /// <summary>
        /// Gets or sets the TOP clause.
        /// </summary>
        public ITop Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
            }
        }

        /// <summary>
        /// Creates a new column under the sub-select.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName)
        {
            return new Column(this, columnName);
        }

        IColumn IJoinItem.CreateColumn(string columnName)
        {
            return CreateColumn(columnName);
        }

        /// <summary>
        /// Creates a new column under the sub-select with the given alias.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName, string alias)
        {
            return new Column(this, columnName) { Alias = alias };
        }

        IColumn IJoinItem.CreateColumn(string columnName, string alias)
        {
            return CreateColumn(columnName, alias);
        }

        /// <summary>
        /// Gets the items that are part of the projection.
        /// </summary>
        public IEnumerable<IProjectionItem> Projection
        {
            get 
            {
                return new ReadOnlyCollection<IProjectionItem>(_projection);
            }
        }

        /// <summary>
        /// Adds a projection item to the projection.
        /// </summary>
        /// <param name="item">The projection item to add.</param>
        /// <returns>The item that was added.</returns>
        public void AddProjection(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _projection.Add(item);
        }

        /// <summary>
        /// Removes the projection item from the projection.
        /// </summary>
        /// <param name="item">The projection item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveProjection(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _projection.Remove(item);
        }

        /// <summary>
        /// Gets the tables, joins or sub-queries that are projected from.
        /// </summary>
        public IEnumerable<IJoinItem> From
        {
            get { return new ReadOnlyCollection<IJoinItem>(_from); }
        }

        /// <summary>
        /// Adds the given join item to the from clause.
        /// </summary>
        /// <param name="joinItem">The join item to add.</param>
        public void AddJoinItem(IJoinItem joinItem)
        {
            if (joinItem == null)
            {
                throw new ArgumentNullException("joinItem");
            }
            _from.Add(joinItem);
        }

        /// <summary>
        /// Removes the given join item from the from clause.
        /// </summary>
        /// <param name="joinItem">The join item to remove.</param>
        public bool RemoveJoinItem(IJoinItem joinItem)
        {
            if (joinItem == null)
            {
                throw new ArgumentNullException("joinItem");
            }
            return _from.Remove(joinItem);
        }

        /// <summary>
        /// Gets the items used to sort the results.
        /// </summary>
        public IEnumerable<IOrderBy> OrderBy
        {
            get { return new ReadOnlyCollection<IOrderBy>(_orderBy); }
        }

        /// <summary>
        /// Adds a sort criteria to the query.
        /// </summary>
        /// <param name="item">The sort criteria to add.</param>
        public void AddOrderBy(IOrderBy item)
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
        public bool RemoveOrderBy(IOrderBy item)
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
        public void AddWhere(IFilter filter)
        {
            _where.AddFilter(filter);
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
        public void AddHaving(IFilter filter)
        {
            _having.AddFilter(filter);
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
        public string GetCommandText()
        {
            return getCommandText(new BuilderContext());
        }

        /// <summary>
        /// Gets the SQL that represents the query.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        public string GetCommandText(BuilderContext context)
        {
            return getCommandText(context);
        }

        private string getCommandText(BuilderContext context)
        {
            if (_projection.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoProjections);
            }
            List<Tuple<string, bool>> parts = new List<Tuple<string, bool>>();
            parts.Add(getHeader(context));
            parts.Add(getFrom(context));
            parts.Add(getWhere(context));
            parts.Add(getOrderBy(context));
            parts.Add(getGroupBy(context));
            parts.Add(getHaving(context));
            StringBuilder result = new StringBuilder();
            result.Append(parts[0].Item1);
            StringBuilder clauseSeparatorBuilder = new StringBuilder();
            if (context.Options.OneClausePerLine)
            {
                clauseSeparatorBuilder.AppendLine();
                clauseSeparatorBuilder.Append(context.GetIndentationText());
            }
            else
            {
                clauseSeparatorBuilder.Append(' ');
            }
            string clauseSeparator = clauseSeparatorBuilder.ToString();
            bool reset = false;
            for (int partIndex = 1; partIndex < parts.Count; ++partIndex)
            {
                if (parts[partIndex].Item1.Length == 0)
                {
                    continue;
                }
                if (!reset)
                {
                    result.Append(clauseSeparator);
                }
                result.Append(parts[partIndex].Item1);
                reset = parts[partIndex].Item2;
            }
            return result.ToString();
        }

        private Tuple<string, bool> getHeader(BuilderContext context)
        {
            List<string> parts = new List<string>() { "SELECT", getDistinct(), getTop(context) };
            StringBuilder builder = new StringBuilder(String.Join(" ", parts.Where(part => part.Length != 0)));
            Tuple<string, bool> projections = getProjections(context.Indent());
            if (!projections.Item2)
            {
                builder.Append(' ');
            }
            builder.Append(projections.Item1);
            return Tuple.Create(builder.ToString(), projections.Item2);
        }

        private string getDistinct()
        {
            StringBuilder result = new StringBuilder();
            if (_isDistinct)
            {
                result.Append("DISTINCT");
            }
            return result.ToString();
        }

        private string getTop(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (_top != null)
            {
                result.Append(_top.GetTopText(context));
            }
            return result.ToString();
        }

        private Tuple<string, bool> getProjections(BuilderContext context)
        {
            ProjectionItemFormatter projectionFormatter = new ProjectionItemFormatter();
            IEnumerable<string> projections = _projection.Select(item => projectionFormatter.GetDeclaration(context, item));
            StringBuilder projectionSeparator = new StringBuilder(",");
            if (context.Options.OneProjectionPerLine)
            {
                string indentation = context.GetIndentationText();
                projections = projections.Select(item => indentation + item);
                projectionSeparator.AppendLine();
            }
            else
            {
                projectionSeparator.Append(' ');
            }
            StringBuilder result = new StringBuilder();
            if (context.Options.OneProjectionPerLine)
            {
                result.AppendLine();
            }
            result.Append(String.Join(projectionSeparator.ToString(), projections));
            return Tuple.Create(result.ToString(), context.Options.OneProjectionPerLine);
        }

        private Tuple<string, bool> getFrom(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (_from.Count != 0)
            {
                result.Append("FROM ");
                StringBuilder separatorBuilder = new StringBuilder(",");
                if (context.Options.OneJoinItemPerLine)
                {
                    separatorBuilder.AppendLine();
                }
                else
                {
                    separatorBuilder.Append(' ');
                }
                string joined = String.Join(separatorBuilder.ToString(), _from.Select(joinItem => joinItem.GetDeclaration(context.Indent(), _where)));
                result.Append(joined);
            }
            return Tuple.Create(result.ToString(), false);
        }

        private Tuple<string, bool> getWhere(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (_where.HasFilters)
            {
                result.Append("WHERE ");
                result.Append(_where.GetFilterText(context));
            }
            return Tuple.Create(result.ToString(), false);
        }

        private Tuple<string, bool> getOrderBy(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (_orderBy.Count > 0)
            {
                result.Append("ORDER BY ");
                IEnumerable<string> orderBys = _orderBy.Select(orderBy => orderBy.GetOrderByText(context));
                string joined = String.Join(", ", orderBys);
                result.Append(joined);
            }
            return Tuple.Create(result.ToString(), false);
        }

        private Tuple<string, bool> getGroupBy(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (_groupBy.Count > 0)
            {
                result.Append("GROUP BY ");
                IEnumerable<string> groupBys = _groupBy.Select(groupBy => groupBy.GetGroupByItemText(context));
                string joined = String.Join(", ", groupBys);
                result.Append(joined);
            }
            return Tuple.Create(result.ToString(), false);
        }

        private Tuple<string, bool> getHaving(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (_having.HasFilters)
            {
                result.Append("HAVING ");
                result.Append(_having.GetFilterText(context));
            }
            return Tuple.Create(result.ToString(), false);
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return '(' + getCommandText(context) + ')';
        }

        string IJoinItem.GetDeclaration(BuilderContext context, IFilterGroup where)
        {
            StringBuilder result = new StringBuilder("(");
            if (context.Options.OneClausePerLine)
            {
                result.AppendLine();
                result.Append(context.Indent().GetIndentationText());
            }
            result.Append(getCommandText(context.Indent()));
            if (context.Options.OneClausePerLine)
            {
                result.AppendLine();
                result.Append(context.GetIndentationText());
            }
            result.Append(')');
            if (!String.IsNullOrWhiteSpace(_alias))
            {
                result.Append(" ");
                result.Append(_alias);
            }
            return result.ToString();
        }

        string IJoinItem.GetReference(BuilderContext context)
        {
            if (String.IsNullOrWhiteSpace(_alias))
            {
                throw new SQLGenerationException(Resources.ReferencedQueryWithoutAlias);
            }
            return _alias;
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return '(' + getCommandText(context) + ')';
        }
    }
}
