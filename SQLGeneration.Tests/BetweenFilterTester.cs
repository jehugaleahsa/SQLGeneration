using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the BetweenFilter class.
    /// </summary>
    [TestClass]
    public class BetweenFilterTester
    {
        /// <summary>
        /// If we do set Not to false, it should not be printed.
        /// </summary>
        [TestMethod]
        public void TestBetweenFilter_NotFalse_PrintsWithoutNot()
        {
            IFilter filter = new BetweenFilter(new NumericLiteral(1), new NumericLiteral(0), new NumericLiteral(2));
            string actual = filter.GetFilterText(new BuilderContext());
            string expected = "1 BETWEEN 0 AND 2";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// If we do set Not to true, it should be printed.
        /// </summary>
        [TestMethod]
        public void TestBetweenFilter_NotTrue_PrintsNot()
        {
            IFilter filter = new BetweenFilter(new NumericLiteral(1), new NumericLiteral(0), new NumericLiteral(2)) { Not = true };
            string actual = filter.GetFilterText(new BuilderContext());
            string expected = "1 NOT BETWEEN 0 AND 2";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// If we do set IFilter.Not to true, it should be printed around the expression.
        /// </summary>
        [TestMethod]
        public void TestBetweenFilter_IFilterNotTrue_PrintsNotSurrounding()
        {
            Filter filter = new BetweenFilter(new NumericLiteral(1), new NumericLiteral(0), new NumericLiteral(2));
            filter.Not = true;
            string actual = ((IFilter)filter).GetFilterText(new BuilderContext());
            string expected = "NOT (1 BETWEEN 0 AND 2)";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }
    }
}
