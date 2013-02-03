using System;

namespace SQLGeneration
{
    /// <summary>
    /// Provides extension methods for defining filters using a natural language syntax.
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Creates a filter where the item is expected to equal the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter EqualTo(this IFilterItem leftHand, IFilterItem rightHand)
        {
            return new EqualToFilter(leftHand, rightHand);
        }

        /// <summary>
        /// Creates a filter where the item is expected to not equal the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter NotEqualTo(this IFilterItem leftHand, IFilterItem rightHand)
        {
            return new NotEqualToFilter(leftHand, rightHand);
        }

        /// <summary>
        /// Creates a filter where the item is expected to be less than the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter LessThan(this IFilterItem leftHand, IFilterItem rightHand)
        {
            return new LessThanFilter(leftHand, rightHand);
        }
        /// <summary>
        /// Creates a filter where the item is expected to be less than or equal to the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter LessThanEqualTo(this IFilterItem leftHand, IFilterItem rightHand)
        {
            return new LessThanEqualToFilter(leftHand, rightHand);
        }

        /// <summary>
        /// Creates a filter where the item is expected to be greater than the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter GreaterThan(this IFilterItem leftHand, IFilterItem rightHand)
        {
            return new GreaterThanFilter(leftHand, rightHand);
        }

        /// <summary>
        /// Creates a filter where the item is expected to be greater than or equal to the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter GreaterThanEqualTo(this IFilterItem leftHand, IFilterItem rightHand)
        {
            return new GreaterThanEqualToFilter(leftHand, rightHand);
        }

        /// <summary>
        /// Creates a filter where the item is expected to be like the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter Like(this IFilterItem leftHand, StringLiteral rightHand)
        {
            return new LikeFilter(leftHand, rightHand);
        }

        /// <summary>
        /// Creates a filter where the item is expected to not be like the given value.
        /// </summary>
        /// <param name="leftHand">The current item.</param>
        /// <param name="rightHand">The item to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter NotLike(this IFilterItem leftHand, StringLiteral rightHand)
        {
            return new LikeFilter(leftHand, rightHand) { Not = true };
        }

        /// <summary>
        /// Creates a filter where the item is expected to not be like the given value.
        /// </summary>
        /// <param name="item">The current item.</param>
        /// <param name="values">The values to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter In(this IFilterItem item, IValueProvider values)
        {
            return new InFilter(item, values);
        }

        /// <summary>
        /// Creates a filter where the item is expected to be like the given value.
        /// </summary>
        /// <param name="item">The current item.</param>
        /// <param name="values">The values to compare the current item to.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter NotIn(this IFilterItem item, IValueProvider values)
        {
            return new InFilter(item, values) { Not = true };
        }

        /// <summary>
        /// Creates a filter where the item is expected to be between the given values.
        /// </summary>
        /// <param name="item">The current item.</param>
        /// <param name="lowerBound">The smallest value the item can be equal to.</param>
        /// <param name="upperBound">The largest value the item can be equal to</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter Between(this IFilterItem item, IFilterItem lowerBound, IFilterItem upperBound)
        {
            return new BetweenFilter(item, lowerBound, upperBound);
        }

        /// <summary>
        /// Creates a filter where the item is expected to not be between the given values.
        /// </summary>
        /// <param name="item">The current item.</param>
        /// <param name="lowerBound">The smallest value the item can be equal to.</param>
        /// <param name="upperBound">The largest value the item can be equal to</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter NotBetween(this IFilterItem item, IFilterItem lowerBound, IFilterItem upperBound)
        {
            return new BetweenFilter(item, lowerBound, upperBound) { Not = true };
        }

        /// <summary>
        /// Creates a filter where the item is expected to be between the given values.
        /// </summary>
        /// <param name="item">The current item.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter IsNull(this IFilterItem item)
        {
            return new NullFilter(item);
        }

        /// <summary>
        /// Creates a filter where the item is expected to not be between the given values.
        /// </summary>
        /// <param name="item">The current item.</param>
        /// <returns>A new filter comparing the current value to the given value.</returns>
        public static IFilter IsNotNull(this IFilterItem item)
        {
            return new NullFilter(item) { Not = true };
        }

        /// <summary>
        /// Combines two filters such that both must be satisfied in order for the new filter to be satisfied.
        /// </summary>
        /// <param name="leftHand">The left hand filter.</param>
        /// <param name="rightHand">The right hand filter.</param>
        /// <returns>The new, combined, filter.</returns>
        public static IFilter And(this IFilter leftHand, IFilter rightHand)
        {
            FilterGroup group = new FilterGroup();
            group.AddFilter(leftHand, Conjunction.And);
            group.AddFilter(rightHand, Conjunction.And);
            return group;
        }

        /// <summary>
        /// Combines two filters such that either can be satisfied in order for the new filter to be satisfied.
        /// </summary>
        /// <param name="leftHand">The left hand filter.</param>
        /// <param name="rightHand">The right hand filter.</param>
        /// <returns>The new, combined, filter.</returns>
        public static IFilter Or(this IFilter leftHand, IFilter rightHand)
        {
            FilterGroup group = new FilterGroup();
            group.AddFilter(leftHand, Conjunction.And);
            group.AddFilter(rightHand, Conjunction.Or);
            return group;
        }
    }
}
