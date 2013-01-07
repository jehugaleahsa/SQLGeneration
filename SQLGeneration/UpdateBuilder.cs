using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of an update statement.
    /// </summary>
    public class UpdateBuilder : IUpdateBuilder
    {
        private readonly ITable _table;
        private readonly IList<ISetter> _setters;
        private readonly IFilterGroup _where;

        /// <summary>
        /// Initializes a new instance of a UpdateBuilder.
        /// </summary>
        /// <param name="table">The table being updated.</param>
        public UpdateBuilder(ITable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            _table = table;
            _setters = new List<ISetter>();
            _where = new FilterGroup();
        }

        /// <summary>
        /// Gets the table that is being updated.
        /// </summary>
        public ITable Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the columns that are being set.
        /// </summary>
        public IEnumerable<ISetter> Setters
        {
            get { return new ReadOnlyCollection<ISetter>(_setters); }
        }

        /// <summary>
        /// Adds the setter to the update statement.
        /// </summary>
        /// <param name="setter">The setter to add.</param>
        public void AddSetter(ISetter setter)
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
        public bool RemoveSetter(ISetter setter)
        {
            if (setter == null)
            {
                throw new ArgumentNullException("setter");
            }
            return _setters.Remove(setter);
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
            if (_setters.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoSetters);
            }
            StringBuilder result = new StringBuilder("UPDATE ");
            result.Append(_table.GetDeclaration(null));
            result.Append(" SET ");
            string setters = String.Join(", ", from setter in _setters select setter.SetterText);
            result.Append(setters);
            if (_where.HasFilters)
            {
                result.Append(" WHERE ");
                result.Append(_where.GetFilterText());
            }
            return result.ToString();
        }
    }
}
