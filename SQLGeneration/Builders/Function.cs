using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Adds a function call to a command.
    /// </summary>
    public class Function : Filter, IProjectionItem, IRightJoinItem, IFilterItem, IGroupByItem, IValueProvider
    {
        private readonly Namespace qualifier;
        private readonly ValueList arguments;

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
        /// <param name="qualifier">The schema the function exists in.</param>
        /// <param name="name">The name of the function.</param>
        public Function(Namespace qualifier, string name)
            : this(qualifier, name, new IProjectionItem[0])
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
        /// <param name="qualifier">The schema the function exists in.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="arguments">The arguments being passed to the function.</param>
        public Function(Namespace qualifier, string name, params IProjectionItem[] arguments)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.BlankFunctionName, "name");
            }
            this.qualifier = qualifier;
            Name = name;
            this.arguments = new ValueList(arguments);
        }

        /// <summary>
        /// Gets or sets the schema the functions belongs to.
        /// </summary>
        public Namespace Qualifier
        {
            get { return qualifier; }
        }

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of the arguments being passed to the function.
        /// </summary>
        public IEnumerable<IProjectionItem> Arguments
        {
            get { return arguments.Values; }
        }

        /// <summary>
        /// Adds the given projection item to the arguments list.
        /// </summary>
        /// <param name="item">The value to add.</param>
        public void AddArgument(IProjectionItem item)
        {
            arguments.AddValue(item);
        }

        /// <summary>
        /// Removes the given projection item from the arguments list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveArgument(IProjectionItem item)
        {
            return arguments.RemoveValue(item);
        }

        /// <summary>
        /// Gets or sets the window to apply the function over.
        /// </summary>
        public FunctionWindow FunctionWindow
        {
            get;
            set;
        }

        TokenStream IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getFunctionTokens(options);
        }

        TokenStream IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getFunctionTokens(options);
        }

        TokenStream IGroupByItem.GetGroupByTokens(CommandOptions options)
        {
            return getFunctionTokens(options);
        }

        private TokenStream getFunctionTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            if (qualifier != null)
            {
                stream.AddRange(qualifier.GetNamespaceTokens(options));
                stream.Add(new TokenResult(SqlTokenRegistry.Dot, "."));
            }
            stream.Add(new TokenResult(SqlTokenRegistry.Identifier, Name));
            stream.AddRange(((IFilterItem)arguments).GetFilterTokens(options));
            if (FunctionWindow != null)
            {
                stream.AddRange(FunctionWindow.GetDeclarationTokens(options));
            }
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        bool IRightJoinItem.IsTable
        {
            get { return false; }
        }

        string IRightJoinItem.GetSourceName()
        {
            return null;
        }

        TokenStream IJoinItem.GetDeclarationTokens(CommandOptions options)
        {
            return getFunctionTokens(options);
        }

        bool IValueProvider.IsValueList
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the filter text irrespective of the parentheses.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override TokenStream GetInnerFilterTokens(CommandOptions options)
        {
            return getFunctionTokens(options);
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected override void OnAccept(BuilderVisitor visitor)
        {
            visitor.VisitFunction(this);
        }
    }
}
