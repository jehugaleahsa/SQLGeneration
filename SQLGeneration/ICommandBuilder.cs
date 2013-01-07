using System;

namespace SQLGeneration
{
    /// <summary>
    /// Build a string of a SQL statement.
    /// </summary>
    public interface ICommandBuilder
    {
        /// <summary>
        /// Gets the command text.
        /// </summary>
        string GetCommandText();
    }
}
