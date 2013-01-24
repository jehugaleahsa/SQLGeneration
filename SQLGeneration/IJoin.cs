using System;
using System.Collections.Generic;

namespace SQLGeneration
{
    /// <summary>
    /// Adds a join between two joinable items to the command.
    /// </summary>
    public interface IJoin : IJoinItem
    {
        /// <summary>
        /// Gets or sets whether the join should be wrapped in parentheses.
        /// </summary>
        bool? WrapInParentheses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the item on the left hand side of the join.
        /// </summary>
        IJoinItem LeftHand
        {
            get;
        }

        /// <summary>
        /// Gets the item on the right hand side of the join.
        /// </summary>
        IJoinItem RightHand
        {
            get;
        }
    }
}
