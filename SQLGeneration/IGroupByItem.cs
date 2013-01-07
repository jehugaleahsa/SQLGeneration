using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item that can appear in a group by clause in a select statement.
    /// </summary>
    public interface IGroupByItem
    {
        /// <summary>
        /// Gets a string representation of the group by.
        /// </summary>
        string GetGroupByItemText();
    }
}
