using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Expressions;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a select statement.
    /// </summary>
    public class SelectBuilder : ISelectBuilder, IFilteredCommand
    {
        private readonly List<IJoinItem> _from;
        private readonly List<IProjectionItem> _projection;
        private readonly FilterGroup _where;
        private readonly List<OrderBy> _orderBy;
        private readonly List<IGroupByItem> _groupBy;
        private readonly FilterGroup _having;

        /// <summary>
        /// Initializes a new instance of a QueryBuilder.
        /// </summary>
        public SelectBuilder()
        {
            _from = new List<IJoinItem>();
            _projection = new List<IProjectionItem>();
            _where = new FilterGroup();
            _orderBy = new List<OrderBy>();
            _groupBy = new List<IGroupByItem>();
            _having = new FilterGroup();
        }

        /// <summary>
        /// Gets or sets the alias of the command.
        /// </summary>
        public string Alias
        {
            get;
            set;
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
        /// Creates a new column under the sub-select.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName)
        {
            return new Column(this, columnName);
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
        /// <param name="options">The configuration to use when building the command.</param>
        public IExpressionItem GetCommandExpression(CommandOptions options)
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
            Expression expression = new Expression(ExpressionItemType.SelectCommand);
            getCommandExpression(expression, options);
            return expression;
        }

        private void getCommandExpression(Expression expression, CommandOptions options)
        {
            // "SELECT" [ "DISTINCT" ] [<Top>] <ProjectionList>
            // [ "FROM" <Join> ]
            // [ "WHERE" <Filter> ]
            // [ "GROUP BY" <GroupByList> ]
            // [ "HAVING" <Filter> ]
            // [ "ORDER BY" <OrderByList> ]
            if (_projection.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoProjections);
            }
            expression.AddItem(new Token("SELECT"));
            if (IsDistinct)
            {
                expression.AddItem(new Token("DISTINCT"));
            }
            if (Top != null)
            {
                expression.AddItem(Top.GetTopExpression(options));
            }
            expression.AddItem(buildProjection(options, 0));
            if (_from.Count != 0)
            {
                expression.AddItem(new Token("FROM"));
                expression.AddItem(buildFrom(options, 0));
            }
            if (_where.HasFilters)
            {
                expression.AddItem(new Token("WHERE"));
                expression.AddItem(_where.GetFilterExpression(options));
            }
            if (_groupBy.Count > 0)
            {
                expression.AddItem(new Token("GROUP BY"));
                expression.AddItem(buildGroupBy(options, 0));
            }
            if (_having.HasFilters)
            {
                expression.AddItem(new Token("HAVING"));
                expression.AddItem(_having.GetFilterExpression(options));
            }
            if (_orderBy.Count > 0)
            {
                expression.AddItem(new Token("ORDER BY"));
                expression.AddItem(buildOrderBy(options, 0));
            }
        }

        private IExpressionItem buildProjection(CommandOptions options, int projectionIndex)
        {
            if (projectionIndex == _projection.Count - 1)
            {
                IProjectionItem current = _projection[projectionIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                return formatter.GetDeclaration(current);
            }
            else
            {
                IExpressionItem right = buildProjection(options, projectionIndex + 1);
                IProjectionItem current = _projection[projectionIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                IExpressionItem left = formatter.GetDeclaration(current);
                Expression expression = new Expression(ExpressionItemType.ProjectionItemList);
                expression.AddItem(left);
                expression.AddItem(new Token(","));
                expression.AddItem(right);
                return expression;
            }
        }

        private IExpressionItem buildFrom(CommandOptions options, int fromIndex)
        {
            if (fromIndex == _from.Count - 1)
            {
                IJoinItem current = _from[fromIndex];
                return current.GetDeclarationExpression(options);
            }
            else
            {
                IExpressionItem right = buildFrom(options, fromIndex + 1);
                IJoinItem current = _from[fromIndex];
                IExpressionItem left = current.GetDeclarationExpression(options);
                Expression expression = new Expression(ExpressionItemType.FromList);
                expression.AddItem(left);
                expression.AddItem(new Token(","));
                expression.AddItem(right);
                return expression;
            }
        }

        private IExpressionItem buildGroupBy(CommandOptions options, int groupByIndex)
        {
            if (groupByIndex == _groupBy.Count - 1)
            {
                IGroupByItem current = _groupBy[groupByIndex];
                Expression expression = new Expression(ExpressionItemType.GroupByList);
                current.GetGroupByExpression(expression, options);
                return expression;
            }
            else
            {
                IExpressionItem right = buildGroupBy(options, groupByIndex + 1);
                Expression expression = new Expression(ExpressionItemType.GroupByList);
                IGroupByItem current = _groupBy[groupByIndex];
                current.GetGroupByExpression(expression, options);
                expression.AddItem(new Token(","));
                expression.AddItem(right);
                return expression;
            }
        }

        private IExpressionItem buildOrderBy(CommandOptions options, int orderByIndex)
        {
            if (orderByIndex == _orderBy.Count - 1)
            {
                OrderBy current = _orderBy[orderByIndex];
                return current.GetOrderByExpression(options);
            }
            else
            {
                IExpressionItem right = buildOrderBy(options, orderByIndex + 1);
                OrderBy current = _orderBy[orderByIndex];
                IExpressionItem left = current.GetOrderByExpression(options);
                Expression expression = new Expression(ExpressionItemType.OrderByList);
                expression.AddItem(left);
                expression.AddItem(new Token(","));
                expression.AddItem(right);
                return expression;
            }
        }

        IExpressionItem IJoinItem.GetDeclarationExpression(CommandOptions options)
        {
            Expression expression = new Expression(ExpressionItemType.SelectCommand);
            getSelectContent(expression, options);
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                if (options.AliasColumnSourcesUsingAs)
                {
                    expression.AddItem(new Token("AS"));
                }
                expression.AddItem(new Token(Alias));
            }
            return expression;
        }

        IExpressionItem IColumnSource.GetReferenceExpression(CommandOptions options)
        {
            if (String.IsNullOrWhiteSpace(Alias))
            {
                throw new SQLGenerationException(Resources.ReferencedQueryWithoutAlias);
            }
            return new Token(Alias);
        }

        void IProjectionItem.GetProjectionExpression(Expression expression, CommandOptions options)
        {
            getSelectContent(expression, options);
        }

        void IFilterItem.GetFilterExpression(Expression expression, CommandOptions options)
        {
            getSelectContent(expression, options);
        }

        private void getSelectContent(Expression expression, CommandOptions options)
        {
            expression.AddItem(new Token("("));
            getCommandExpression(expression, options);
            expression.AddItem(new Token(")"));
        }

        bool IValueProvider.IsQuery
        {
            get { return true; }
        }
    }
}
