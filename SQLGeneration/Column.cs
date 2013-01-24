using System;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a database column.
    /// </summary>
    public class Column : IColumn
    {
        private readonly IColumnSource source;
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of a Column.
        /// </summary>
        /// <param name="source">The column source that the column belongs to.</param>
        /// <param name="name">The name of the column.</param>
        internal Column(IColumnSource source, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankColumnName, "name");
            }
            this.source = source;
            this.name = name;
        }

        /// <summary>
        /// Gets the table that the column belongs to.
        /// </summary>
        public IColumnSource Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the alias of the column.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return getColumnText(context);
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return getColumnText(context);
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return getColumnText(context);
        }

        private string getColumnText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            if (context.IsSelect
                || (context.IsInsert && context.Options.QualifyInsertColumns)
                || (context.IsUpdate && context.Options.QualifyUpdateColumn)
                || (context.IsDelete && context.Options.QualifyDeleteColumns))
            {
                string table = source.GetReference(context);
                result.Append(table);
                result.Append(".");
            }
            result.Append(name);
            return result.ToString();
        }
    }
}
