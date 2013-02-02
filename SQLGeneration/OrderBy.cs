using System;
using SQLGeneration.Properties;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an item in the order by clause of a select statement.
    /// </summary>
    public class OrderBy
    {
        private readonly IProjectionItem _item;
        private Order _order;
        private NullPlacement _placement;

        /// <summary>
        /// Initializes a new instance of a OrderBy.
        /// </summary>
        /// <param name="item">The item to sort by.</param>
        public OrderBy(IProjectionItem item)
            : this(item, Order.Default, NullPlacement.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of a OrderBy.
        /// </summary>
        /// <param name="item">The item to sort by.</param>
        /// <param name="order">The order in which to sort the items.</param>
        public OrderBy(IProjectionItem item, Order order)
            : this(item, order, NullPlacement.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of a OrderBy.
        /// </summary>
        /// <param name="item">The item to sort by.</param>
        /// <param name="order">The order in which to sort the items.</param>
        /// <param name="nullPlacement">The placement of nulls in the results.</param>
        public OrderBy(IProjectionItem item, Order order, NullPlacement nullPlacement)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _item = item;
            Order = order;
            NullPlacement = nullPlacement;
        }

        /// <summary>
        /// Gets the item to order by.
        /// </summary>
        public IProjectionItem Item
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets or sets the order to sort the results.
        /// </summary>
        public Order Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (!Enum.IsDefined(typeof(Order), value))
                {
                    throw new ArgumentException(Resources.UnknownOrder, "value");
                }
                _order = value;
            }
        }

        /// <summary>
        /// Specifies where null values appear in the results.
        /// </summary>
        public NullPlacement NullPlacement
        {
            get
            {
                return _placement;
            }
            set
            {
                if (!Enum.IsDefined(typeof(NullPlacement), value))
                {
                    throw new ArgumentException(Resources.UnknownNullPlacement, "value");
                }
                _placement = value;
            }
        }

        /// <summary>
        /// Gets the text making up the order by expression.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The order by text.</returns>
        public IEnumerable<string> GetOrderByExpression(CommandOptions options)
        {
            // <OrderBy> => <ColumnRef> [ { "ASC" | "DESC" } ] [ { "NULLS FIRST" | "NULLS LAST" } ]
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(options);
            foreach (string token in formatter.GetAliasedReference(_item))
            {
                yield return token;
            }
            if (_order != Order.Default)
            {
                OrderConverter converter = new OrderConverter();
                yield return converter.ToToken(_order);
            }
            if (_placement != NullPlacement.Default)
            {
                NullPlacementConverter converter = new NullPlacementConverter();
                yield return converter.ToToken(_placement);
            }
        }
    }
}
