using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Describes the window that a function is applied to.
    /// </summary>
    public class FunctionWindow
    {
        private readonly List<AliasedProjection> partitionItems;
        private readonly List<OrderBy> orderByItems;
        
        /// <summary>
        /// Initializes a new instance of a FunctionWindow.
        /// </summary>
        public FunctionWindow()
        {
            this.partitionItems = new List<AliasedProjection>();
            this.orderByItems = new List<OrderBy>();
        }

        /// <summary>
        /// Gets the items making up the partitioning.
        /// </summary>
        public IEnumerable<AliasedProjection> Partition
        {
            get { return partitionItems; }
        }

        /// <summary>
        /// Adds the item as a partitioner.
        /// </summary>
        /// <param name="item">The item to partition the records on.</param>
        /// <returns>An aliased projection wrapping the given item.</returns>
        public AliasedProjection AddPartition(IProjectionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            AliasedProjection projection = new AliasedProjection(item, null);
            partitionItems.Add(projection);
            return projection;
        }

        /// <summary>
        /// Adds the item as a partitioner.
        /// </summary>
        /// <param name="item">The aliased projection to add.</param>
        public void AddPartition(AliasedProjection item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            partitionItems.Add(item);
        }

        /// <summary>
        /// Removes the item from the partition.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemovePartition(AliasedProjection item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return partitionItems.Remove(item);
        }

        /// <summary>
        /// Gets the order by items.
        /// </summary>
        public IEnumerable<OrderBy> OrderBy
        {
            get { return orderByItems; }
        }

        /// <summary>
        /// Gets the order by items.
        /// </summary>
        internal List<OrderBy> OrderByList
        {
            get { return orderByItems; }
        }

        /// <summary>
        /// Adds the item as a sort condition to the window.
        /// </summary>
        /// <param name="orderBy">The order by to add.</param>
        public void AddOrderBy(OrderBy orderBy)
        {
            if (orderBy == null)
            {
                throw new ArgumentNullException("orderBy");
            }
            orderByItems.Add(orderBy);
        }

        /// <summary>
        /// Removes the item as a sort condition to the window.
        /// </summary>
        /// <param name="orderBy">The order by to remove.</param>
        /// <returns>True if the order by was removed; otherwise, false.</returns>
        public bool RemoveOrderBy(OrderBy orderBy)
        {
            if (orderBy == null)
            {
                throw new ArgumentNullException("orderBy");
            }
            return orderByItems.Remove(orderBy);
        }

        /// <summary>
        /// Gets or sets the window framing.
        /// </summary>
        public WindowFrame Frame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the tokens for specifying a window over a function.
        /// </summary>
        /// <param name="options">The configuration settings to use when generating tokens.</param>
        /// <returns>The tokens making up the function window.</returns>
        internal TokenStream GetDeclarationTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.Add(new TokenResult(SqlTokenRegistry.Over, "OVER"));
            stream.Add(new TokenResult(SqlTokenRegistry.LeftParenthesis, "("));
            stream.AddRange(buildPartitionList(options));
            stream.AddRange(buildOrderByList(options));
            if (Frame != null)
            {
                stream.AddRange(Frame.GetDeclarationTokens(options));
            }
            stream.Add(new TokenResult(SqlTokenRegistry.RightParenthesis, ")"));
            return stream;
        }

        private TokenStream buildPartitionList(CommandOptions options)
        {
            using (IEnumerator<AliasedProjection> enumerator = partitionItems.GetEnumerator())
            {
                TokenStream stream = new TokenStream();
                if (enumerator.MoveNext())
                {
                    stream.Add(new TokenResult(SqlTokenRegistry.PartitionBy, "PARTITION BY"));
                    stream.AddRange(enumerator.Current.GetReferenceTokens(options));
                    while (enumerator.MoveNext())
                    {
                        stream.Add(new TokenResult(SqlTokenRegistry.Comma, ","));
                        stream.AddRange(enumerator.Current.GetReferenceTokens(options));
                    }
                }
                return stream;
            }
        }

        private TokenStream buildOrderByList(CommandOptions options)
        {
            using (IEnumerator<OrderBy> enumerator = orderByItems.GetEnumerator())
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
    }
}
