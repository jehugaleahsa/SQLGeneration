using System;
using SQLGeneration.Properties;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a database column.
    /// </summary>
    public class Column : IProjectionItem, IGroupByItem, IFilterItem
    {
        /// <summary>
        /// Initializes a new instance of a Column.
        /// </summary>
        /// <param name="source">The column source that the column belongs to.</param>
        /// <param name="name">The name of the column.</param>
        internal Column(AliasedSource source, string name)
        {
            Source = source;
            Name = name;
        }

        /// <summary>
        /// Gets the table that the column belongs to.
        /// </summary>
        public AliasedSource Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets whether the column should be qualified with the source.
        /// </summary>
        public bool? Qualify
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        IEnumerable<string> IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getColumnTokens(options);
        }

        IEnumerable<string> IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getColumnTokens(options);
        }

        IEnumerable<string> IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getColumnTokens(options);
        }

        private IEnumerable<string> getColumnTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            bool qualify = Qualify ?? (options.IsSelect
                || (options.IsInsert && options.QualifyInsertColumns)
                || (options.IsUpdate && options.QualifyUpdateColumn)
                || (options.IsDelete && options.QualifyDeleteColumns));
            if (qualify)
            {
                stream.AddRange(Source.GetReferenceTokens(options));
                stream.Add(".");
            }
            stream.Add(Name);
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return Name;
        }
    }
}
