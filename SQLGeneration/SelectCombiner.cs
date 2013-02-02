using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public IEnumerable<string> GetCommandExpression(CommandOptions options)
        {
            // <SelectCombiner> => <Select> <Combiner> <Select>
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            return getCommandExpression(options);
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected abstract string GetCombinationName(CommandOptions options);

        /// <summary>
        /// Gets or sets an alias when the SELECT command appears in the FROM clause.
        /// </summary>
        public string ColumnSourceAlias
        {
            get;
            set;
        }

        string IColumnSource.Alias
        {
            get { return ColumnSourceAlias; }
            set { ColumnSourceAlias = value; }
        }

        /// <summary>
        /// Gets or sets an alias when the SELECT command appears as a projection.
        /// </summary>
        public string ProjectionAlias
        {
            get;
            set;
        }

        string IProjectionItem.Alias
        {
            get { return ProjectionAlias; }
            set { ProjectionAlias = value; }
        }

        IEnumerable<string> IJoinItem.GetDeclarationExpression(CommandOptions options)
        {
            foreach (string token in getWrappedCommand(options))
            {
                yield return token;
            }
            if (!String.IsNullOrWhiteSpace(ColumnSourceAlias))
            {
                if (options.AliasColumnSourcesUsingAs)
                {
                    yield return "AS";
                }
                yield return ColumnSourceAlias;
            }
        }

        IEnumerable<string> IColumnSource.GetReferenceExpression(CommandOptions options)
        {
            if (String.IsNullOrWhiteSpace(ColumnSourceAlias))
            {
                throw new SQLGenerationException(Resources.ReferencedQueryCombinerWithoutAlias);
            }
            yield return ColumnSourceAlias;
        }

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            return getWrappedCommand(options);
        }

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            return getWrappedCommand(options);
        }

        private IEnumerable<string> getWrappedCommand(CommandOptions options)
        {
            yield return "(";
            foreach (string token in getCommandExpression(options))
            {
                yield return token;
            }
            yield return ")";
        }

        private IEnumerable<string> getCommandExpression(CommandOptions options)
        {
            foreach (string token in leftHand.GetCommandExpression(options))
            {
                yield return token;
            }
            yield return GetCombinationName(options);
            foreach (string token in rightHand.GetCommandExpression(options))
            {
                yield return token;
            }
        }

        bool IValueProvider.IsQuery
        {
            get { return true; }
        }
    }
}
