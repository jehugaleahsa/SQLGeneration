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
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            _leftHand = leftHand;
            _on = new List<IFilter>();
            _rightHand = rightHand;
            _wrapInParentheses = true;
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
        /// Creates a new column under the table.
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

        string IJoinItem.GetDeclaration(IFilterGroup where)
        {
            StringBuilder result = new StringBuilder();
            if (_wrapInParentheses || !String.IsNullOrWhiteSpace(_alias))
            {
                result.Append("(");
            }
            string leftHand = _leftHand.GetDeclaration(where);
            string rightHand = _rightHand.GetDeclaration(where);
            result.Append(Combine(leftHand, rightHand));
            result.Append(" ON ");
            FilterGroup on = new FilterGroup();
            foreach (IFilter filter in _on)
            {
                on.AddFilter(filter);
            }
            result.Append(((IFilter)on).GetFilterText());
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

        string IJoinItem.GetReference()
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
        /// <param name="leftHand">The left item.</param>
        /// <param name="rightHand">The right item.</param>
        /// <returns>A string combining the left and right items with a join.</returns>
        protected abstract string Combine(string leftHand, string rightHand);
    }
}
