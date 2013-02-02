using System;
using System.Globalization;
using System.Collections.Generic;

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

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            yield return getNumericLiteral();
        }

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            yield return getNumericLiteral();
        }

        IEnumerable<string> IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            yield return getNumericLiteral();
        }

        private string getNumericLiteral()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
