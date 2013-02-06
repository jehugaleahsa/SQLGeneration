using System;
using System.Collections.Generic;
using System.Globalization;
using SQLGeneration.Parsing;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a numeric literal.
    /// </summary>
    public class NumericLiteral : IArithmetic, IProjectionItem, IFilterItem, IGroupByItem
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

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getNumberToken();
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getNumberToken();
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getNumberToken();
        }

        private IEnumerable<string> getNumberToken()
        {
            TokenStream stream = new TokenStream();
            stream.Add(Value.ToString(CultureInfo.InvariantCulture));
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }
    }
}
