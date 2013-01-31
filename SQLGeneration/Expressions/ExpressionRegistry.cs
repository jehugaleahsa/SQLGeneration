using System;
using System.Collections.Generic;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Registers expression definitions.
    /// </summary>
    public class ExpressionRegistry
    {
        private readonly Dictionary<ExpressionItemType, ExpressionDefinition> definitionLookup;

        /// <summary>
        /// Initializes a new instance of an ExpressionRegistry.
        /// </summary>
        public ExpressionRegistry()
        {
            this.definitionLookup = new Dictionary<ExpressionItemType, ExpressionDefinition>();
        }

        /// <summary>
        /// Defines the syntax for an expression of the given type.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        /// <returns>The expression definition configuration object.</returns>
        public ExpressionDefinition Define(ExpressionItemType type)
        {
            ExpressionDefinition definition = new ExpressionDefinition(this);
            definitionLookup[type] = definition;
            return definition;
        }

        /// <summary>
        /// Finds the definition registered for the given type.
        /// </summary>
        /// <param name="type">The expression item type.</param>
        /// <returns>The definition registered for the given type.</returns>
        public ExpressionDefinition Find(ExpressionItemType type)
        {
            ExpressionDefinition definition;
            if (!definitionLookup.TryGetValue(type, out definition))
            {
                // TODO - make this a more meaningful exception
                throw new Exception();
            }
            return definition;
        }
    }
}
