using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Builds a TOP clause that is found in a SELECT statement.
    /// </summary>
    public class Top
    {
        private readonly IProjectionItem _expression;

        /// <summary>
        /// Initializes a new instance of a Top.
        /// </summary>
        /// <param name="expression">The number or percent of items to return.</param>
        public Top(IProjectionItem expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _expression = expression;
        }

        /// <summary>
        /// Gets the expression representing the number or percent of rows to return.
        /// </summary>
        public IProjectionItem Expression
        {
            get
            {
                return _expression;
            }
        }

        /// <summary>
        /// Gets whether or not the expression represents a percent.
        /// </summary>
        public bool IsPercent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether records matching the last item according to the order by
        /// clause shall be returned.
        /// </summary>
        public bool WithTies
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the textual representation of the TOP clause.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        internal TokenStream GetTopTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Top, "TOP"));
            stream.AddRange(_expression.GetProjectionTokens(options));
            if (IsPercent)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.Percent, "PERCENT"));
            }
            if (WithTies)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.WithTies, "WITH TIES"));
            }
            return stream;
        }
    }
}
