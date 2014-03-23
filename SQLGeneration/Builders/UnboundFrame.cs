using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Describes a window frame that is unbounded in one direction.
    /// </summary>
    public class UnboundFrame : IPrecedingFrame, IFollowingFrame
    {
        /// <summary>
        /// Initializes a new instance of an UnboundFrame.
        /// </summary>
        public UnboundFrame()
        {
        }

        TokenStream IPrecedingFrame.GetFrameTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Unbounded, "UNBOUNDED"));
            stream.Add(new TokenResult(SqlTokenRegistry.Preceding, "PRECEDING"));
            return stream;
        }

        TokenStream IFollowingFrame.GetFrameTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Unbounded, "UNBOUNDED"));
            stream.Add(new TokenResult(SqlTokenRegistry.Following, "FOLLOWING"));
            return stream;
        }

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            visitor.VisitUnboundFrame(this);
        }
    }
}
