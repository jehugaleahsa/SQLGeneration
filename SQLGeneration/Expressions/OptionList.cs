using System;
using System.Collections.Generic;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Holds the legal sub-expressions that can appear within an expression.
    /// </summary>
    public class OptionList
    {
        private readonly ExpressionRegistry registry;
        private readonly List<ExpressionDefinition> definitions;

        /// <summary>
        /// Initializes a new instance of an OptionList.
        /// </summary>
        /// <param name="registry">The registry that the parent expression was registered with.</param>
        internal OptionList(ExpressionRegistry registry)
        {
            this.registry = registry;
            this.definitions = new List<ExpressionDefinition>();
        }

        /// <summary>
        /// Defines a new sub-expression for the expression.
        /// </summary>
        /// <returns>The new sub-expression configuration object.</returns>
        public ExpressionDefinition Define()
        {
            ExpressionDefinition definition = new ExpressionDefinition(registry);
            definitions.Add(definition);
            return definition;
        }

        /// <summary>
        /// Gets the optional definitions registered with the list.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ExpressionDefinition> GetOptions()
        {
            return definitions;
        }

        /// <summary>
        /// Gets the number of optional definitions.
        /// </summary>
        internal int Count 
        {
            get { return definitions.Count; }
        }
    }
}
