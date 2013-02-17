using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a test for the presence of a record in a sub-query.
    /// </summary>
    public class ExistsFilter : Filter
    {
        /// <summary>
        /// Initializes a new instance of an ExistsFilter.
        /// </summary>
        /// <param name="select"></param>
        public ExistsFilter(ISelectBuilder select)
        {
            if (select == null)
            {
                throw new ArgumentNullException("select");
            }
            Select = select;
        }

        /// <summary>
        /// Gets the select builder used to test for the existance of a record.
        /// </summary>
        public ISelectBuilder Select
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override TokenStream GetInnerFilterTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Exists, "EXISTS"));
            stream.AddRange(Select.GetFilterTokens(options));
            return stream;
        }
    }
}
