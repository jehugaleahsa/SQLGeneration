using System;
using System.Text;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration
{
    /// <summary>
    /// Selects all of the columns in a table or a join.
    /// </summary>
    public class AllColumns : IProjectionItem
    {
        private readonly AliasedSource source;

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
        public AllColumns(AliasedSource source)
        {
            this.source = source;
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            if (source != null)
            {
                stream.AddRange(source.GetReferenceTokens(options));
                stream.Add(".");
            }
            stream.Add("*");
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }
    }
}
