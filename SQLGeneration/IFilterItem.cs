using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in a filter.
    /// </summary>
    public interface IFilterItem
    {
        /// <summary>
        /// Gets a string representing the item.
        /// </summary>
        /// <param name="expression">The expression currently being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        void GetFilterExpression(Expression expression, CommandOptions options);
    }
}
