using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a literal string.
    /// </summary>
    public class StringLiteral : ILiteral
    {
        private string _value;
        private string _alias;

        /// <summary>
        /// Initializes a new instance of a StringLiteral.
        /// </summary>
        public StringLiteral()
        {
            _value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of a StringLiteral.
        /// </summary>
        /// <param name="value">The string value.</param>
        public StringLiteral(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets or sets the value of the string literal.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Gets or sets an alias for the string.
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
            if (_value != null)
            {
                // escape quotes
                builder.Append(_value.Replace("'", "''"));
            }
            builder.Append("'");
            return builder.ToString();
        }
    }
}
