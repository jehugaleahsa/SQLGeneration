using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Defines a sequence of tokens and expressions making up part of a larger expression.
    /// </summary>
    public class ExpressionDefinition
    {
        private readonly ExpressionRegistry registry;
        private readonly List<Func<Parser, InspectionResult>> inspectors;

        /// <summary>
        /// Initializes a new instance of an ExpressionDefinition.
        /// </summary>
        /// <param name="registry">The registry that the definition is found in.</param>
        internal ExpressionDefinition(ExpressionRegistry registry)
        {
            this.registry = registry;
            this.inspectors = new List<Func<Parser, InspectionResult>>();
        }

        /// <summary>
        /// Requires that a token with given value appears next in the expression.
        /// </summary>
        /// <param name="type">The type of the expected token.</param>
        /// <param name="token">The expected token.</param>
        /// <returns>The udpated definition.</returns>
        public ExpressionDefinition Require(TokenType type, string token = null)
        {
            Func<Parser, InspectionResult> inspector = parser =>
            {
                Parser nextParser = parser.GetNextToken(type, token);
                if (nextParser == null)
                {
                    return new InspectionResult() { IsMatch = false };
                }
                else
                {
                    ExpressionDefinition definition = registry.Find(nextParser.ExpressionItem.Type);
                    return new InspectionResult() 
                    {
                        IsMatch = definition.IsMatch(nextParser), 
                        Parser = nextParser,
                    };
                }
            };
            inspectors.Add(inspector);
            return this;
        }

        /// <summary>
        /// Requires that an expression item of the given type appears next in the expression.
        /// </summary>
        /// <param name="types">The type of types of the expected sub-expressions.</param>
        /// <returns>The updated definition.</returns>
        public ExpressionDefinition Require(params ExpressionItemType[] types)
        {
            Func<Parser, InspectionResult> inspector = parser =>
            {
                Parser nextParser = parser.GetNextParser(types);
                if (nextParser == null)
                {
                    return new InspectionResult() { IsMatch = false };
                }
                else
                {
                    ExpressionDefinition definition = registry.Find(nextParser.ExpressionItem.Type);
                    return new InspectionResult()
                    {
                        IsMatch = definition.IsMatch(nextParser),
                        Parser = nextParser
                    };
                }
            };
            inspectors.Add(inspector);
            return this;
        }

        /// <summary>
        /// Indicates that one of the defined sub-expressions must be found by the parser.
        /// </summary>
        /// <param name="optionsGetter">A function that returns which sub-expressions may be found.</param>
        /// <returns>The updated definition.</returns>
        public ExpressionDefinition RequireAny(Action<OptionList> optionsGetter)
        {
            OptionList options = new OptionList(registry);
            optionsGetter(options);
            Func<Parser, InspectionResult> inspector = parser =>
            {
                foreach (ExpressionDefinition option in options.GetOptions())
                {
                    if (option.IsMatch(parser))
                    {
                        return new InspectionResult() { IsMatch = true, Parser = parser };
                    }
                }
                return new InspectionResult() { IsMatch = false, Parser = parser };
            };
            inspectors.Add(inspector);
            return this;
        }

        /// <summary>
        /// Requires that a token with given value appears next in the expression.
        /// </summary>
        /// <param name="type">The type of the expected token.</param>
        /// <param name="token">The expected token.</param>
        /// <returns>The udpated definition.</returns>
        public ExpressionDefinition Optional(TokenType type, string token = null)
        {
            Func<Parser, InspectionResult> inspector = parser =>
            {
                Parser nextParser = parser.GetNextToken(type, token);
                if (nextParser == null)
                {
                    return new InspectionResult() { IsMatch = true };
                }
                else
                {
                    ExpressionDefinition definition = registry.Find(nextParser.ExpressionItem.Type);
                    return new InspectionResult()
                    {
                        IsMatch = definition.IsMatch(nextParser),
                        Parser = nextParser,
                    };
                }
            };
            inspectors.Add(inspector);
            return this;
        }

        /// <summary>
        /// Requires that an expression item of the given type appears next in the expression.
        /// </summary>
        /// <param name="types">The type of types of the expected sub-expressions.</param>
        /// <returns>The updated definition.</returns>
        public ExpressionDefinition Optional(params ExpressionItemType[] types)
        {
            Func<Parser, InspectionResult> inspector = parser =>
            {
                Parser nextParser = parser.GetNextParser(types);
                if (nextParser == null)
                {
                    return new InspectionResult() { IsMatch = true };
                }
                else
                {
                    ExpressionDefinition definition = registry.Find(nextParser.ExpressionItem.Type);
                    return new InspectionResult()
                    {
                        IsMatch = definition.IsMatch(nextParser),
                        Parser = nextParser
                    };
                }
            };
            inspectors.Add(inspector);
            return this;
        }

        /// <summary>
        /// Indicates that one of the defined sub-expressions may be found by
        /// the parser.
        /// </summary>
        /// <param name="optionsGetter">A function that returns which sub-expressions may be found.</param>
        /// <returns>The updated definition.</returns>
        public ExpressionDefinition OptionalAny(Action<OptionList> optionsGetter)
        {
            OptionList options = new OptionList(registry);
            optionsGetter(options);
            Func<Parser, InspectionResult> inspector = parser =>
            {
                foreach (ExpressionDefinition option in options.GetOptions())
                {
                    if (option.IsMatch(parser))
                    {
                        return new InspectionResult() { IsMatch = true, Parser = parser };
                    }
                }
                return new InspectionResult() { IsMatch = true };
            };
            inspectors.Add(inspector);
            return this;
        }

        /// <summary>
        /// Gets whether the current expression can be parsed.
        /// </summary>
        /// <param name="parser">The parser over the current expression.</param>
        /// <returns>True if the current expression could be parsed; otherwise, false.</returns>
        public bool IsMatch(Parser parser)
        {
            List<Parser> matches = new List<Parser>();
            foreach (Func<Parser, InspectionResult> inspector in inspectors)
            {
                InspectionResult result = inspector(parser);
                if (result.IsMatch)
                {
                    if (result.Parser != null)
                    {
                        matches.Add(result.Parser);
                    }
                }
                else
                {
                    parser.PutBack(matches.Where(match => match != null).Select(match => match.ExpressionItem));
                    return false;
                }
            }
            return true;
        }

        private class InspectionResult
        {
            public bool IsMatch { get; set; }

            public Parser Parser { get; set; }
        }
    }
}
