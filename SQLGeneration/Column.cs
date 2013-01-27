using System;
using SQLGeneration.Expressions;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a database column.
    /// </summary>
    public class Column : IProjectionItem, IGroupByItem, IFilterItem
    {
        private readonly IColumnSource source;
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of a Column.
        /// </summary>
        /// <param name="source">The column source that the column belongs to.</param>
        /// <param name="name">The name of the column.</param>
        internal Column(IColumnSource source, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankColumnName, "name");
            }
            this.source = source;
            this.name = name;
        }

        /// <summary>
        /// Gets the table that the column belongs to.
        /// </summary>
        public IColumnSource Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the alias of the column.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        IExpressionItem IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            return getColumnExpression(options);
        }

        IExpressionItem IFilterItem.GetFilterExpression(CommandOptions options)
        {
            return getColumnExpression(options);
        }

        IExpressionItem IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            return getColumnExpression(options);
        }

        private IExpressionItem getColumnExpression(CommandOptions options)
        {
            Expression expression = new Expression();
            expression.AddItem(source.GetReferenceExpression(options));
            expression.AddItem(new Token("."));
            expression.AddItem(new Token(name));
            return expression;
        }
    }
}
