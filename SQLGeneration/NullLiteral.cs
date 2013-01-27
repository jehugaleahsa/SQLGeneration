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

        IExpressionItem IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            return new Token("NULL");
        }

        IExpressionItem IFilterItem.GetFilterExpression(CommandOptions options)
        {
            return new Token("NULL");
        }

        IExpressionItem IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            return new Token("NULL");
        }
    }
}
