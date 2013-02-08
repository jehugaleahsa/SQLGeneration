using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Builds SELECT statements.
    /// </summary>
    public interface ISelectBuilder : ICommand, IRightJoinItem, IProjectionItem, IValueProvider
    {
    }
}
