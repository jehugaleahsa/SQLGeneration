using System;
using SQLGeneration.Expressions;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Specifies the position of NULLs in a result set.
    /// </summary>
    public enum NullPlacement
    {
        /// <summary>
        /// The null placement is determined by the database provider.
        /// </summary>
        Default,
        /// <summary>
        /// The null values appear before the remaining results.
        /// </summary>
        First,
        /// <summary>
        /// The null values appear after the remaining results.
        /// </summary>
        Last,
    }

    /// <summary>
    /// Converts null placements to their string representations.
    /// </summary>
    internal class NullPlacementConverter
    {
        /// <summary>
        /// Initializes a new instance of a NullPlacementConverter.
        /// </summary>
        public NullPlacementConverter()
        {
        }

        /// <summary>
        /// Gets a string representation of the given null placement.
        /// </summary>
        /// <param name="placement">The null placement to convert to a string.</param>
        /// <returns>The string representation.</returns>
        public IExpressionItem ToToken(NullPlacement placement)
        {
            switch (placement)
            {
                case NullPlacement.Default:
                    return Expression.None;
                case NullPlacement.First:
                    return new Token("NULLS FIRST");
                case NullPlacement.Last:
                    return new Token("NULLS LAST");
                default:
                    throw new ArgumentException(Resources.UnknownNullPlacement, "placement");
            }
        }
    }
}
