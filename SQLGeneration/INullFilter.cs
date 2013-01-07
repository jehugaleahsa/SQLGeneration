using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a filter comparing a column to null.
    /// </summary>
    public interface INullFilter : IFilter
    {
        /// <summary>
        /// Gets the left hand operand of the filter.
        /// </summary>
        IFilterItem LeftHand
        {
            get;
        }

        /// <summary>
        /// Gets or sets whether the value is null.
        /// </summary>
        bool IsNull
        {
            get;
            set;
        }
    }
}
