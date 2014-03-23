using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Defines the limits of a function window.
    /// </summary>
    public abstract class WindowFrame : IVisitableBuilder
    {
        /// <summary>
        /// Initializes a new instance of a WindowFrame.
        /// </summary>
        protected WindowFrame()
        {
        }

        /// <summary>
        /// Gets or sets which keyword to use for the frame.
        /// </summary>
        public FrameType FrameType { get; set; }

        /// <summary>
        /// Gets the tokens for specifying a window frame.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating tokens.</param>
        /// <returns>The tokens making up the window frame.</returns>
        internal TokenStream GetDeclarationTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            FrameTypeConverter converter = new FrameTypeConverter();
            stream.Add(converter.ToToken(FrameType));
            stream.AddRange(GetWindowFrameTokens(options));
            return stream;
        }

        /// <summary>
        /// Gets the tokens for specifying a window frame.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating tokens.</param>
        /// <returns>The tokens making up the window frame.</returns>
        protected abstract TokenStream GetWindowFrameTokens(CommandOptions options);

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            OnAccept(visitor);
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected abstract void OnAccept(BuilderVisitor visitor);
    }
}
