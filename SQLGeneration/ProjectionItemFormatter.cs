using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string for a projection item.
    /// </summary>
    public class ProjectionItemFormatter
    {
        private readonly CommandOptions options;

        /// <summary>
        /// Initializes a new instance of a ProjectionItemFormatter.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        public ProjectionItemFormatter(CommandOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Gets the declaration of a projection item.
        /// </summary>
        /// <param name="item">The item being declared.</param>
        /// <returns>A string declaring the projection item.</returns>
        public IExpressionItem GetDeclaration(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            IExpressionItem projection = item.GetProjectionExpression(options);
            Expression expression = new Expression();
            expression.AddItem(projection);
            if (!String.IsNullOrWhiteSpace(item.Alias))
            {
                if (options.AliasProjectionsUsingAs)
                {
                    expression.AddItem(new Token("AS"));
                }
                expression.AddItem(new Token(item.Alias));
            }
            return expression;
        }

        /// <summary>
        /// Gets the alias of the projection item if it exists; otherwise, its full text.
        /// </summary>
        /// <param name="item">The item being printed.</param>
        /// <returns>A string referencing the projection item.</returns>
        public IExpressionItem GetAliasedReference(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!String.IsNullOrWhiteSpace(item.Alias))
            {
                return new Token(item.Alias);
            }
            else
            {
                return item.GetProjectionExpression(options);
            }
        }

        /// <summary>
        /// Gets the full text of the projection item.
        /// </summary>
        /// <param name="item">The item being printed.</param>
        /// <returns>A string referencing the projection item.</returns>
        public IExpressionItem GetUnaliasedReference(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return item.GetProjectionExpression(options);
        }
    }
}
