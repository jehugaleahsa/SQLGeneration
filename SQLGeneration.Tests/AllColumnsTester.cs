using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the AllColumn class.
    /// </summary>
    [TestClass]
    public class AllColumnsTester
    {
        /// <summary>
        /// If we don't specify a join item, a single asterisk will show up by itself.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_NoJoinItem_PrintsStar()
        {
            IProjectionItem columns = new AllColumns();
            string actual = columns.GetFullText(new BuilderContext());
            string expected = "*";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// If we specify a join item, it's name will precede an asterisk.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_JoinItem_PrintsJointItemNameDotStar()
        {
            IProjectionItem columns = new AllColumns(new Table("Table"));
            string actual = columns.GetFullText(new BuilderContext());
            string expected = "Table.*";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// If we specify a join item with an alias, it's alias will precede an asterisk.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_AliasedJoinItem_PrintsJointItemAliasDotStar()
        {
            IProjectionItem columns = new AllColumns(new Table("Table") { Alias = "t" });
            string actual = columns.GetFullText(new BuilderContext());
            string expected = "t.*";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }
    }
}
