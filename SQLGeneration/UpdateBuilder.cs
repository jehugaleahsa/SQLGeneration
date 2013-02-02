using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of an update statement.
    /// </summary>
    public class UpdateBuilder : IFilteredCommand
    {
        private readonly Table _table;
        private readonly IList<Setter> _setters;
        private readonly FilterGroup _where;

        /// <summary>
        /// Initializes a new instance of a UpdateBuilder.
        /// </summary>
        /// <param name="table">The table being updated.</param>
        public UpdateBuilder(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            _table = table;
            _setters = new List<Setter>();
            _where = new FilterGroup();
        }

        /// <summary>
        /// Gets the table that is being updated.
        /// </summary>
        public Table Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the columns that are being set.
        /// </summary>
        public IEnumerable<Setter> Setters
        {
            get { return new ReadOnlyCollection<Setter>(_setters); }
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
        public IEnumerable<string> GetCommandExpression(CommandOptions options)
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
            return getCommandExpression(options);
        }

        private IEnumerable<string> getCommandExpression(CommandOptions options)
        {
            // <UpdateCommand> => "UPDATE" <Table> "SET" <SetterList> [ "WHERE" <Filter> ]
            if (_setters.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoSetters);
            }
            yield return "UPDATE";
            foreach (string token in _table.GetDeclarationExpression(options))
            {
                yield return token;
            }
            yield return "SET";
            foreach (string token in buildSetterList(options, 0))
            {
                yield return token;
            }
            if (_where.HasFilters)
            {
                yield return "WHERE";
                foreach (string token in _where.GetFilterExpression(options))
                {
                    yield return token;
                }
            }
        }

        private IEnumerable<string> buildSetterList(CommandOptions options, int setterIndex)
        {
            if (setterIndex == _setters.Count - 1)
            {
                Setter current = _setters[setterIndex];
                foreach (string token in current.GetSetterExpression(options))
                {
                    yield return token;
                }
            }
            else
            {
                Setter current = _setters[setterIndex];
                foreach (string token in current.GetSetterExpression(options))
                {
                    yield return token;
                }
                yield return ",";
                foreach (string token in buildSetterList(options, setterIndex + 1))
                {
                    yield return token;
                }
            }
        }
    }
}
