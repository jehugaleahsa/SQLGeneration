using System;
using System.Collections.Generic;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Represents a list of possible expressions or tokens that the parser should try.
    /// </summary>
    public sealed class Options : IExpressionItem
    {
        private readonly List<ExpressionItem> options;

        /// <summary>
        /// Initializes a new instance of an Options.
        /// </summary>
        internal Options()
        {
            options = new List<ExpressionItem>();
        }

        /// <summary>
        /// Indicates that the given item is the next expected, giving it a
        /// name and specifying whether it is required.
        /// </summary>
        /// <param name="itemName">The name that the token will be identified with in the outer expression.</param>
        /// <param name="item">The expression item to add to the sequence.</param>
        /// <returns>The updated expression.</returns>
        public Options Add(string itemName, IExpressionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            options.Add(new ExpressionItem(itemName, false, item));
            return this;
        }

        /// <summary>
        /// Indicates that the given sub-expression is the next expected, giving it a
        /// name and specifying whether it is required.
        /// </summary>
        /// <param name="itemName">The name that the token will be identified with in the outer expression.</param>
        /// <param name="definition">The definition for the sub-expression.</param>
        /// <returns>The updated expression.</returns>
        public Options Add(string itemName, ExpressionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }
            IExpressionItem item = new Expression(definition);
            options.Add(new ExpressionItem(itemName, false, item));
            return this;
        }

        /// <summary>
        /// Attempts to match the expression item with the values returned by the parser.
        /// </summary>
        /// <param name="parser">The parser currently iterating over the token source.</param>
        /// <param name="depth"></param>
        /// <returns>The results of the match.</returns>
        public MatchResult Match(Parser parser, int depth)
        {
            foreach (ExpressionItem option in options)
            {
                Console.Out.WriteLine("Attempting{0}{1}... ", new String(' ', depth + 1), option.ItemName);
                parser.StartTransaction();
                MatchResult innerResult = option.Item.Match(parser, depth + 1);
                if (innerResult.IsMatch)
                {
                    parser.Commit();
                    MatchResult result = new MatchResult(true);
                    result.Matches.Add(option.ItemName, innerResult);
                    Console.Out.WriteLine("Success{0}{1}... ", new String(' ', depth + 1), option.ItemName);
                    return result;
                }
                Console.Out.WriteLine("Failure{0}{1}... ", new String(' ', depth + 1), option.ItemName);
                parser.Rollback();
            }
            return new MatchResult(false);
        }
    }
}
