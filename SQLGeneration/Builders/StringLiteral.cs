using System;
using System.Collections.Generic;
using System.Text;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a literal string.
    /// </summary>
    public class StringLiteral : Literal
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
        /// Gets a string representing the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        protected override IEnumerable<string> GetTokens(CommandOptions options)
        {
            StringBuilder result = new StringBuilder();
            result.Append("'");
            if (Value != null)
            {
                result.Append(Value.Replace("'", "''"));
            }
            result.Append("'");
            TokenStream stream = new TokenStream();
            stream.Add(result.ToString());
            return stream;
        }
    }
}
