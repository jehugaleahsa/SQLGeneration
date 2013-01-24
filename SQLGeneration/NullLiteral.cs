using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents the literal NULL.
    /// </summary>
    public class NullLiteral : ILiteral
    {
        /// <summary>
        /// Initializes a new instance of a NullLiteral.
        /// </summary>
        public NullLiteral()
        {
        }

        /// <summary>
        /// Gets or sets an alias for the null.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return "NULL";
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return "NULL";
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return "NULL";
        }
    }
}
