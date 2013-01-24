using System;

namespace SQLGeneration
{
    /// <summary>
    /// Holds configuration settings for formatting the generated SQL.
    /// </summary>
    public class FormatOptions
    {
        /// <summary>
        /// Initializes a new instance of a FormatOptions,
        /// setting the default configurations.
        /// </summary>
        public FormatOptions()
        {
        }

        /// <summary>
        /// Gets or sets the number of spaces to use when indenting.
        /// </summary>
        public int Indentation { get; set; }

        /// <summary>
        /// Gets or sets whether each clause should appear at the start of a new line.
        /// </summary>
        public bool OneClausePerLine { get; set; }

        /// <summary>
        /// Gets or sets whether each project item will appear on a line by itself.
        /// </summary>
        public bool OneProjectionPerLine { get; set; }

        /// <summary>
        /// Gets or sets whether to indent projections when they are on a line by themselves.
        /// </summary>
        public bool IndentProjections { get; set; }

        /// <summary>
        /// Gets or sets whether each join item should appear on its own line.
        /// </summary>
        public bool OneJoinItemPerLine { get; set; }
        
        /// <summary>
        /// Gets or sets whether to indent projections when they are on a line by themselves.
        /// </summary>
        public bool IndentJoinItems { get; set; }
    }
}
