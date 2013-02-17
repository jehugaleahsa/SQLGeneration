using System;
using System.Collections.Generic;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Specifies where a windowed function's frame should stop.
    /// </summary>
    public interface IFollowingFrame
    {
        /// <summary>
        /// Gets the tokens for specifying a window frame.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating tokens.</param>
        /// <returns>The tokens making up the window frame.</returns>
        IEnumerable<string> GetFrameTokens(CommandOptions options);
    }
}
