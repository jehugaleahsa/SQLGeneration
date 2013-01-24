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
        protected internal FormatOptions()
        {
            SpacesToIndent = 4;
            AliasProjectionsUsingAs = true;
            WrapArithmeticExpressionsInParentheses = true;
            VerboseInnerJoin = true;
            VerboseOuterJoin = true;
        }

        /// <summary>
        /// Creates an exact copy of the format options.
        /// </summary>
        /// <returns>The copy.</returns>
        public FormatOptions Clone()
        {
            return (FormatOptions)MemberwiseClone();
        }

        /// <summary>
        /// Gets or sets the number of spaces to use when indenting.
        /// </summary>
        public int SpacesToIndent { get; set; }

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
        /// Gets or sets whether to indent join items when they are on a line by themselves.
        /// </summary>
        public bool IndentJoinItems { get; set; }

        /// <summary>
        /// Gets or sets whether to include the optional keyword AS when aliasing projection items.
        /// </summary>
        public bool AliasProjectionsUsingAs { get; set; }

        /// <summary>
        /// Gets or sets whether to include the optional keyword AS when aliasing join items.
        /// </summary>
        public bool AliasJoinItemsUsingAs { get; set; }

        /// <summary>
        /// Gets or sets whether to wrap filters in parentheses by default.
        /// </summary>
        public bool WrapFiltersInParentheses { get; set; }

        /// <summary>
        /// Gets or sets whether to wrap arithmetic expressions in parentheses by default.
        /// </summary>
        public bool WrapArithmeticExpressionsInParentheses { get; set; }

        /// <summary>
        /// Gets or sets whether to wrap joins in parentheses by default.
        /// </summary>
        public bool WrapJoinsInParentheses { get; set; }

        /// <summary>
        /// Gets or sets whether inner joins should specify INNER explicitly.
        /// </summary>
        public bool VerboseInnerJoin { get; set; }

        /// <summary>
        /// Gets or sets whether outer joins should specify OUTER explicitly.
        /// </summary>
        public bool VerboseOuterJoin { get; set; }

        /// <summary>
        /// Gets or sets whether each column appears on its own line in an insert statement column list.
        /// </summary>
        public bool OneInsertColumnPerLine { get; set; }

        /// <summary>
        /// Gets or sets whether to indent insert column when they appear on a line by themselves.
        /// </summary>
        public bool IndentInsertColumns { get; set; }

        /// <summary>
        /// Gets or sets whether each value list item should appear on a line of its own.
        /// </summary>
        public bool OneValueListItemPerLine { get; set; }

        /// <summary>
        /// Gets or sets whether to indent value list items when they appear on a line by themselves.
        /// </summary>
        public bool IndentValueListItems { get; set; }

        /// <summary>
        /// Gets or sets whether columns should be fully qualified within an insert statement.
        /// </summary>
        public bool QualifyInsertColumns { get; set; }

        /// <summary>
        /// Gets or sets whether columns should be fully qualified within an update statement.
        /// </summary>
        public bool QualifyUpdateColumn { get; set; }

        /// <summary>
        /// Gets or sets whether columns should be fully qualified within a delete statement.
        /// </summary>
        public bool QualifyDeleteColumns { get; set; }

        /// <summary>
        /// Gets or sets whether each setter in an UPDATE statement should appear on a line by itself.
        /// </summary>
        public bool OneSetterPerLine { get; set; }

        /// <summary>
        /// Gets or sets whether to indent UPDATE setters when they appear on a line by themselves.
        /// </summary>
        public bool IndentSetters { get; set; }

        /// <summary>
        /// Gets or sets whether each filter should appear on a line by itself.
        /// </summary>
        public bool OneFilterPerLine { get; set; }

        /// <summary>
        /// Gets or sets whether to indent filters when they appear on a line by themselves.
        /// </summary>
        public bool IndentFilters { get; set; }
    }
}
