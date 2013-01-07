using System;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a parameter to a command.
    /// </summary>
    public class Parameter : IParameter
    {
        private readonly IParameterDecorator _decorator;
        private readonly string _name;
        private string _alias;

        /// <summary>
        /// Initializes a new instance of a Parameter.
        /// </summary>
        /// <param name="decorator">The provider-specific parameter decorator.</param>
        /// <param name="name">The name of the parameter.</param>
        public Parameter(IParameterDecorator decorator, string name)
        {
            if (decorator == null)
            {
                throw new ArgumentNullException("decorator");
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankParameterName, "name");
            }
            _decorator = decorator;
            _name = name;
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets or sets an alias for the parameter.
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

        string IProjectionItem.GetFullText()
        {
            return _decorator.DecorateParameter(_name);
        }

        string IFilterItem.GetFilterItemText()
        {
            return _decorator.DecorateParameter(_name);
        }

        string IGroupByItem.GetGroupByItemText()
        {
            return _decorator.DecorateParameter(_name);
        }
    }
}
