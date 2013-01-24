using System;

namespace SQLGeneration
{
    /// <summary>
    /// Contains the logic for generating leading whitespace.
    /// </summary>
    public class IndentationBuilder
    {
        private readonly FormatOptions options;
        private readonly int level;

        /// <summary>
        /// Initializes a new instance of an IndentationBuilder.
        /// </summary>
        /// <param name="options">The format options to use.</param>
        public IndentationBuilder(FormatOptions options)
        {
            this.options = options;
        }

        private IndentationBuilder(FormatOptions options, int level)
        {
            this.options = options;
            this.level = level;
        }

        /// <summary>
        /// Gets the next level of indentation.
        /// </summary>
        /// <returns>The next level of indentation.</returns>
        public IndentationBuilder Indent()
        {
            return new IndentationBuilder(options, level + 1);
        }

        /// <summary>
        /// Gets the textual representation of the indentation.
        /// </summary>
        /// <returns>The generated text.</returns>
        public string GetText()
        {
            return new String(' ', level * options.Indentation);
        }
    }
}
