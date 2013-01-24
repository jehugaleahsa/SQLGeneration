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
        private readonly IJoinItem _joinItem;
        private readonly string _name;
        private string _alias;

        /// <summary>
        /// Initializes a new instance of a Column.
        /// </summary>
        /// <param name="joinItem">The join item that the column belongs to.</param>
        /// <param name="name">The name of the column.</param>
        internal Column(IJoinItem joinItem, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankColumnName, "name");
            }
            _joinItem = joinItem;
            _name = name;
        }

        /// <summary>
        /// Gets the table that the column belongs to.
        /// </summary>
        public IJoinItem JoinItem
        {
            get { return _joinItem; }
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets or sets the alias of the column.
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

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            string table = _joinItem.GetReference(context);
            result.Append(table);
            result.Append(".");
            result.Append(_name);
            return result.ToString();
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            string table = _joinItem.GetReference(context);
            result.Append(table);
            result.Append(".");
            result.Append(_name);
            return result.ToString();
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder();
            string table = _joinItem.GetReference(context);
            result.Append(table);
            result.Append(".");
            result.Append(_name);
            return result.ToString();
        }
    }
}
