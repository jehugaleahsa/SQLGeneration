using System;
using System.Text;

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
        /// Gets a filter to apply to the query.
        /// </summary>
        public IFilterGroup Where
        {
            get { return _where; }
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        public string GetCommandText()
        {
            StringBuilder result = new StringBuilder("DELETE FROM ");
            result.Append(_table.GetDeclaration(_where));
            if (_where.HasFilters)
            {
                result.Append(" WHERE ");
                result.Append(_where.GetFilterText());
            }
            return result.ToString();
        }
    }
}
