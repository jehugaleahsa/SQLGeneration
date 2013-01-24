using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a join between two tables, joins or sub-queries.
    /// </summary>
    public abstract class Join : IJoin
    {
        private readonly IJoinItem _leftHand;
        private readonly IJoinItem _rightHand;
        private readonly List<IFilter> _on;
        private bool _wrapInParentheses;
        private string _alias;

        /// <summary>
        /// Initializes a new instance of a Join.
        /// </summary>
        /// <param name="leftHand">The left hand table, join or sub-query.</param>
        /// <param name="rightHand">The right hand table, join or sub-query.</param>
        protected Join(IJoinItem leftHand, IJoinItem rightHand)
            : this(leftHand, rightHand, new IFilter[0])
        {
        }

                /// <summary>
        /// Initializes a new instance of a InnerJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        /// <param name="filters">The filters to join to the join items on.</param>
        protected Join(IJoinItem leftHand, IJoinItem rightHand, IEnumerable<IFilter> filters)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }
            _leftHand = leftHand;
            _rightHand = rightHand;
            _on = new List<IFilter>();
            _on.AddRange(filters);
        }

        /// <summary>
        /// Gets or sets whether the join should be wrapped in parentheses.
        /// </summary>
        public bool WrapInParentheses
        {
            get
            {
                return _wrapInParentheses;
            }
            set
            {
                _wrapInParentheses = value;
            }
        }

        /// <summary>
        /// Gets the item on the left hand side of the join.
        /// </summary>
        public IJoinItem LeftHand
        {
            get
            {
                return _leftHand;
            }
        }

        /// <summary>
        /// Gets the item on the right hand side of the join.
        /// </summary>
        public IJoinItem RightHand
        {
            get
            {
                return _rightHand;
            }
        }

        /// <summary>
        /// Gets the filters by which the left and right hand items are joined.
        /// </summary>
        public IEnumerable<IFilter> On
        {
            get
            {
                return new ReadOnlyCollection<IFilter>(_on);
            }
        }

        /// <summary>
        /// Creates a new column under the join.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName)
        {
            return new Column(this, columnName);
        }

        IColumn IJoinItem.CreateColumn(string columnName)
        {
            return CreateColumn(columnName);
        }

        /// <summary>
        /// Creates a new column under the join with the given alias.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName, string alias)
        {
            return new Column(this, columnName) { Alias = alias };
        }

        IColumn IJoinItem.CreateColumn(string columnName, string alias)
        {
            return CreateColumn(columnName, alias);
        }

        /// <summary>
        /// Adds a condition by which the items are joined.
        /// </summary>
        /// <param name="filter">The join condition.</param>
        public void AddFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            _on.Add(filter);
        }

        /// <summary>
        /// Removes a condition by which the items are joined.
        /// </summary>
        /// <param name="filter">The join condition.</param>
        /// <returns>True if the filter was removed; otherwise, false.</returns>
        public bool RemoveFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return _on.Remove(filter);
        }

        /// <summary>
        /// Gets or sets an alias for the join.
        /// </summary>
        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        string IJoinItem.GetDeclaration(BuilderContext context, IFilterGroup where)
        {
            StringBuilder result = new StringBuilder();
            if (_wrapInParentheses || !String.IsNullOrWhiteSpace(_alias))
            {
                result.Append("(");
            }
            string leftHand = _leftHand.GetDeclaration(context, where);
            string rightHand = _rightHand.GetDeclaration(context, where);
            result.Append(combine(context, leftHand, rightHand));
            result.Append(" ON ");
            FilterGroup on = new FilterGroup();
            foreach (IFilter filter in _on)
            {
                on.AddFilter(filter);
            }
            result.Append(((IFilter)on).GetFilterText(context));
            if (_wrapInParentheses || !String.IsNullOrWhiteSpace(_alias))
            {
                result.Append(")");
            }
            if (!String.IsNullOrWhiteSpace(_alias))
            {
                result.Append(" ");
                result.Append(_alias);
            }
            return result.ToString();
        }

        string IJoinItem.GetReference(BuilderContext context)
        {
            if (String.IsNullOrWhiteSpace(_alias))
            {
                throw new SQLGenerationException(Resources.ReferencedJoinWithoutAlias);
            }
            else
            {
                return _alias;
            }
        }

        /// <summary>
        /// Combines the left and right items with the type of join.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <param name="leftHand">The left item.</param>
        /// <param name="rightHand">The right item.</param>
        /// <returns>A string combining the left and right items with a join.</returns>
        private string combine(BuilderContext context, string leftHand, string rightHand)
        {
            StringBuilder result = new StringBuilder();
            result.Append(leftHand);
            if (context.Options.OneJoinItemPerLine)
            {
                result.AppendLine();
                if (context.Options.IndentJoinItems)
                {
                    result.Append(context.GetIndentationText());
                }
            }
            else
            {
                result.Append(' ');
            }
            result.Append(GetJoinName(context));
            result.Append(' ');
            result.Append(rightHand);
            return result.ToString();
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected abstract string GetJoinName(BuilderContext context);
    }
}
