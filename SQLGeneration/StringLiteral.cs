using System;
using System.Text;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a literal string.
    /// </summary>
    public class StringLiteral : IProjectionItem, IFilterItem, IGroupByItem
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

        void IProjectionItem.GetProjectionExpression(Expression expression, CommandOptions options)
        {
            getExpression(expression);
        }

        void IFilterItem.GetFilterExpression(Expression expression, CommandOptions options)
        {
            getExpression(expression);
        }

        void IGroupByItem.GetGroupByExpression(Expression expression, CommandOptions options)
        {
            getExpression(expression);
        }

        private void getExpression(Expression expression)
        {
            // "'" .* "'"
            StringBuilder result = new StringBuilder();
            result.Append("'");
            if (Value != null)
            {
                result.Append(Value.Replace("'", "''"));
            }
            result.Append("'");
            expression.AddItem(new Token(result.ToString(), TokenType.String));
        }
    }
}
