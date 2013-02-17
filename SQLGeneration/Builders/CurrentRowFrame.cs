using System;
using System.Collections.Generic;
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

        IEnumerable<string> IPrecedingFrame.GetFrameTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        IEnumerable<string> IFollowingFrame.GetFrameTokens(CommandOptions options)
        {
            return getTokens(options);
        }

        private IEnumerable<string> getTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add("CURRENT ROW");
            return stream;
        }
    }
}
