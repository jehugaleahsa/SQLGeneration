using System;
using System.Text;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Selects all of the columns in a table or a join.
    /// </summary>
    public class AllColumns : IProjectionItem
    {
        private readonly IColumnSource source;

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
        /// <param name="source">The table or join to select all the columns from.</param>
        public AllColumns(IColumnSource source)
        {
            this.source = source;
        }

        /// <summary>
        /// Gets or sets an alias. This is ignored.
        /// </summary>
        public string Alias
        {
            get { return String.Empty; }
            set { }
        }

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            // <Column> => [ <Source> "." ] "*"
            StringBuilder builder = new StringBuilder();
            if (source != null)
            {
                foreach (string token in source.GetReferenceExpression(options))
                {
                    yield return token;
                }
                yield return ".";
            }
            yield return "*";
        }
    }
}
