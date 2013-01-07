using System;

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
        IInList Arguments
        {
            get;
        }
    }
}
