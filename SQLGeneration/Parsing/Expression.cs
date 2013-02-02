using System;
using System.Collections.Generic;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Represents a sub-expression made up of tokens and sub-expressions.
    /// </summary>
    public sealed class Expression : IExpressionItem
    {
        private readonly ExpressionDefinition expression;

        /// <summary>
        /// Initializes a new instance of an Expression.
        /// </summary>
        /// <param name="expression">The sequence of tokens and sub-expressions expected to appear.</param>
        internal Expression(ExpressionDefinition expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Attempts to match the expression item with the values returned by the parser.
        /// </summary>
        /// <param name="parser">The parser currently iterating over the token source.</param>
        /// <param name="depth"></param>
        /// <returns>The results of the match.</returns>
        public MatchResult Match(Parser parser, int depth)
        {
            bool isMatch = true;
            Dictionary<string, MatchResult> innerResults = new Dictionary<string, MatchResult>();
            foreach (ExpressionItem detail in expression.Items)
            {
                parser.StartTransaction();
                Console.Out.WriteLine("Attempting{0}{1}...", new String(' ', depth + 1), detail.ItemName);
                MatchResult innerResult = detail.Item.Match(parser, depth + 1);
                innerResults.Add(detail.ItemName, innerResult);
                if (innerResult.IsMatch)
                {
                    Console.Out.WriteLine("Success{0}{1}...", new String(' ', depth + 1), detail.ItemName);
                    parser.Commit();
                }
                else
                {
                    Console.Out.WriteLine("Failed{0}{1}...", new String(' ', depth + 1), detail.ItemName);
                    parser.Rollback();
                    if (detail.IsRequired)
                    {
                        isMatch = false;
                        break;
                    }
                }
            }
            MatchResult result = new MatchResult(isMatch);
            foreach (string itemName in innerResults.Keys)
            {
                result.Matches.Add(itemName, innerResults[itemName]);
            }
            return result;
        }
    }
}
