using System;
using System.Collections.Generic;
using SQLGeneration.Parsing;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Represents a join between two tables or sub-queries.
    /// </summary>
    public abstract class BinaryJoin : Join
    {
        /// <summary>
        /// Initializes a new instance of a BinaryJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item or join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        protected BinaryJoin(Join leftHand, AliasedSource rightHand)
            : base(leftHand, rightHand)
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
        /// Gets the item on the left hand side of the join.
        /// </summary>
        public Join LeftHand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the table on the right hand side of the join.
        /// </summary>
        public AliasedSource RightHand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a string that declares the item.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>A string declaring the item.</returns>
        internal override IEnumerable<string> GetDeclarationTokens(CommandOptions options)
        {
            TokenStream stream = new TokenStream();
            stream.AddRange(LeftHand.GetDeclarationTokens(options));
            stream.Add(GetJoinType(options));
            stream.AddRange(((IJoinItem)RightHand).GetDeclarationTokens(options));
            stream.AddRange(GetOnTokens(options));
            return stream;
        }

        /// <summary>
        /// Gets the ON expression for the join.
        /// </summary>
        /// <param name="options">The configuration settings to use.</param>
        /// <returns>The generated text.</returns>
        protected abstract IEnumerable<string> GetOnTokens(CommandOptions options);

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="options">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected abstract string GetJoinType(CommandOptions options);
    }
}
