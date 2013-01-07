using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents the literal NULL.
    /// </summary>
    public class NullLiteral : ILiteral
    {
        private string _alias;

        /// <summary>
        /// Creates a new NullLiteral.
        /// </summary>
        public NullLiteral()
        {
        }

        /// <summary>
        /// Gets or sets an alias for the null.
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

        string IProjectionItem.GetFullText()
        {
            return "NULL";
        }

        string IFilterItem.GetFilterItemText()
        {
            return "NULL";
        }

        string IGroupByItem.GetGroupByItemText()
        {
            return "NULL";
        }
    }
}
