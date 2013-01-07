using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison that checks that an item is between two values.
    /// </summary>
    public interface IBetweenFilter : IFilter
    {
        /// <summary>
        /// Gets the least the value can be.
        /// </summary>
        IFilterItem LowerBound { get; }

        /// <summary>
        /// Gets the most the value can be.
        /// </summary>
        IFilterItem UpperBound { get; }

        /// <summary>
        /// Gets the value being checked.
        /// </summary>
        IFilterItem Value { get; }
    }
}
