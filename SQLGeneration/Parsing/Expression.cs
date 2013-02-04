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
        /// <param name="itemName">The name of the item in the outer expression.</param>
        /// <returns>The results of the match.</returns>
        public MatchResult Match(Parser parser, string itemName)
        {
            bool isMatch = true;
            MatchResult result = new MatchResult(isMatch);
            result.ItemName = itemName;
            foreach (ExpressionItem detail in expression.Items)
            {
                parser.StartTransaction();
                MatchResult innerResult = detail.Item.Match(parser, detail.ItemName);
                result.Matches.Add(innerResult);
                if (innerResult.IsMatch)
                {
                    parser.Commit();
                }
                else
                {
                    parser.Rollback();
                    if (detail.IsRequired)
                    {
                        isMatch = false;
                        break;
                    }
                }
            }
            parser.RunHandler(expression.ExpressionType, result);
            return result;
        }
    }
}
