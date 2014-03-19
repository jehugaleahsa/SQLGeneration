using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Builds a string of an update statement.
    /// </summary>
    public class UpdateBuilder : IFilteredCommand
    {
        private readonly AliasedSource _table;
        private readonly IList<Setter> _setters;
        private readonly FilterGroup _where;

        /// <summary>
        /// Initializes a new instance of a UpdateBuilder.
        /// </summary>
        /// <param name="table">The table being updated.</param>
        /// <param name="alias">The alias to use to refer to the table.</param>
        public UpdateBuilder(Table table, string alias = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            _table = new AliasedSource(table, alias);
            _setters = new List<Setter>();
            _where = new FilterGroup();
        }

        /// <summary>
        /// Gets the table that is being updated.
        /// </summary>
        public AliasedSource Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the columns that are being set.
        /// </summary>
        public IEnumerable<Setter> Setters
        {
            get { return _setters; }
        }

        /// <summary>
        /// Adds the setter to the update statement.
        /// </summary>
        /// <param name="setter">The setter to add.</param>
        public void AddSetter(Setter setter)
        {
            if (setter == null)
            {
                throw new ArgumentNullException("setter");
            }
            _setters.Add(setter);
        }

        /// <summary>
        /// Removes the setter from the update statement.
        /// </summary>
        /// <param name="setter">The setter to remove.</param>
        /// <returns>True if the setter is removed; otherwise, false.</returns>
        public bool RemoveSetter(Setter setter)
        {
            if (setter == null)
            {
                throw new ArgumentNullException("setter");
            }
            return _setters.Remove(setter);
        }

        /// <summary>
        /// Gets the filters in the where clause.
        /// </summary>
        public IEnumerable<IFilter> Where
        {
            get { return _where.Filters; }
        }

        /// <summary>
        /// Gets the filter group used to build the where clause.
        /// </summary>
        internal FilterGroup WhereFilterGroup 
        {
            get { return _where; }
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
        /// <param name="options">The configuration to use when building the command.</param>
        TokenStream ICommand.GetCommandTokens(CommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            options = options.Clone();
            options.IsSelect = false;
            options.IsInsert = false;
            options.IsUpdate = true;
            options.IsDelete = false;
            return getCommandTokens(options);
        }

        private TokenStream getCommandTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Update, "UPDATE"));
            stream.AddRange(((IJoinItem)_table).GetDeclarationTokens(options));
            stream.Add(new TokenResult(SqlTokenRegistry.Set, "SET"));
            stream.AddRange(buildSetterList(options));
            if (_where.HasFilters)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.Where, "WHERE"));
                stream.AddRange(((IFilter)_where).GetFilterTokens(options));
            }
            return stream;
        }

        private TokenStream buildSetterList(CommandOptions options)
        {
            using (IEnumerator<Setter> enumerator = _setters.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new SQLGenerationException(Resources.NoSetters);
                }
                TokenStream stream = new TokenStream();
                stream.AddRange(enumerator.Current.GetSetterTokens(options));
                while (enumerator.MoveNext())
                {
                    stream.Add(new TokenResult(SqlTokenRegistry.Comma, ","));
                    stream.AddRange(enumerator.Current.GetSetterTokens(options));
                }
                return stream;
            }
        }
    }
}
