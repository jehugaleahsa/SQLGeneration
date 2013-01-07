using System;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a TOP clause that is found in a SELECT statement.
    /// </summary>
    public interface ITop
    {
        /// <summary>
        /// Gets the number or arithmetic expression representing the number or percent of rows to return.
        /// </summary>
        IArithmetic Expression
        {
            get;
        }

        /// <summary>
        /// Gets whether or not the expression represents a percent.
        /// </summary>
        bool IsPercent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether records matching the last item according to the order by
        /// clause shall be returned.
        /// </summary>
        bool WithTies
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the textual representation of the TOP clause.
        /// </summary>
        string TopText
        {
            get;
        }
    }
}
