using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of a delete statement.
    /// </summary>
    public class DeleteBuilder : IFilteredCommand
    {
        private readonly AliasedSource _table;
        private readonly FilterGroup _where;

        /// <summary>
        /// Initializes a new instance of a DeleteBuilder.
        /// </summary>
        /// <param name="table">The table being deleted from.</param>
        /// <param name="alias">The alias to use to refer to the table.</param>
        public DeleteBuilder(Table table, string alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            _table = new AliasedSource(table, alias);
            _where = new FilterGroup();
        }

        /// <summary>
        /// Gets the table being deleted from.
        /// </summary>
        public AliasedSource Table
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
        /// <param name="conjunction">Specifies whether to AND or OR the filter with the other filters in the group.</param>
        public void AddWhere(IFilter filter, Conjunction conjunction = Conjunction.And)
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
        /// Gets the command, formatting it using the given options.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The command text.</returns>
        IEnumerable<string> ICommand.GetCommandTokens(CommandOptions options)
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
            return getCommandTokens(options);
        }

        private IEnumerable<string> getCommandTokens(CommandOptions options)
        {
            // <DeleteCommand> => "DELETE" [ "FROM" ] <Source> [ "WHERE" <Filter> ]
            TokenStream stream = new TokenStream();
            stream.Add("DELETE");
            stream.Add("FROM");
            stream.AddRange(((IJoinItem)_table).GetDeclarationTokens(options));
            if (_where.HasFilters)
            {
                stream.Add("WHERE");
                stream.AddRange(((IFilter)_where).GetFilterTokens(options));
            }
            return stream;
        }
    }
}
