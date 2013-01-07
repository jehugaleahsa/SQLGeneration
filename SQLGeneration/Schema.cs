using System;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Provides a schema name.
    /// </summary>
    public class Schema : ISchema
    {
        private string _name;

        /// <summary>
        /// Creates a new Schema.
        /// </summary>
        /// <param name="name">The name of the schema.</param>
        public Schema(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankSchemaName, "name");
            }
            _name = name;
        }

        /// <summary>
        /// Gets the name of the schema.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
