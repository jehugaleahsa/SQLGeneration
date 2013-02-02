using System;
using System.Collections.Generic;

namespace SQLGeneration
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

        /// <summary>
        /// Gets or sets an alias for the null.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            yield return getNullLiteral();
        }

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            yield return getNullLiteral();
        }

        IEnumerable<string> IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            yield return getNullLiteral();
        }

        private static string getNullLiteral()
        {
            // "NULL"
            return "NULL";
        }
    }
}
