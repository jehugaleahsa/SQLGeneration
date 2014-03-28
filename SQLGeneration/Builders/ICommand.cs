using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a SQL statement.
    /// </summary>
    public interface ICommand : IVisitableBuilder
    {
    }
}
