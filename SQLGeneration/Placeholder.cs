using System;

namespace SQLGeneration
{
    /// <summary>
    /// Acts as a placeholder anywhere within the context of a SQL statement.
    /// </summary>
    public class Placeholder : ILiteral
    {
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of a Placeholder.
        /// </summary>
        /// <param name="value">The value of the placeholder.</param>
        public Placeholder(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets an alias for the placeholder. This is ignored.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return value;
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return value;
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return value;
        }
    }
}
