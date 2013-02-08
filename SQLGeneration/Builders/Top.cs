using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Builds a TOP clause that is found in a SELECT statement.
    /// </summary>
    public class Top
    {
        private readonly IArithmetic _expression;

        /// <summary>
        /// Initializes a new instance of a Top.
        /// </summary>
        /// <param name="expression">The number or percent of items to return.</param>
        public Top(IArithmetic expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _expression = expression;
        }

        /// <summary>
        /// Gets the arithmetic expression representing the number or percent of rows to return.
        /// </summary>
        public IArithmetic Expression
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
        internal IEnumerable<string> GetTopTokens(CommandOptions options)
        {
            // <Top> => "TOP" <Arithmetic> [ "PERCENT" ] [ "WITH TIES" ]
            TokenStream stream = new TokenStream();
            stream.Add("TOP");
            stream.AddRange(_expression.GetFilterTokens(options));
            if (IsPercent)
            {
                stream.Add("PERCENT");
            }
            if (WithTies)
            {
                stream.Add("WITH TIES");
            }
            return stream;
        }
    }
}
