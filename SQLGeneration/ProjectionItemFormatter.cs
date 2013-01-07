using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string for a projection item.
    /// </summary>
    public class ProjectionItemFormatter
    {
        /// <summary>
        /// Initializes a new instance of a ProjectionItemFormatter.
        /// </summary>
        public ProjectionItemFormatter()
        {
        }

        /// <summary>
        /// Gets the declaration of a projection item.
        /// </summary>
        /// <param name="item">The item being declared.</param>
        /// <returns>A string declaring the projection item.</returns>
        public string GetDeclaration(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            StringBuilder result = new StringBuilder();
            result.Append(item.GetFullText());
            if (!String.IsNullOrWhiteSpace(item.Alias))
            {
                result.Append(" AS ");
                result.Append(item.Alias);
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets the alias of the projection item if it exists; otherwise, its full text.
        /// </summary>
        /// <param name="item">The item being printed.</param>
        /// <returns>A string referencing the projection item.</returns>
        public string GetAliasedReference(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!String.IsNullOrWhiteSpace(item.Alias))
            {
                return item.Alias;
            }
            else
            {
                return item.GetFullText();
            }
        }

        /// <summary>
        /// Gets the full text of the projection item.
        /// </summary>
        /// <param name="item">The item being printed.</param>
        /// <returns>A string referencing the projection item.</returns>
        public string GetUnaliasedReference(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return item.GetFullText();
        }
    }
}
