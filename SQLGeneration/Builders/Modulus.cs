﻿using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents the remainder after division in T-SQL systems.
    /// </summary>
    public class Modulus : ArithmeticExpression
    {
        /// <summary>
        /// Initializes a new instance of a Modulus.
        /// </summary>
        /// <param name="leftHand">The left hand side of the expression.</param>
        /// <param name="rightHand">The right hand side of the expression.</param>
        public Modulus(IProjectionItem leftHand, IProjectionItem rightHand)
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
            return new TokenResult(SqlTokenRegistry.ModulusOperator, "%");
        }
    }
}
