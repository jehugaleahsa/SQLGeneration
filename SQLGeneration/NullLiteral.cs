using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents the literal NULL.
    /// </summary>
    public class NullLiteral : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Initializes a new instance of a NullLiteral.
        /// </summary>
        public NullLiteral()
        {
        }

        /// <summary>
        /// Gets or sets an alias for the null.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        void IProjectionItem.GetProjectionExpression(Expression expression, CommandOptions options)
        {
            getNullLiteral(expression);
        }

        void IFilterItem.GetFilterExpression(Expression expression, CommandOptions options)
        {
            getNullLiteral(expression);
        }

        void IGroupByItem.GetGroupByExpression(Expression expression, CommandOptions options)
        {
            getNullLiteral(expression);
        }

        private static void getNullLiteral(Expression expression)
        {
            // "NULL"
            expression.AddItem(new Token("NULL"));
        }
    }
}
