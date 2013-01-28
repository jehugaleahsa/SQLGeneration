using System;

namespace SQLGeneration
{
    /// <summary>
    /// Builds SELECT statements.
    /// </summary>
    public interface ISelectBuilder : ICommand, IColumnSource, IRightJoinItem, IProjectionItem, IValueProvider
    {
    }
}
