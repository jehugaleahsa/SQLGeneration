using System;

namespace SQLGeneration
{
    /// <summary>
    /// Holds information used when building commands.
    /// </summary>
    public class BuilderContext
    {
        private readonly FormatOptions options;
        private readonly IndentationBuilder indentation;

        /// <summary>
        /// Initializes a new instance of a BuilderContext.
        /// </summary>
        public BuilderContext()
        {
            options = new FormatOptions();
            indentation = new IndentationBuilder(options);
        }

        private BuilderContext(FormatOptions options, IndentationBuilder indentation)
        {
            this.options = options;
            this.indentation = indentation;
        }

        /// <summary>
        /// Gets the configuration settings for building commands.
        /// </summary>
        public FormatOptions Options
        {
            get { return options; }
        }

        /// <summary>
        /// Creates a new BuilderContext indented to the next level.
        /// </summary>
        /// <returns>A BuilderContext indented to the next level.</returns>
        public BuilderContext Indent()
        {
            return new BuilderContext(options, indentation.Indent());
        }

        /// <summary>
        /// Gets the leading indentation for a line.
        /// </summary>
        /// <returns>The leading indentation.</returns>
        public string GetIndentationText()
        {
            return indentation.GetText();
        }
    }
}
