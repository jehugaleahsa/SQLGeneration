using System;
using SQLGeneration.Parsing;
using SQLGeneration.Builders;

namespace SQLGeneration.Generators
{
    /// <summary>
    /// Builds an ICommand from a SQL statement.
    /// </summary>
    public sealed class CommandBuilder : SqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="registry">The token registry to use.</param>
        public CommandBuilder(SqlTokenRegistry registry)
            : base(new SqlGrammar(registry))
        {
        }

        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        public CommandBuilder(SqlGrammar grammar = null)
            : base(grammar)
        {
        }

        /// <summary>
        /// Parses the given command text to build a command builder.
        /// </summary>
        /// <param name="commandText">The command text to parse.</param>
        /// <returns>The command that was parsed.</returns>
        public ICommand GetCommand(string commandText)
        {
            ITokenSource tokenSource = Grammar.TokenRegistry.CreateTokenSource(commandText);
            MatchResult result = GetResult(tokenSource);
            return buildStart(result);
        }

        private ICommand buildStart(MatchResult result)
        {
            throw new NotImplementedException();
        }
    }
}
