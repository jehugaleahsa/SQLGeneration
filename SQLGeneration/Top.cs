using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a TOP clause that is found in a SELECT statement.
    /// </summary>
    public class Top : ITop
    {
        private readonly IArithmetic _expression;
        private bool _isPercent;
        private bool _withTies;

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
            get
            {
                return _isPercent;
            }
            set
            {
                _isPercent = value;
            }
        }

        /// <summary>
        /// Gets or sets whether records matching the last item according to the order by
        /// clause shall be returned.
        /// </summary>
        public bool WithTies
        {
            get
            {
                return _withTies;
            }
            set
            {
                _withTies = value;
            }
        }

        /// <summary>
        /// Gets the textual representation of the TOP clause.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        public string GetTopText(BuilderContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TOP ");
            builder.Append(_expression.GetFilterItemText(context));
            if (_isPercent)
            {
                builder.Append(" PERCENT");
            }
            if (_withTies)
            {
                builder.Append(" WITH TIES");
            }
            return builder.ToString();
        }
    }
}
