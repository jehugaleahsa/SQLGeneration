using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Selects all of the columns in a table or a join.
    /// </summary>
    public class AllColumns : IProjectionItem
    {
        private readonly IJoinItem _joinItem;
        private string _alias;

        /// <summary>
        /// Initializes a new instacne of an AllColumns
        /// that doesn't have a table or join.
        /// </summary>
        public AllColumns()
        {
        }

        /// <summary>
        /// Initializes a new instance of an AllColumns
        /// that selects all the columns from the given table or join.
        /// </summary>
        /// <param name="joinItem">The table or join to select all the columns from.</param>
        public AllColumns(IJoinItem joinItem)
        {
            _joinItem = joinItem;
        }

        /// <summary>
        /// Gets or sets an alias. This is ignored.
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

        /// <summary>
        /// Gets a textual representation of the column selection.
        /// </summary>
        /// <returns>The textual representation.</returns>
        string IProjectionItem.GetFullText()
        {
            StringBuilder builder = new StringBuilder();
            if (_joinItem != null)
            {
                builder.Append(_joinItem.GetReference());
                builder.Append(".");
            }
            builder.Append("*");
            return builder.ToString();
        }
    }
}
