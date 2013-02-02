﻿using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a column being set to a value to the command.
    /// </summary>
    public class Setter
    {
        private readonly Column _column;
        private readonly IProjectionItem _value;

        /// <summary>
        /// Initializes a new instance of a Setter.
        /// </summary>
        /// <param name="column">The name of the column to set.</param>
        /// <param name="value">The value to set the column to.</param>
        public Setter(Column column, IProjectionItem value)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            _column = column;
            _value = value;
        }

        /// <summary>
        /// Gets the column being set.
        /// </summary>
        public Column Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Gets the value that the column is being set to.
        /// </summary>
        public IProjectionItem Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the expression for setting a column in an update statement.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The setter expression.</returns>
        public IEnumerable<string> GetSetterExpression(CommandOptions options)
        {
            // <Setter> => <Column> "=" <Projection>
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
            foreach (string token in formatter.GetUnaliasedReference(_column))
            {
                yield return token;
            }
            yield return "=";
            foreach (string token in formatter.GetUnaliasedReference(_value))
            {
                yield return token;
            }
        }
    }
}
