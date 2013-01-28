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
        private readonly ISelectBuilder leftHand;
        private readonly ISelectBuilder rightHand;

        /// <summary>
        /// Initializes a new instance of a SelectCombiner.
        /// </summary>
        protected SelectCombiner(ISelectBuilder leftHand, ISelectBuilder rightHand)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            this.leftHand = leftHand;
            this.rightHand = rightHand;
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
        /// Gets the SELECT command on the left side.
        /// </summary>
        public ISelectBuilder LeftHand
        {
            get { return leftHand; }
        }

        /// <summary>
        /// Gets the SELECT comman on the right side.
        /// </summary>
        public ISelectBuilder RightHand
        {
            get { return rightHand; }
        }

        /// <summary>
        /// Gets the command expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expression making up the command.</returns>
        public IExpressionItem GetCommandExpression(CommandOptions options)
        {
            // <Select> <Combiner> <Select>
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            Expression expression = new Expression();
            expression.AddItem(leftHand.GetCommandExpression(options));
            IExpressionItem separator = GetCombinationName(options);
            expression.AddItem(rightHand.GetCommandExpression(options));
            return expression;
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected abstract Token GetCombinationName(CommandOptions options);

        /// <summary>
        /// Gets or sets an alias for the query results.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        IExpressionItem IJoinItem.GetDeclarationExpression(CommandOptions options)
        {
            Expression expression = new Expression();
            expression.AddItem(new Token("("));
            expression.AddItem(GetCommandExpression(options));
            expression.AddItem(new Token(")"));
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
            return getCombinedCommand(options);
        }

        IExpressionItem IFilterItem.GetFilterExpression(CommandOptions options)
        {
            return getCombinedCommand(options);
        }

        private IExpressionItem getCombinedCommand(CommandOptions options)
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
