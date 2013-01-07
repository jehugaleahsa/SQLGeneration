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
        /// Creates a new column under the table.
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
        /// Gets a filter to apply to the query.
        /// </summary>
        public IFilterGroup Where
        {
            get
            {
                return _where;
            }
        }

        /// <summary>
        /// Gets a filter to apply to the aggregate functions.
        /// </summary>
        public IFilterGroup Having
        {
            get
            {
                return _having;
            }
        }

        /// <summary>
        /// Gets the SQL that represents the query.
        /// </summary>
        public string GetCommandText()
        {
            if (_projection.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoProjections);
            }
            StringBuilder result = new StringBuilder("SELECT ");
            if (_isDistinct)
            {
                result.Append("DISTINCT ");
            }
            if (_top != null)
            {
                result.Append(" ");
                result.Append(_top.TopText);
                result.Append(" ");
            }
            ProjectionItemFormatter projectionFormatter = new ProjectionItemFormatter();
            IEnumerable<string> projections = from projection in _projection select projectionFormatter.GetDeclaration(projection);
            result.Append(String.Join(", ", projections));
            if (_from.Count != 0)
            {
                result.Append(" FROM ");
                string from = String.Join(", ", (from joinItem in _from select joinItem.GetDeclaration(_where)));
                result.Append(from);
            }
            if (_where.HasFilters)
            {
                result.Append(" WHERE ");
                result.Append(_where.GetFilterText());
            }
            if (_orderBy.Count > 0)
            {
                result.Append(" ORDER BY ");
                IEnumerable<string> orderBys = from orderBy in _orderBy select orderBy.GetOrderByText();
                result.Append(String.Join(", ", orderBys));
            }
            if (_groupBy.Count > 0)
            {
                result.Append(" GROUP BY ");
                IEnumerable<string> groupBys = from groupBy in _groupBy select groupBy.GetGroupByItemText();
                result.Append(String.Join(", ", groupBys));
            }
            if (_having.HasFilters)
            {
                result.Append(" HAVING ");
                result.Append(_having.GetFilterText());
            }
            return result.ToString();
        }

        string IProjectionItem.GetFullText()
        {
            return '(' + GetCommandText() + ')';
        }

        string IJoinItem.GetDeclaration(IFilterGroup where)
        {
            StringBuilder result = new StringBuilder("(");
            result.Append(GetCommandText());
            result.Append(')');
            if (!String.IsNullOrWhiteSpace(_alias))
            {
                result.Append(" ");
                result.Append(_alias);
            }
            return result.ToString();
        }

        string IJoinItem.GetReference()
        {
            if (String.IsNullOrWhiteSpace(_alias))
            {
                throw new SQLGenerationException(Resources.ReferencedQueryWithoutAlias);
            }
            return _alias;
        }

        string IFilterItem.GetFilterItemText()
        {
            return '(' + GetCommandText() + ')';
        }
    }
}
