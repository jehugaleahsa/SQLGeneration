using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a source of values in an insert statement.
    /// </summary>
    public interface IValueProvider : IFilterItem
    {
        /// <summary>
        /// Gets or sets whether the value provider gets its values from a query.
        /// </summary>
        bool IsQuery
        {
            get;
        }
    }
}
