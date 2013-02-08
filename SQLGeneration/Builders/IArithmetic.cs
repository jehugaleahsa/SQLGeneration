using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents an arithmetical item, such as a number or mathematical expression.
    /// </summary>
    public interface IArithmetic : IProjectionItem, IGroupByItem, IFilterItem
    {
    }
}
