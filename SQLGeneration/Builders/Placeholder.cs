using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Acts as a placeholder anywhere within the options of a SQL statement.
    /// </summary>
    public class Placeholder : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Initializes a new instance of a Placeholder.
        /// </summary>
        /// <param name="value">The value of the placeholder.</param>
        public Placeholder(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of the placeholder.
        /// </summary>
        public string Value
        {
            get;
            private set;
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getPlaceholderToken();
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getPlaceholderToken();
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getPlaceholderToken();
        }

        private IEnumerable<string> getPlaceholderToken()
        {
            TokenStream stream = new TokenStream();
            stream.Add(Value);
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }
    }
}
