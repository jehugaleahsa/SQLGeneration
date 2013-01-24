using System;
using System.Globalization;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a numeric literal.
    /// </summary>
    public class NumericLiteral : IArithmetic, ILiteral
    {
        private decimal _value;
        private string _alias;

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
            _value = value;
        }

        /// <summary>
        /// Gets or sets an alias for the literal.
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

        /// <summary>
        /// Gets or sets the numeric value of the literal.
        /// </summary>
        public decimal Value
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

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        string IGroupByItem.GetGroupByItemText(BuilderContext context)
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
