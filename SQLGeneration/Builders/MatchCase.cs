using System;
using System.Collections.Generic;
using System.Linq;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a conditional statement where an item is matched with another.
    /// </summary>
    public class MatchCase : IProjectionItem, IFilterItem, IGroupByItem
    {
        private readonly List<Tuple<IProjectionItem, IProjectionItem>> options;

        /// <summary>
        /// Initializes a new instance of a MatchCase.
        /// </summary>
        /// <param name="item">The item that will be matched to the different values.</param>
        public MatchCase(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            Item = item;
            options = new List<Tuple<IProjectionItem, IProjectionItem>>();
        }

        /// <summary>
        /// Gets the item that will be compared to the different values.
        /// </summary>
        public IProjectionItem Item 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the values that the item will be compared to.
        /// </summary>
        public IEnumerable<IProjectionItem> Options
        {
            get { return options.Select(pair => pair.Item1); }
        }

        /// <summary>
        /// Adds the case option to the case expression.
        /// </summary>
        /// <param name="option">The value that the case item must equal for given the result to be returned.</param>
        /// <param name="result">The value to return when the item equals the option.</param>
        public void AddCaseOption(IProjectionItem option, IProjectionItem result)
        {
            if (option == null)
            {
                throw new ArgumentNullException("option");
            }
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            options.Add(Tuple.Create(option, result));
        }

        /// <summary>
        /// Removes the case option.
        /// </summary>
        /// <param name="option">The value of the option to be removed.</param>
        /// <returns>True if a case with the given option was found; otherwise, false.</returns>
        public bool RemoveCaseOption(IProjectionItem option)
        {
            if (option == null)
            {
                throw new ArgumentNullException("option");
            }
            int index = options.FindIndex(pair => pair.Item1 == option);
            if (index != -1)
            {
                options.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the default value to return if no options match the item.
        /// </summary>
        public IProjectionItem Default
        {
            get;
            set;
        }

        TokenStream IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        TokenStream IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        TokenStream IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        private TokenStream getTokens(CommandOptions options)
        {
            if (this.options.Count == 0)
            {
                throw new SQLGenerationException(Resources.EmptyCaseExpression);
            }
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Case, "CASE"));
            stream.AddRange(Item.GetProjectionTokens(options));
            foreach (Tuple<IProjectionItem, IProjectionItem> pair in this.options)
            {
                IProjectionItem option = pair.Item1;
                IProjectionItem result = pair.Item2;
                stream.Add(new TokenResult(SqlTokenRegistry.When, "WHEN"));
                stream.AddRange(option.GetProjectionTokens(options));
                stream.Add(new TokenResult(SqlTokenRegistry.Then, "THEN"));
                stream.AddRange(result.GetProjectionTokens(options));
            }
            if (Default != null)
            {
                stream.Add(new TokenResult(SqlTokenRegistry.Else, "ELSE"));
                stream.AddRange(Default.GetProjectionTokens(options));
            }
            stream.Add(new TokenResult(SqlTokenRegistry.End, "END"));
            return stream;
        }

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            visitor.VisitMatchCase(this);
        }
    }
}
