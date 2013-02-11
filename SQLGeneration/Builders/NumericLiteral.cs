using System;
using System.Collections.Generic;
using System.Globalization;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a numeric literal.
    /// </summary>
    public class NumericLiteral : Literal
    {
        /// <summary>
        /// Initializes a new instance of a NumericLiteral.
        /// </summary>
        public NumericLiteral()
        {
        }

        /// <summary>
        /// Initializes a new instance of a NumericLiteral.
        /// </summary>
        /// <param name="value">The value to make the literal.</param>
        public NumericLiteral(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the numeric value of the literal.
        /// </summary>
        public decimal Value
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
            TokenStream stream = new TokenStream();
            stream.Add(Value.ToString(CultureInfo.InvariantCulture));
            return stream;
        }
    }
}
