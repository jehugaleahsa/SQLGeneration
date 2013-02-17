using System;
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
        public NumericLiteral(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the numeric value of the literal.
        /// </summary>
        public double Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the format to represent the value with.
        /// </summary>
        public string Format
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a string representing the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        protected override TokenStream GetTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            if (Format == null)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.Number, Value.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                stream.Add(new TokenResult(SqlTokenRegistry.Number, Value.ToString(Format, CultureInfo.InvariantCulture)));
            }
            return stream;
        }
    }
}
