using System;
using SQLGeneration.Expressions;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison between a value and null.
    /// </summary>
    public class NullFilter : Filter
    {
        private readonly IFilterItem _item;

        /// <summary>
        /// Initializes a new instance of a NullFilter.
        /// </summary>
        /// <param name="item">The item to check whether or not is null.</param>
        public NullFilter(IFilterItem item)
            : this(item, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of a NullFilter.
        /// </summary>
        /// <param name="item">The item to check whether or not is null.</param>
        /// <param name="isNull">Specifies whether to check if the column is null or not null.</param>
        public NullFilter(IFilterItem item, bool isNull)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _item = item;
            IsNull = isNull;
        }

        /// <summary>
        /// Gets the item being compared to null.
        /// </summary>
        public IFilterItem LeftHand
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets or sets whether to compare the item to null or not null.
        /// </summary>
        public bool IsNull
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="expression">The filter expression being built.</param>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override void GetInnerFilterExpression(Expression expression, CommandOptions options)
        {
            // "IS" [ "NOT" ] "NULL"
            expression.AddItem(_item.GetFilterExpression(options));
            expression.AddItem(new Token("IS"));
            if (!IsNull)
            {
                expression.AddItem(new Token("NOT"));
            }
            expression.AddItem(new Token("NULL"));
        }
    }
}
