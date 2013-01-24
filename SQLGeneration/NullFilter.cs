using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison between a value and null.
    /// </summary>
    public class NullFilter : Filter, INullFilter
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
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override string GetFilterText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder(_item.GetFilterItemText(context));
            result.Append(" IS");
            if (!IsNull)
            {
                result.Append(" NOT");
            }
            result.Append(" NULL");
            return result.ToString();
        }
    }
}
