using System;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a function call to a command.
    /// </summary>
    public class Function : IFunction
    {
        private readonly ISchema _schema;
        private readonly string _name;
        private readonly IInList _arguments;
        private string _alias;

        /// <summary>
        /// Initializes a new instance of a Function.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        public Function(string name)
            : this(null, name)
        {
        }

        /// <summary>
        /// Initializes a new instance of a Function.
        /// </summary>
        /// <param name="schema">The schema the function exists in.</param>
        /// <param name="name">The name of the function.</param>
        public Function(ISchema schema, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankFunctionName, "name");
            }
            _schema = schema;
            _name = name;
            _arguments = new InList();
        }

        /// <summary>
        /// Gets or sets the schema the functions belongs to.
        /// </summary>
        public ISchema Schema
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
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        /// <summary>
        /// Gets a list of the arguments being passed to the function.
        /// </summary>
        public IInList Arguments
        {
            get
            {
                return _arguments;
            }
        }

        string IProjectionItem.GetFullText()
        {
            StringBuilder result = new StringBuilder();
            if (_schema != null)
            {
                result.Append(_schema.Name);
                result.Append(".");
            }
            result.Append(_name);
            result.Append(_arguments.GetFilterItemText());
            return result.ToString();
        }

        string IFilterItem.GetFilterItemText()
        {
            StringBuilder result = new StringBuilder();
            if (_schema != null)
            {
                result.Append(_schema.Name);
                result.Append(".");
            }
            result.Append(_name);
            result.Append(_arguments.GetFilterItemText());
            return result.ToString();
        }

        string IGroupByItem.GetGroupByItemText()
        {
            StringBuilder result = new StringBuilder();
            if (_schema != null)
            {
                result.Append(_schema.Name);
                result.Append(".");
            }
            result.Append(_name);
            result.Append(_arguments.GetFilterItemText());
            return result.ToString();
        }
    }
}
