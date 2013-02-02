using System;
using SQLGeneration.Properties;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a table name.
    /// </summary>
    public class Table : IRightJoinItem, IColumnSource
    {
        private readonly Schema _schema;
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
        public Table(Schema schema, string name)
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
        public Schema Schema
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

        /// <summary>
        /// Gets the table declaration expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expression declaring the table.</returns>
        public IEnumerable<string> GetDeclarationExpression(CommandOptions options)
        {
            // <TableDeclaration> => [ <Schema> "." ] <ID> [ "AS" ] <ID>
            foreach (string token in getFullNameExpression())
            {
                yield return token;
            }
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                if (options.AliasColumnSourcesUsingAs)
                {
                    yield return "AS";
                }
                yield return Alias;
            }
        }

        IEnumerable<string> IColumnSource.GetReferenceExpression(CommandOptions options)
        {
            if (String.IsNullOrWhiteSpace(Alias))
            {
                foreach (string token in getFullNameExpression())
                {
                    yield return token;
                }
            }
            else
            {
                yield return Alias;
            }
        }

        private IEnumerable<string> getFullNameExpression()
        {
            if (_schema != null)
            {
                yield return _schema.Name;
                yield return ".";
            }
            yield return _name;
        }
    }
}
