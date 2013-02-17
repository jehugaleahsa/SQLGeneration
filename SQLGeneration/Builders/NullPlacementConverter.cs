using System;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Builders
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
        public TokenResult ToToken(NullPlacement placement)
        {
            switch (placement)
            {
                case NullPlacement.First:
                    return new TokenResult(SqlTokenRegistry.NullsFirst, "NULLS FIRST");
                case NullPlacement.Last:
                    return new TokenResult(SqlTokenRegistry.NullsLast, "NULLS LAST");
                default:
                    throw new ArgumentException(Resources.UnknownNullPlacement, "placement");
            }
        }
    }
}
