using System;

namespace SQLGeneration
{
    /// <summary>
    /// Holds information used when building commands.
    /// </summary>
    public class BuilderContext
    {
        private readonly FormatOptions options;
        private readonly int indentionLevel;

        /// <summary>
        /// Initializes a new instance of a BuilderContext.
        /// </summary>
        public BuilderContext()
        {
            options = new FormatOptions();
            indentionLevel = 0;
        }

        private BuilderContext(FormatOptions options, int indentationLevel)
        {
            this.options = options;
            this.indentionLevel = indentationLevel;
        }

        /// <summary>
        /// Gets whether the context is within a SELECT SQL.
        /// </summary>
        public bool IsSelect
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether the context is within an INSERT SQL.
        /// </summary>
        public bool IsInsert
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether the context is within an UPDATE SQL.
        /// </summary>
        public bool IsUpdate
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether the context is within a DELETE SQL.
        /// </summary>
        public bool IsDelete
        {
            get;
            internal set;
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
            return new BuilderContext(options, indentionLevel + 1);
        }

        /// <summary>
        /// Gets the leading indentation for a line.
        /// </summary>
        /// <returns>The leading indentation.</returns>
        public string GetIndentationText()
        {
            return new String(' ', indentionLevel * options.SpacesToIndent);
        }

        /// <summary>
        /// Creates an exact copy of the context and its settings.
        /// </summary>
        /// <returns>The copy.</returns>
        public BuilderContext Clone()
        {
            BuilderContext clone = new BuilderContext(options.Clone(), indentionLevel);
            clone.IsSelect = IsSelect;
            clone.IsInsert = IsInsert;
            clone.IsUpdate = IsUpdate;
            clone.IsDelete = IsDelete;
            return clone;
        }
    }
}
