using System;
using System.Collections.Generic;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the first source in a series of joins.
    /// </summary>
    internal class JoinStart : Join
    {
        private readonly AliasedSource source;

        /// <summary>
        /// Initializes a new instance of a JoinStart.
        /// </summary>
        /// <param name="source">The first source in a series of joins.</param>
        public JoinStart(AliasedSource source)
            : base(source)
        {
            this.source = source;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        internal AliasedSource Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets a string that declares the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string declaring the item.</returns>
        internal override IEnumerable<string> GetDeclarationTokens(CommandOptions options)
        {
            return ((IJoinItem)source).GetDeclarationTokens(options);
        }
    }
}
