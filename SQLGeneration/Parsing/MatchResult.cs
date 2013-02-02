using System;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Holds the result of trying to match an expression item.
    /// </summary>
    public sealed class MatchResult
    {
        private readonly MatchResultCollection matches;

        /// <summary>
        /// Initializes a new instance of a MatchResult.
        /// </summary>
        /// <param name="isMatch">Specifies whether the match was successful or not.</param>
        internal MatchResult(bool isMatch)
        {
            IsMatch = isMatch;
            matches = new MatchResultCollection();
        }

        /// <summary>
        /// Gets whether the expression item was matched against the parser.
        /// </summary>
        public bool IsMatch
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the match result for sub-expressions or tokens within the current expression.
        /// This collection will be empty for tokens.
        /// </summary>
        public MatchResultCollection Matches
        {
            get { return matches; }
        }

        /// <summary>
        /// Gets the data that is meant to be available to the outer expression.
        /// </summary>
        public object Context
        {
            get;
            internal set;
        }
    }
}
