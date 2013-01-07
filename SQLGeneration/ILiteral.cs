using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a numeric, string, etc. literal.
    /// </summary>
    public interface ILiteral : IProjectionItem, IFilterItem, IGroupByItem
    {
    }
}
