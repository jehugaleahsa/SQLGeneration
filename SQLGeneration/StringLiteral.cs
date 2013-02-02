using System;
using System.Text;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a literal string.
    /// </summary>
    public class StringLiteral : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Initializes a new instance of a StringLiteral.
        /// </summary>
        public StringLiteral()
        {
            Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of a StringLiteral.
        /// </summary>
        /// <param name="value">The string value.</param>
        public StringLiteral(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value of the string literal.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an alias for the string.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            yield return getString();
        }

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            yield return getString();
        }

        IEnumerable<string> IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            yield return getString();
        }

        private string getString()
        {
            // "'" .* "'"
            StringBuilder result = new StringBuilder();
            result.Append("'");
            if (Value != null)
            {
                result.Append(Value.Replace("'", "''"));
            }
            result.Append("'");
            return result.ToString();
        }
    }
}
