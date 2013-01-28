using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
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
        public IExpressionItem GetTopExpression(CommandOptions options)
        {
            // "TOP" <Arithmetic> [ "PERCENT" ] [ "WITH TIES" ]
            Expression expression = new Expression();
            expression.AddItem(new Token("TOP"));
            expression.AddItem(_expression.GetFilterExpression(options));
            if (IsPercent)
            {
                expression.AddItem(new Token("PERCENT"));
            }
            if (WithTies)
            {
                expression.AddItem(new Token("WITH"));
                expression.AddItem(new Token("TIES"));
            }
            return expression;
        }
    }
}
