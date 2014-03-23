using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Specifies where a windowed function's frame should start.
    /// </summary>
    public interface IPrecedingFrame : IVisitableBuilder
    {
        /// <summary>
        /// Gets the tokens for specifying a window frame.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating tokens.</param>
        /// <returns>The tokens making up the window frame.</returns>
        TokenStream GetFrameTokens(CommandOptions options);
    }
}
