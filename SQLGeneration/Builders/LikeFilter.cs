using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a comparison where the left hand item is greater than or equal to the right hand item.
    /// </summary>
    public class LikeFilter : Filter
    {
        /// <summary>
        /// Initializes a new instance of a LikeFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public LikeFilter(IFilterItem leftHand, StringLiteral rightHand)
        {
            if (leftHand == null)
            {
                throw new ArgumentNullException("leftHand");
            }
            if (rightHand == null)
            {
                throw new ArgumentNullException("rightHand");
            }
            LeftHand = leftHand;
            RightHand = rightHand;
        }

        /// <summary>
        /// Gets the object on the left of the operation.
        /// </summary>
        public IFilterItem LeftHand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object to the right of the operation.
        /// </summary>
        public StringLiteral RightHand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets whether to negate the comparison.
        /// </summary>
        public bool Not
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the tokens making up the comparison.
        /// </summary>
        /// <param name="options">The configuration settings to use when building the filter.</param>
        /// <returns>The tokens making up the comparison.</returns>
        protected override IEnumerable<string> GetInnerFilterTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(LeftHand.GetFilterTokens(options));
            if (Not)
            {
                stream.Add("NOT");
            }
            stream.Add("LIKE");
            stream.AddRange(((IFilterItem)RightHand).GetFilterTokens(options));
            return stream;
        }
    }
}
