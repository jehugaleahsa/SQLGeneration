using System;
using SQLGeneration.Properties;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Represents a keyword or delimiter token.
    /// </summary>
    public class Token : IExpressionItem
    {
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of a Token.
        /// </summary>
        /// <param name="value">The fixed value of the token.</param>
        public Token(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.value = value;
        }

        /// <summary>
        /// Gets the expression that is the parent of the token.
        /// </summary>
        public Expression Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value of the token.
        /// </summary>
        public string Value
        {
            get { return value; }
        }

        /// <summary>
        /// Visits the current expression item.
        /// </summary>
        /// <param name="visiter">A function that will be passed a token when it is encountered.</param>
        public void Visit(Action<Token> visiter)
        {
            visiter(this);
        }
    }
}
