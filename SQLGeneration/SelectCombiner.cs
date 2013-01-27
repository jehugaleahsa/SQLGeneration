using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Expressions;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Performs a set operation on the results of two queries.
    /// </summary>
    public abstract class SelectCombiner : ISelectBuilder
    {
        private readonly List<ISelectBuilder> _queries;

        /// <summary>
        /// Initializes a new instance of a SelectCombiner.
        /// </summary>
        protected SelectCombiner()
        {
            _queries = new List<ISelectBuilder>();
        }

        /// <summary>
        /// Creates a new column under the table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName)
        {
            return new Column(this, columnName);
        }

        /// <summary>
        /// Creates a new column under the multi-select.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName, string alias)
        {
            return new Column(this, columnName);
        }

        /// <summary>
        /// Gets the queries that are to be combined.
        /// </summary>
        public IEnumerable<ISelectBuilder> Queries
        {
            get { return new ReadOnlyCollection<ISelectBuilder>(_queries); }
        }

        /// <summary>
        /// Adds the query to the combination.
        /// </summary>
        /// <param name="query">The query to add.</param>
        public void AddQuery(ISelectBuilder query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            _queries.Add(query);
        }

        /// <summary>
        /// Removes the query from the combination.
        /// </summary>
        /// <param name="query">The query to remove.</param>
        /// <returns>True if the query is removed; otherwise, false.</returns>
        public bool RemoveQuery(ISelectBuilder query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return _queries.Remove(query);
        }

        /// <summary>
        /// Gets the command expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expression making up the command.</returns>
        public IExpressionItem GetCommandExpression(CommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (_queries.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoQueries);
            }
            IExpressionItem separator = GetCombinationExpression(options);
            return Expression.Join(separator, _queries.Select(query => query.GetCommandExpression(options)));
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected abstract IExpressionItem GetCombinationExpression(CommandOptions options);

        /// <summary>
        /// Gets or sets an alias for the query results.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        IExpressionItem IJoinItem.GetDeclarationExpression(CommandOptions options, FilterGroup where)
        {
            Expression expression = new Expression();
            if (_queries.Count > 1)
            {
                expression.AddItem(new Token("("));
                expression.AddItem(GetCommandExpression(options));
                expression.AddItem(new Token(")"));
            }
            else
            {
                expression.AddItem(GetCommandExpression(options));
            }
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                if (options.AliasColumnSourcesUsingAs)
                {
                    expression.AddItem(new Token("AS"));
                }
                expression.AddItem(new Token(Alias));
            }
            return expression;
        }

        IExpressionItem IColumnSource.GetReferenceExpression(CommandOptions options)
        {
            if (String.IsNullOrWhiteSpace(Alias))
            {
                throw new SQLGenerationException(Resources.ReferencedQueryCombinerWithoutAlias);
            }
            return new Token(Alias);
        }

        IExpressionItem IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            Expression expression = new Expression();
            expression.AddItem(new Token("("));
            expression.AddItem(GetCommandExpression(options));
            expression.AddItem(new Token(")"));
            return expression;
        }

        IExpressionItem IFilterItem.GetFilterExpression(CommandOptions options)
        {
            Expression expression = new Expression();
            expression.AddItem(new Token("("));
            expression.AddItem(GetCommandExpression(options));
            expression.AddItem(new Token(")"));
            return expression;
        }

        bool IValueProvider.IsQuery
        {
            get { return true; }
        }
    }
}
