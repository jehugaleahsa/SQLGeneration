using System;
using System.Collections.Generic;
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

        IEnumerable<string> IPrecedingFrame.GetFrameTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("UNBOUNDED");
            stream.Add("PRECEDING");
            return stream;
        }

        IEnumerable<string> IFollowingFrame.GetFrameTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("UNBOUNDED");
            stream.Add("FOLLOWING");
            return stream;
        }
    }
}
