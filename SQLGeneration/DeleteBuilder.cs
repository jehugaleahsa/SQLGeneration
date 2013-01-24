using System;
using System.Text;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a delete statement.
    /// </summary>
    public class DeleteBuilder : IDeleteBuilder
    {
        private readonly ITable _table;
        private readonly IFilterGroup _where;

        /// <summary>
        /// Initializes a new instance of a DeleteBuilder.
        /// </summary>
        /// <param name="table">The table being deleted from.</param>
        public DeleteBuilder(ITable table)
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
        public ITable Table
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
        /// Gets the command text.
        /// </summary>
        public string GetCommandText()
        {
            return GetCommandText(new BuilderContext());
        }

        /// <summary>
        /// Gets the command, formatting it using the given options.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The command text.</returns>
        public string GetCommandText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder("DELETE FROM ");
            result.Append(_table.GetDeclaration(context, _where));
            if (_where.HasFilters)
            {
                result.Append(" WHERE ");
                result.Append(_where.GetFilterText(context));
            }
            return result.ToString();
        }
    }
}
