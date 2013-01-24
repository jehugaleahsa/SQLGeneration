using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a column being set to a value to the command.
    /// </summary>
    public interface ISetter
    {
        /// <summary>
        /// Gets the column being set.
        /// </summary>
        IColumn Column
        {
            get;
        }

        /// <summary>
        /// Gets the value that the column is being set to.
        /// </summary>
        IProjectionItem Value
        {
            get;
        }

        /// <summary>
        /// Gets a string representing the setter.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The generated text.</returns>
        string GetSetterText(BuilderContext context);
    }
}
