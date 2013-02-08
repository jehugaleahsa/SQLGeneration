using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Allows a column to be optionally referred to using an alias.
    /// </summary>
    public class AliasedProjection
    {
        /// <summary>
        /// Initializes a new instance of an AliasedProjection.
        /// </summary>
        /// <param name="item">The projection item.</param>
        /// <param name="alias">The alias to refer to the item with.</param>
        internal AliasedProjection(IProjectionItem item, string alias = null)
        {
            ProjectionItem = item;
            Alias = alias;
        }

        /// <summary>
        /// Gets the projection item.
        /// </summary>
        public IProjectionItem ProjectionItem
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets or sets the alias for the projection item.
        /// </summary>
        public string Alias
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tokens comprising the declaration of the projection.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating the tokens.</param>
        /// <returns>The tokens comprising a declaration of the projection.</returns>
        internal IEnumerable<string> GetDeclarationTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(ProjectionItem.GetProjectionTokens(options));
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                if (options.AliasProjectionsUsingAs)
                {
                    stream.Add("AS");
                }
                stream.Add(Alias);
            }
            return stream;
        }

        /// <summary>
        /// Gets the tokens comprising a reference to the projection.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating the tokens.</param>
        /// <returns>The tokens comprising a reference to the projection.</returns>
        internal IEnumerable<string> GetReferenceTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            if (String.IsNullOrWhiteSpace(Alias))
            {
                stream.AddRange(ProjectionItem.GetProjectionTokens(options));
            }
            else
            {
                stream.Add(Alias);
            }
            return stream;
        }

        /// <summary>
        /// Gets the name that will be used to refer to the projection.
        /// </summary>
        /// <returns>The name that will be used to refer to the projection.</returns>
        internal string GetProjectionName()
        {
            if (String.IsNullOrWhiteSpace(Alias))
            {
                return ProjectionItem.GetProjectionName();
            }
            else
            {
                return Alias;
            }
        }
    }
}
