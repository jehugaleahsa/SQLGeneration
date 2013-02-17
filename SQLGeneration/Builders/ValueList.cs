using System;
using System.Collections.Generic;
using System.Linq;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
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
            get { return _values; }
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

        TokenStream IFilterItem.GetFilterTokens(CommandOptions options)
        {
            using (IEnumerator<IProjectionItem> enumerator = _values.GetEnumerator())
            {
                TokenStream stream = new TokenStream();
                stream.Add(new TokenResult(SqlTokenRegistry.LeftParenthesis, "("));
                if (enumerator.MoveNext())
                {
                    stream.AddRange(enumerator.Current.GetProjectionTokens(options));
                    while (enumerator.MoveNext())
                    {
                        stream.Add(new TokenResult(SqlTokenRegistry.Comma, ","));
                        stream.AddRange(enumerator.Current.GetProjectionTokens(options));
                    }
                }
                stream.Add(new TokenResult(SqlTokenRegistry.RightParenthesis, ")"));
                return stream;
            }
        }

        bool IValueProvider.IsValueList
        {
            get { return true; }
        }
    }
}
