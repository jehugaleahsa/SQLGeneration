using System;

namespace SQLGeneration
{
    /// <summary>
    /// Build a string of a SQL statement.
    /// </summary>
    public interface ICommandBuilder
    {
        /// <summary>
        /// Gets the command text, using the default formatting options.
        /// </summary>
        /// <returns>The command text.</returns>
        string GetCommandText();

        /// <summary>
        /// Gets the command, formatting it using the given options.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The command text.</returns>
        string GetCommandText(BuilderContext context);
    }
}
