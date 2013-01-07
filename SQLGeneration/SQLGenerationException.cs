using System;
using System.Runtime.Serialization;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs within SQLGeneration.
    /// </summary>
    [Serializable]
    public sealed class SQLGenerationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of a SQLGenerationException.
        /// </summary>
        internal SQLGenerationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of a SQLGenerationException.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        internal SQLGenerationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of a SQLGenerationException.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">The exception that caused the exception.</param>
        internal SQLGenerationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of a SQLGenerationException.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.
        /// </param>
        internal SQLGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
