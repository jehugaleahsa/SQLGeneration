using System;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Describes a window frame that is limited to a specific number of rows in one direction.
    /// </summary>
    public class BoundFrame : IPrecedingFrame, IFollowingFrame
    {
        /// <summary>
        /// Initializes a new instance of a BoundFrame.
        /// </summary>
        /// <param name="rowCount">The limit to the number of rows to include in the frame.</param>
        public BoundFrame(int rowCount)
        {
            if (rowCount < 0)
            {
                throw new ArgumentOutOfRangeException("rowCount", rowCount, Resources.NegativeRowCount);
            }
            RowCount = rowCount;
        }

        /// <summary>
        /// Gets the number of rows to include in the frame in one direction.
        /// </summary>
        public int RowCount
        {
            get;
            private set;
        }

        TokenStream IPrecedingFrame.GetFrameTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            getTokens(stream, options);
            stream.Add(new TokenResult(SqlTokenRegistry.Preceding, "PRECEDING"));
            return stream;
        }

        TokenStream IFollowingFrame.GetFrameTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            getTokens(stream, options);
            stream.Add(new TokenResult(SqlTokenRegistry.Following, "FOLLOWING"));
            return stream;
        }

        private void getTokens(TokenStream stream, CommandOptions options)
        {
            IProjectionItem literal = new NumericLiteral(RowCount);
            stream.AddRange(literal.GetProjectionTokens(options));
        }
    }
}
