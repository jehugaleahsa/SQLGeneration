﻿using System;
using System.Collections.Generic;
using System.Linq;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a conditional statement.
    /// </summary>
    public class ConditionalCase : IProjectionItem, IFilterItem, IGroupByItem
    {
        private readonly List<Tuple<IFilter, IProjectionItem>> options;

        /// <summary>
        /// Initializes a new instance of a ConditionalCase.
        /// </summary>
        public ConditionalCase()
        {
            options = new List<Tuple<IFilter, IProjectionItem>>();
        }

        /// <summary>
        /// Gets the values that the item will be compared to.
        /// </summary>
        public IEnumerable<IFilter> Options
        {
            get { return options.Select(pair => pair.Item1); }
        }

        /// <summary>
        /// Adds the case option to the case expression.
        /// </summary>
        /// <param name="filter">The value that the case item must equal for given the result to be returned.</param>
        /// <param name="result">The value to return when the item equals the option.</param>
        public void AddCaseOption(IFilter filter, IProjectionItem result)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            options.Add(Tuple.Create(filter, result));
        }

        /// <summary>
        /// Removes the case option.
        /// </summary>
        /// <param name="filter">The value of the option to be removed.</param>
        /// <returns>True if a case with the given option was found; otherwise, false.</returns>
        public bool RemoveCaseOption(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            int index = options.FindIndex(pair => pair.Item1 == filter);
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
            foreach (Tuple<IFilter, IProjectionItem> pair in this.options)
            {
                IFilter option = pair.Item1;
                IProjectionItem result = pair.Item2;
                stream.Add(new TokenResult(SqlTokenRegistry.When, "WHEN"));
                stream.AddRange(option.GetFilterTokens(options));
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
    }
}