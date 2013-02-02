﻿using System;
using System.Collections.Generic;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a function call to a command.
    /// </summary>
    public class Function : IProjectionItem, IFilterItem, IGroupByItem
    {
        private readonly Schema _schema;
        private readonly string _name;
        private readonly ValueList _arguments;

        /// <summary>
        /// Initializes a new instance of a Function.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        public Function(string name)
            : this(null, name, new IProjectionItem[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of a Function.
        /// </summary>
        /// <param name="schema">The schema the function exists in.</param>
        /// <param name="name">The name of the function.</param>
        public Function(Schema schema, string name)
            : this(schema, name, new IProjectionItem[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of a Function.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="arguments">The arguments being passed to the function.</param>
        public Function(string name, params IProjectionItem[] arguments)
            : this(null, name, arguments)
        {
        }

        /// <summary>
        /// Initializes a new instance of a Function.
        /// </summary>
        /// <param name="schema">The schema the function exists in.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="arguments">The arguments being passed to the function.</param>
        public Function(Schema schema, string name, params IProjectionItem[] arguments)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankFunctionName, "name");
            }
            _schema = schema;
            _name = name;
            _arguments = new ValueList(arguments);
        }

        /// <summary>
        /// Gets or sets the schema the functions belongs to.
        /// </summary>
        public Schema Schema
        {
            get
            {
                return _schema;
            }
        }

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets or sets the alias of the function.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of the arguments being passed to the function.
        /// </summary>
        public IEnumerable<IProjectionItem> Arguments
        {
            get
            {
                return _arguments.Values;
            }
        }

        /// <summary>
        /// Adds the given projection item to the arguments list.
        /// </summary>
        /// <param name="item">The value to add.</param>
        public void AddArgument(IProjectionItem item)
        {
            _arguments.AddValue(item);
        }

        /// <summary>
        /// Removes the given projection item from the arguments list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveArgument(IProjectionItem item)
        {
            return _arguments.RemoveValue(item);
        }

        IEnumerable<string> IProjectionItem.GetProjectionExpression(CommandOptions options)
        {
            return getFunctionExpression(options);
        }

        IEnumerable<string> IFilterItem.GetFilterExpression(CommandOptions options)
        {
            return getFunctionExpression(options);
        }

        IEnumerable<string> IGroupByItem.GetGroupByExpression(CommandOptions options)
        {
            return getFunctionExpression(options);
        }

        private IEnumerable<string> getFunctionExpression(CommandOptions options)
        {
            // <Function> => [ <Schema> "." ] <Name> "(" [ <ValueList> ] ")"
            if (_schema != null)
            {
                yield return _schema.Name;
                yield return ".";
            }
            yield return _name;
            foreach (string token in ((IFilterItem)_arguments).GetFilterExpression(options))
            {
                yield return token;
            }
        }
    }
}
