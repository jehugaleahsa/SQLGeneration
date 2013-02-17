using System;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Allows a table or select statement to be referred to by an alias.
    /// </summary>
    public class AliasedSource : IJoinItem
    {
        /// <summary>
        /// Initializes a new instance of an AliasedSource.
        /// </summary>
        /// <param name="source">The table or SELECT statement acting as the source.</param>
        /// <param name="alias">The alias to refer to the source with.</param>
        internal AliasedSource(IRightJoinItem source, string alias)
        {
            Source = source;
            Alias = alias;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        public IRightJoinItem Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the alias for the source.
        /// </summary>
        public string Alias
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a column that can refer qualify its name with source.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column Column(string columnName)
        {
            if (String.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException(Resources.BlankColumnName, "columnName");
            }
            return new Column(this, columnName);
        }

        /// <summary>
        /// Gets the tokens comprising a declaration of the source.
        /// </summary>
        /// <param name="options">The configuration settings to use to generate the tokens.</param>
        /// <returns>The tokens comprising a reference to the source.</returns>
        TokenStream IJoinItem.GetDeclarationTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(Source.GetDeclarationTokens(options));
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                if (options.AliasColumnSourcesUsingAs)
                {
                    stream.Add(new TokenResult(SqlTokenRegistry.As, "AS"));
                }
                stream.Add(new TokenResult(SqlTokenRegistry.Identifier, Alias));
            }
            return stream;
        }
        
        /// <summary>
        /// Gets the tokens comprising a reference to the source.
        /// </summary>
        /// <param name="options">The configuration settings to use to generate the tokens.</param>
        /// <returns>The tokens comprising a reference to the source..</returns>
        internal TokenStream GetReferenceTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            if (String.IsNullOrWhiteSpace(Alias))
            {
                if (!Source.IsTable)
                {
                    throw new SQLGenerationException(Resources.ReferencedQueryWithoutAlias);
                }
                stream.AddRange(Source.GetDeclarationTokens(options));
            }
            else
            {
                stream.Add(new TokenResult(SqlTokenRegistry.Identifier, Alias));
            }
            return stream;
        }

        /// <summary>
        /// Gets the name used to refer to the source.
        /// </summary>
        /// <returns>The name used to refer to the source.</returns>
        internal string GetSourceName()
        {
            if (String.IsNullOrWhiteSpace(Alias))
            {
                return Source.GetSourceName();
            }
            else
            {
                return Alias;
            }
        }
    }
}
