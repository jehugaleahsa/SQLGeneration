using System;
using System.Collections.Generic;

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
        public IEnumerable<string> GetDeclaration(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            foreach (string token in item.GetProjectionExpression(options))
            {
                yield return token;
            }
            if (!String.IsNullOrWhiteSpace(item.Alias))
            {
                if (options.AliasProjectionsUsingAs)
                {
                    yield return "AS";
                }
                yield return item.Alias;
            }
        }

        /// <summary>
        /// Gets the alias of the projection item if it exists; otherwise, its full text.
        /// </summary>
        /// <param name="item">The item being printed.</param>
        /// <returns>A string referencing the projection item.</returns>
        public IEnumerable<string> GetAliasedReference(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!String.IsNullOrWhiteSpace(item.Alias))
            {
                yield return item.Alias;
            }
            else
            {
                foreach (string token in item.GetProjectionExpression(options))
                {
                    yield return token;
                }
            }
        }

        /// <summary>
        /// Gets the full text of the projection item.
        /// </summary>
        /// <param name="item">The item being printed.</param>
        /// <returns>A string referencing the projection item.</returns>
        public IEnumerable<string> GetUnaliasedReference(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return item.GetProjectionExpression(options);
        }
    }
}
