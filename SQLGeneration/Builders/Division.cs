using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the division of two items in a command.
    /// </summary>
    public class Division : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a Division.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public Division(IProjectionItem leftHand, IProjectionItem rightHand)
            : base(leftHand, rightHand)
        {
        }

        /// <summary>
        /// Gets the token representing the arithmetic operator.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The token representing the arithmetic operator.</returns>
        protected override TokenResult GetOperator(CommandOptions options)
        {
            return new TokenResult(SqlTokenRegistry.DivisionOperator, "/");
        }
    }
}
