using System;

namespace SQLGeneration
{
    /// <summary>
    /// Holds configuration setting for building commands.
    /// </summary>
    public class CommandOptions
    {
        /// <summary>
        /// Initializes a new instance of a CommandOptions with the default configurations.
        /// </summary>
        public CommandOptions()
        {
            AliasProjectionsUsingAs = true;
            WrapArithmeticExpressionsInParentheses = true;
            VerboseInnerJoin = true;
            VerboseOuterJoin = true;
        }

        /// <summary>
        /// Creates a copy of the current options.
        /// </summary>
        /// <returns>The copy.</returns>
        public CommandOptions Clone()
        {
            return (CommandOptions)MemberwiseClone();
        }

        /// <summary>
        /// Gets or sets whether a SELECT command is being built.
        /// </summary>
        internal protected bool IsSelect { get; set; }

        /// <summary>
        /// Gets or sets whether an INSERT command is being built.
        /// </summary>
        internal protected bool IsInsert { get; set; }

        /// <summary>
        /// Gets or sets whether an UPDATE command is being built.
        /// </summary>
        internal protected bool IsUpdate { get; set; }

        /// <summary>
        /// Gets or sets whether a DELETE command is being built.
        /// </summary>
        internal protected bool IsDelete { get; set; }

        /// <summary>
        /// Gets or sets whether to include the optional keyword AS when aliasing projection items.
        /// </summary>
        public bool AliasProjectionsUsingAs { get; set; }

        /// <summary>
        /// Gets or sets whether to include the optional keyword AS when aliasing join items.
        /// </summary>
        public bool AliasColumnSourcesUsingAs { get; set; }

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
    }
}
