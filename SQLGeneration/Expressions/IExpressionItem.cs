using System;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Represents a token in SQL command text.
    /// </summary>
    public interface IExpressionItem
    {
        /// <summary>
        /// Gets the expression that is the parent of the item.
        /// </summary>
        Expression Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Visits the current expression item.
        /// </summary>
        /// <param name="visiter">A function that will be passed a token when it is encountered.</param>
        void Visit(Action<Token> visiter);
    }
}
