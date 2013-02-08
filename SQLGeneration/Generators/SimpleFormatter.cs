using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLGeneration.Builders;
using SQLGeneration.Parsing;

namespace SQLGeneration.Generators
{
    /// <summary>
    /// Generates simple SQL text from a token source.
    /// </summary>
    public sealed class SimpleFormatter : SqlResponder
    {
        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="registry">The token registry to use.</param>
        public SimpleFormatter(SqlTokenRegistry registry)
            : base(new SqlGrammar(registry))
        {
        }

        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        public SimpleFormatter(SqlGrammar grammar = null)
            : base(grammar)
        {
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <returns>The command text.</returns>
        public string GetCommandText(ICommand command, CommandOptions options = null)
        {
            if (options == null)
            {
                options = new CommandOptions();
            }
            IEnumerable<string> tokenStream = command.GetCommandTokens(options);
            ITokenSource tokenSource = Grammar.TokenRegistry.CreateTokenSource(tokenStream);
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                MatchResult result = GetResult(tokenSource, writer);
                // TODO - generate text
            }
            return builder.ToString();
        }
    }
}
