using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a parameter to a command.
    /// </summary>
    public interface IParameter : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
