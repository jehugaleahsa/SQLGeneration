using System;
using System.Collections.Generic;
using SQLGeneration.Expressions;

namespace SQLGeneration.Formatters
{
    /// <summary>
    /// Formats the command expressions given to it by generating the minimal amount of text.
    /// </summary>
    public class MinimizingFormatter
    {
        /// <summary>
        /// Initializes a new instance of a MinimizingFormatter.
        /// </summary>
        public MinimizingFormatter()
        {
        }

        /// <summary>
        /// Gets the command text for the given command using the given options. If
        /// the options are null, the default options will be used.
        /// </summary>
        /// <param name="command">The command to format.</param>
        /// <param name="options">The configuration settings for the command builder.</param>
        /// <returns>The formatted command text.</returns>
        public string GetCommandText(ICommand command, CommandOptions options = null)
        {
            if (options == null)
            {
                options = new CommandOptions();
            }
            IExpressionItem commandExpression = command.GetCommandExpression(options);
            TokenCollector collector = new TokenCollector();
            commandExpression.Visit(collector.Collect);
            return collector.GetCommandText();
        }

        private class TokenCollector
        {
            private readonly List<string> tokens;

            public TokenCollector()
            {
                this.tokens = new List<string>();
            }

            public void Collect(Token token)
            {
                tokens.Add(token.Value);
            }

            public string GetCommandText()
            {
                return String.Join(" ", tokens);
            }
        }
    }
}
