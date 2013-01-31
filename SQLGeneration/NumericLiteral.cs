using System;
using System.Globalization;
using SQLGeneration.Expressions;

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

        void IProjectionItem.GetProjectionExpression(Expression expression, CommandOptions options)
        {
            getNumericLiteral(expression);
        }

        void IFilterItem.GetFilterExpression(Expression expression, CommandOptions options)
        {
            getNumericLiteral(expression);
        }

        void IGroupByItem.GetGroupByExpression(Expression expression, CommandOptions options)
        {
            getNumericLiteral(expression);
        }

        private void getNumericLiteral(Expression expression)
        {
            string value = Value.ToString(CultureInfo.InvariantCulture);
            expression.AddItem(new Token(value, TokenType.Number));
        }
    }
}
