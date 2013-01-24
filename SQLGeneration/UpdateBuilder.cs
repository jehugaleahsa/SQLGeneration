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
        /// Gets the command text.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        public string GetCommandText(BuilderContext context)
        {
            context = context.Clone();
            context.IsSelect = false;
            context.IsInsert = false;
            context.IsUpdate = true;
            context.IsDelete = false;

            return getCommandText(context);
        }

        private string getCommandText(BuilderContext context)
        {
            if (_setters.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoSetters);
            }
            StringBuilder result = new StringBuilder("UPDATE ");
            result.Append(_table.GetDeclaration(context, null));
            if (context.Options.OneClausePerLine)
            {
                result.AppendLine();
            }
            else
            {
                result.Append(' ');
            }
            result.Append("SET");
            StringBuilder separatorBuilder = new StringBuilder(",");
            if (context.Options.OneSetterPerLine)
            {
                result.AppendLine();
                separatorBuilder.AppendLine();
            }
            else
            {
                result.Append(' ');
                separatorBuilder.Append(' ');
            }
            IEnumerable<string> setters = _setters.Select(setter => setter.GetSetterText(context));
            if (context.Options.OneSetterPerLine && context.Options.IndentSetters)
            {
                string indentationText = context.Indent().GetIndentationText();
                setters = setters.Select(setter => indentationText + setter);
            }
            string separated = String.Join(separatorBuilder.ToString(), setters);
            result.Append(separated);
            if (_where.HasFilters)
            {
                if (context.Options.OneClausePerLine || context.Options.OneSetterPerLine)
                {
                    result.AppendLine();
                }
                else
                {
                    result.Append(' ');
                }
                result.Append("WHERE ");
                result.Append(_where.GetFilterText(context));
            }
            return result.ToString();
        }
    }
}
