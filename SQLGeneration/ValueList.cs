using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a list of values that can appear in an 'in' comparison.
    /// </summary>
    public class ValueList : IValueProvider
    {
        private readonly List<IProjectionItem> _values;

        /// <summary>
        /// Initializes a new instance of a InList.
        /// </summary>
        public ValueList()
        {
            _values = new List<IProjectionItem>();
        }

        /// <summary>
        /// Initializes a new instance of a InList.
        /// </summary>
        /// <param name="values">The values to add to the list.</param>
        public ValueList(params IProjectionItem[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (values.Contains(null))
            {
                throw new ArgumentNullException("values");
            }
            _values = new List<IProjectionItem>(values);
        }

        /// <summary>
        /// Gets the values being provided.
        /// </summary>
        public IEnumerable<IProjectionItem> Values
        {
            get { return new ReadOnlyCollection<IProjectionItem>(_values); }
        }

        /// <summary>
        /// Adds the given projection item to the values list.
        /// </summary>
        /// <param name="item">The value to add.</param>
        public void AddValue(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _values.Add(item);
        }

        /// <summary>
        /// Adds the given projection item from the values list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveValue(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _values.Remove(item);
        }

        /// <summary>
        /// Gets the value list as it would appear within a filter expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expression representing the value list.</returns>
        public IExpressionItem GetFilterExpression(CommandOptions options)
        {
            // "(" <Projection> [ "," <ValueList> ] ")"
            Expression expression = new Expression();
            expression.AddItem(new Token("("));
            if (_values.Count > 0)
            {
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                IEnumerable<IExpressionItem> values = _values.Select(value => formatter.GetUnaliasedReference(value));
                expression.AddItem(Expression.Join(new Token(","), values));
            }
            expression.AddItem(new Token(")"));
            return expression;
        }

        private IExpressionItem buildValueList(CommandOptions options, int valueIndex)
        {
            if (valueIndex == _values.Count - 1)
            {
                IProjectionItem current = _values[valueIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                return formatter.GetUnaliasedReference(current);
            }
            else
            {
                IExpressionItem right = buildValueList(options, valueIndex + 1);
                IProjectionItem current = _values[valueIndex];
                ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
                IExpressionItem left = formatter.GetUnaliasedReference(current);
                Expression expression = new Expression();
                expression.AddItem(left);
                expression.AddItem(new Token(","));
                expression.AddItem(right);
                return expression;
            }
        }

        bool IValueProvider.IsQuery
        {
            get { return false; }
        }
    }
}
