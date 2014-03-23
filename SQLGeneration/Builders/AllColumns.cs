using System;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Selects all of the columns in a table or a join.
    /// </summary>
    public class AllColumns : IProjectionItem
    {
        private readonly AliasedSource source;

        /// <summary>
        /// Initializes a new instacne of an AllColumns
        /// that doesn't have a table or join.
        /// </summary>
        public AllColumns()
        {
        }

        /// <summary>
        /// Initializes a new instance of an AllColumns
        /// that selects all the columns from the given table or join.
        /// </summary>
        /// <param name="source">The table or join to select all the columns from.</param>
        public AllColumns(AliasedSource source)
        {
            this.source = source;
        }

        TokenStream IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            if (source != null)
            {
                stream.AddRange(source.GetReferenceTokens(options));
                stream.Add(new TokenResult(SqlTokenRegistry.Dot, "."));
            }
            stream.Add(new TokenResult(SqlTokenRegistry.MultiplicationOperator, "*"));
            return stream;
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            visitor.VisitAllColumns(this);
        }
    }
}
