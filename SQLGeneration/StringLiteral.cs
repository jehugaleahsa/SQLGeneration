using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a literal string.
    /// </summary>
    public class StringLiteral : ILiteral
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

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return getText();
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return getText();
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return getText();
        }

        private string getText()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("'");
            if (Value != null)
            {
                // escape quotes
                int index = builder.Length;
                builder.Append(Value);
                builder.Replace("'", "''", index, Value.Length);
            }
            builder.Append("'");
            return builder.ToString();
        }
    }
}
