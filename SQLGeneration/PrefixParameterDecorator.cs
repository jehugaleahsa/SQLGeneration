using System;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Decorates a parameter name to make it a valid Oracle parameter.
    /// </summary>
    public class PrefixParameterDecorator : IParameterDecorator
    {
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of a PrefixParameterDecorator.
        /// </summary>
        /// <param name="prefix">The prefix used to indicate a parameter.</param>
        public PrefixParameterDecorator(string prefix)
        {
            if (String.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException(Resources.BlankParameterPrefix);
            }
            _prefix = prefix;
        }

        /// <summary>
        /// Decorates the given parameter name to make it a valid Oracle parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The decorated parameter name.</returns>
        public string DecorateParameter(string parameterName)
        {
            return _prefix + parameterName;
        }
    }
}
