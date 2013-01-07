using System;

namespace SQLGeneration
{
    /// <summary>
    /// Decorates a parameter so it is recognized by the provider.
    /// </summary>
    public interface IParameterDecorator
    {
        /// <summary>
        /// Creates the string representing a parameter placeholder for
        /// a specific provider.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The decorated parameter.</returns>
        string DecorateParameter(string parameterName);
    }
}
