using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the literal NULL.
    /// </summary>
    public class NullLiteral : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Initializes a new instance of a NullLiteral.
        /// </summary>
        public NullLiteral()
        {
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getNullToken();
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getNullToken();
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getNullToken();
        }

        private static IEnumerable<string> getNullToken()
        {
            // "NULL"
            TokenStream stream = new TokenStream();
            stream.Add("NULL");
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }
    }
}
