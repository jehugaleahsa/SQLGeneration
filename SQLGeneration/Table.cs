using System;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a table name.
    /// </summary>
    public class Table : ITable
    {
        private readonly ISchema _schema;
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of a Table.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        public Table(string name)
            : this(null, name)
        {
        }

        /// <summary>
        /// Initializes a new instance of a Table.
        /// </summary>
        /// <param name="schema">The schema the table belongs to.</param>
        /// <param name="name">The name of the table.</param>
        public Table(ISchema schema, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankTableName, "name");
            }
            _schema = schema;
            _name = name;
        }

        /// <summary>
        /// Gets or sets the schema the table belongs to.
        /// </summary>
        public ISchema Schema
        {
            get
            {
                return _schema;
            }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets or sets an alias to use for the table.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new column under the table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName)
        {
            return new Column(this, columnName);
        }

        IColumn IColumnSource.CreateColumn(string columnName)
        {
            return CreateColumn(columnName);
        }

        /// <summary>
        /// Creates a new column under the table with the given alias.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName, string alias)
        {
            return new Column(this, columnName) { Alias = alias };
        }

        IColumn IColumnSource.CreateColumn(string columnName, string alias)
        {
            return CreateColumn(columnName, alias);
        }

        string IJoinItem.GetDeclaration(BuilderContext context, IFilterGroup where)
        {
            StringBuilder result = new StringBuilder(getFullName());
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                result.Append(' ');
                if (context.Options.AliasJoinItemsUsingAs)
                {
                    result.Append("AS ");
                }
                result.Append(Alias);
            }
            return result.ToString();
        }

        string IColumnSource.GetReference(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (String.IsNullOrWhiteSpace(Alias))
            {
                result.Append(getFullName());
            }
            else
            {
                result.Append(Alias);
            }
            return result.ToString();
        }

        private string getFullName()
        {
            StringBuilder result = new StringBuilder();
            if (_schema != null)
            {
                result.Append(_schema.Name);
                result.Append(".");
            }
            result.Append(_name);
            return result.ToString();
        }
    }
}
