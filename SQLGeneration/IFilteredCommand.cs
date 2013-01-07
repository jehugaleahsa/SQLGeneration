using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a command that can be filtered.
    /// </summary>
    public interface IFilteredCommand
    {
        /// <summary>
        /// Gets a filter to apply to the query.
        /// </summary>
        IFilterGroup Where
        {
            get;
        }
    }
}
