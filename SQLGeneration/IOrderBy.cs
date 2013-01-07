using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item in the order by clause of a select statement.
    /// </summary>
    public interface IOrderBy
    {
        /// <summary>
        /// Gets the items to order by.
        /// </summary>
        IProjectionItem Item
        {
            get;
        }

        /// <summary>
        /// Gets or sets the order to sort the results.
        /// </summary>
        Order Order
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies where null values appear in the results.
        /// </summary>
        NullPlacement NullPlacement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a string representing the order by.
        /// </summary>
        string GetOrderByText();
    }
}
