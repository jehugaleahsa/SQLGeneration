using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Performs a set operation on the results of two queries.
    /// </summary>
    public abstract class SelectCombiner : ISelectBuilder
    {
        private readonly ISelectBuilder leftHand;
        private readonly ISelectBuilder rightHand;
        private readonly List<OrderBy> orderBy;

        /// <summary>
        /// Initializes a new instance of a SelectCombiner.
        /// </summary>
        protected SelectCombiner(ISelectBuilder leftHand, ISelectBuilder rightHand)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            this.leftHand = leftHand;
            this.rightHand = rightHand;
            this.orderBy = new List<OrderBy>();
        }

        /// <summary>
        /// Gets the SELECT command on the left side.
        /// </summary>
        public ISelectBuilder LeftHand
        {
            get { return leftHand; }
        }

        /// <summary>
        /// Gets the SELECT comman on the right side.
        /// </summary>
        public ISelectBuilder RightHand
        {
            get { return rightHand; }
        }

        /// <summary>
        /// Gets the distinct qualifier for the combiner.
        /// </summary>
        public DistinctQualifier Distinct
        {
            get;
            set;
        }

        SourceCollection ISelectBuilder.Sources
        { 
            get { return new SourceCollection(); } 
        }

        List<OrderBy> ISelectBuilder.OrderByList
        {
            get { return orderBy; }
        }

        /// <summary>
        /// Gets the items used to sort the results.
        /// </summary>
        public IEnumerable<OrderBy> OrderBy
        {
            get { return orderBy; }
        }

        /// <summary>
        /// Adds a sort criteria to the query.
        /// </summary>
        /// <param name="item">The sort criteria to add.</param>
        public void AddOrderBy(OrderBy item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            orderBy.Add(item);
        }

        /// <summary>
        /// Removes the sort criteria from the query.
        /// </summary>
        /// <param name="item">The order by item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveOrderBy(OrderBy item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return orderBy.Remove(item);
        }

        /// <summary>
        /// Gets the command expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The expression making up the command.</returns>
        TokenStream ICommand.GetCommandTokens(CommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            return getCommandTokens(options);
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected abstract TokenResult GetCombinationType(CommandOptions options);

        TokenStream IJoinItem.GetDeclarationTokens(CommandOptions options)
        {
            return getWrappedCommandTokens(options);
        }

        TokenStream IProjectionItem.GetProjectionTokens(CommandOptions options)
        {
            return getWrappedCommandTokens(options);
        }

        TokenStream IFilterItem.GetFilterTokens(CommandOptions options)
        {
            return getWrappedCommandTokens(options);
        }

        private TokenStream getWrappedCommandTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.LeftParenthesis, "("));
            stream.AddRange(getCommandTokens(options));
            stream.Add(new TokenResult(SqlTokenRegistry.RightParenthesis, ")"));
            return stream;
        }

        private TokenStream getCommandTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(leftHand.GetCommandTokens(options));
            stream.Add(GetCombinationType(options));
            if (Distinct != DistinctQualifier.Default)
            {
                DistinctQualifierConverter converter = new DistinctQualifierConverter();
                stream.Add(converter.ToToken(Distinct));
            }
            stream.AddRange(rightHand.GetCommandTokens(options));
            stream.AddRange(buildOrderBy(options));
            return stream;
        }

        private TokenStream buildOrderBy(CommandOptions options)
        {
            using (IEnumerator<OrderBy> enumerator = orderBy.GetEnumerator())
            {
                TokenStream stream = new TokenStream();
                if (enumerator.MoveNext())
                {
                    stream.Add(new TokenResult(SqlTokenRegistry.OrderBy, "ORDER BY"));
                    stream.AddRange(enumerator.Current.GetOrderByTokens(options));
                    while (enumerator.MoveNext())
                    {
                        stream.Add(new TokenResult(SqlTokenRegistry.Comma, ","));
                        stream.AddRange(enumerator.Current.GetOrderByTokens(options));
                    }
                }
                return stream;
            }
        }

        string IRightJoinItem.GetSourceName()
        {
            return null;
        }

        bool IRightJoinItem.IsTable
        {
            get { return false; }
        }

        bool IValueProvider.IsValueList
        {
            get { return false; }
        }

        string IProjectionItem.GetProjectionName()
        {
            return null;
        }

        void IVisitableBuilder.Accept(BuilderVisitor visitor)
        {
            OnAccept(visitor);
        }

        /// <summary>
        /// Provides information to the given visitor about the current builder.
        /// </summary>
        /// <param name="visitor">The visitor requesting information.</param>
        protected abstract void OnAccept(BuilderVisitor visitor);
    }
}
