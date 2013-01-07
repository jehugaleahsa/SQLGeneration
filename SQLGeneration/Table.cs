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
        private string _alias; 

        /// <summary>
        /// Creates a new Table.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        public Table(string name)
            : this(null, name)
        {
        }

        /// <summary>
        /// Creates a new Table.
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
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        IColumn IJoinItem.CreateColumn(string columnName)
        {
            return CreateColumn(columnName);
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

        string IJoinItem.GetDeclaration(IFilterGroup where)
        {
            StringBuilder result = new StringBuilder(getFullName());
            if (!String.IsNullOrWhiteSpace(_alias))
            {
                result.Append(" ");
                result.Append(_alias);
            }
            return result.ToString();
        }

        string IJoinItem.GetReference()
        {
            StringBuilder result = new StringBuilder();
            if (String.IsNullOrWhiteSpace(_alias))
            {
                result.Append(getFullName());
            }
            else
            {
                result.Append(_alias);
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
