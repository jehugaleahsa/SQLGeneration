using System;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a schema name.
    /// </summary>
    public interface ISchema
    {
        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
