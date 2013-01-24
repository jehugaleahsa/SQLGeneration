using System;
using System.Globalization;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a numeric literal.
    /// </summary>
    public class NumericLiteral : IArithmetic, ILiteral
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
        /// Gets or sets an alias for the literal.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the numeric value of the literal.
        /// </summary>
        public decimal Value
        {
            get;
            set;
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
