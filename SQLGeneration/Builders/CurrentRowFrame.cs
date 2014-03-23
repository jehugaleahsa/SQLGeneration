using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Describes a window frame that is limited to the current row in one direction.
    /// </summary>
    public class CurrentRowFrame : IPrecedingFrame, IFollowingFrame
    {
        /// <summary>
        /// Initializes a new instance of a CurrentRowFrame.
        /// </summary>
        public CurrentRowFrame()
        {
        }

        TokenStream IPrecedingFrame.GetFrameTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        TokenStream IFollowingFrame.GetFrameTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        private TokenStream getTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.CurrentRow, "CURRENT ROW"));
            return stream;
        }

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            visitor.VisitCurrentRowFrame(this);
        }
    }
}
