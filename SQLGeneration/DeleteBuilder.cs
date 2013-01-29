using System;
using System.Collections.Generic;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a delete statement.
    /// </summary>
    public class DeleteBuilder : IFilteredCommand
    {
        private readonly Table _table;
        private readonly FilterGroup _where;

        /// <summary>
        /// Initializes a new instance of a DeleteBuilder.
        /// </summary>
        /// <param name="table">The table being deleted from.</param>
        public DeleteBuilder(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            _table = table;
            _where = new FilterGroup();
        }

        /// <summary>
        /// Gets the table being deleted from.
        /// </summary>
        public Table Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the filters in the where clause.
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
        /// Gets the command, formatting it using the given options.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The command text.</returns>
        public IExpressionItem GetCommandExpression(CommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            options = options.Clone();
            options.IsSelect = false;
            options.IsInsert = false;
            options.IsUpdate = false;
            options.IsDelete = true;
            IExpressionItem expression = getCommandExpression(options);
            return expression;
        }

        private IExpressionItem getCommandExpression(CommandOptions options)
        {
            // <DeleteCommand> => "DELETE" [ "FROM" ] <Source> [ "WHERE" <Filter> ]
            Expression expression = new Expression(ExpressionItemType.DeleteCommand);
            expression.AddItem(new Token("DELETE"));
            expression.AddItem(new Token("FROM"));
            expression.AddItem(_table.GetDeclarationExpression(options));
            if (_where.HasFilters)
            {
                expression.AddItem(new Token("WHERE"));
                expression.AddItem(_where.GetFilterExpression(options));
            }
            return expression;
        }
    }
}
