using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a function call to a command.
    /// </summary>
    public interface IFunction : IProjectionItem, IFilterItem, IGroupByItem
    {
        /// <summary>
        /// Gets the schema the functions belongs to.
        /// </summary>
        ISchema Schema
        {
            get;
        }

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets a list of the arguments being passed to the function.
        /// </summary>
        IEnumerable<IProjectionItem> Arguments
        {
            get;
        }

        /// <summary>
        /// Adds the given projection item to the arguments list.
        /// </summary>
        /// <param name="item">The value to add.</param>
        void AddArgument(IProjectionItem item);

        /// <summary>
        /// Removes the given projection item from the arguments list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        bool RemoveArgument(IProjectionItem item);
    }
}
