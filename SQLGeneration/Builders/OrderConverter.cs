using System;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Specifies the order that results are sorted.
    /// </summary>
    public enum Order
    {
        /// <summary>
        /// Sorts the result using the default ordering.
        /// </summary>
        Default,
        /// <summary>
        /// Sort the results in ascending order.
        /// </summary>
        Ascending,
        /// <summary>
        /// Sorts the results in descending order.
        /// </summary>
        Descending
    }

    /// <summary>
    /// Converts between representations of the Order enum.
    /// </summary>
    internal class OrderConverter
    {
        /// <summary>
        /// Initializes a new instance of a OrderConverter.
        /// </summary>
        public OrderConverter()
        {
        }

        /// <summary>
        /// Gets the string representation of an Order enum.
        /// </summary>
        /// <param name="order">The value of the enum.</param>
        /// <returns>The string representation.</returns>
        public TokenResult ToToken(Order order)
        {
            switch (order)
            {
                case Order.Ascending:
                    return new TokenResult(SqlTokenRegistry.Ascending, "ASC");
                case Order.Descending:
                    return new TokenResult(SqlTokenRegistry.Descending, "DESC");
                default:
                    throw new ArgumentException(Resources.UnknownOrder, "order");
            }
        }
    }
}
