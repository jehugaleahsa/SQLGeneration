using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of an insert statement.
    /// </summary>
    public class InsertBuilder : ICommand
    {
        private readonly Table _table;
        private readonly List<Column> _columns;
        private readonly IValueProvider _values;

        /// <summary>
        /// Initializes a new instance of a InsertBuilder.
        /// </summary>
        /// <param name="table">The table being inserted into.</param>
        /// <param name="values">The values to insert into the table.</param>
        public InsertBuilder(Table table, IValueProvider values)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            _table = table;
            _columns = new List<Column>();
            _values = values;
        }

        /// <summary>
        /// Gets the table that is being inserted into.
        /// </summary>
        public Table Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the columns being inserted into.
        /// </summary>
        public IEnumerable<Column> Columns
        {
            get { return new ReadOnlyCollection<Column>(_columns); }
        }

        /// <summary>
        /// Adds the column to the insert statement.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void AddColumn(Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            _columns.Add(column);
        }

        /// <summary>
        /// Removes the column from the insert statement.
        /// </summary>
        /// <param name="column">The column to remove.</param>
        /// <returns>True if the column was removed; otherwise, false.</returns>
        public bool RemoveColumn(Column column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            return _columns.Remove(column);
        }

        /// <summary>
        /// Gets the list of values or select statement that populates the insert.
        /// </summary>
        public IValueProvider Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Gets the SQL for the insert statement.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        public IExpressionItem GetCommandExpression(CommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            options = options.Clone();
            options.IsSelect = false;
            options.IsInsert = true;
            options.IsUpdate = false;
            options.IsDelete = false;
            IExpressionItem expression = getCommandExpression(options);
            return expression;
        }

        private IExpressionItem getCommandExpression(CommandOptions options)
        {
            // "INSERT" [ "INTO" ] <Source> [ "(" <ColumnList> ")" ] { "VALUES" "(" <ValueList> ")" | <SubSelect> }
            Expression expression = new Expression();
            expression.AddItem(new Token("INSERT"));
            expression.AddItem(new Token("INTO"));
            expression.AddItem(_table.GetDeclarationExpression(options, null));
            if (_columns.Count > 0)
            {
                expression.AddItem(new Token("("));
                expression.AddItem(buildColumnList(options, 0));
                expression.AddItem(new Token(")"));
            }
            if (!_values.IsQuery)
            {
                expression.AddItem(new Token("VALUES"));
            }
            expression.AddItem(_values.GetFilterExpression(options));
            return expression;
        }

        private IExpressionItem buildColumnList(CommandOptions options, int columnIndex)
        {
            if (columnIndex == _columns.Count - 1)
            {
                Column column = _columns[columnIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                return formatter.GetUnaliasedReference(column);
            }
            else
            {
                IExpressionItem right = buildColumnList(options, columnIndex + 1);
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                Column column = _columns[columnIndex];
                IExpressionItem left = formatter.GetUnaliasedReference(column);
                Expression expression = new Expression();
                expression.AddItem(left);
                expression.AddItem(new Token(","));
                expression.AddItem(right);
                return expression;
            }
        }
    }
}
