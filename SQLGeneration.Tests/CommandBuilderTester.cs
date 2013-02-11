using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGeneration.Builders;
using SQLGeneration.Generators;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the CommandBuilder creates commands as expected.
    /// </summary>
    [TestClass]
    public class CommandBuilderTester
    {
        /// <summary>
        /// Tests that we can reproduce a simple select statement.
        /// </summary>
        [TestMethod]
        public void TestSelect()
        {
            string commandText = "SELECT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests that we can reproduce a select statement with an order by clause.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderBy()
        {
            string commandText = "SELECT 1 ORDER BY 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests that we can reproduce a combined select statement.
        /// </summary>
        [TestMethod]
        public void TestSelect_SelectCombiner()
        {
            string commandText = "SELECT 1 UNION SELECT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests that we can reproduce a combined select statement with a distinct qualifier.
        /// </summary>
        [TestMethod]
        public void TestSelect_SelectCombiner_DistinctQualifier()
        {
            string commandText = "SELECT 1 UNION ALL SELECT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests that we can reproduce a select statement with a distinct qualifier.
        /// </summary>
        [TestMethod]
        public void TestSelect_DistinctQualifier()
        {
            string commandText = "SELECT DISTINCT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests that we can reproduce a select statement with TOP limit.
        /// </summary>
        [TestMethod]
        public void TestSelect_Top()
        {
            string commandText = "SELECT TOP 1 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests that we can reproduce a select statement with a simple FROM clause.
        /// </summary>
        [TestMethod]
        public void TestSelect_FromTable()
        {
            string commandText = "SELECT 1 FROM Table";
            assertCanReproduce(commandText);
        }

        private void assertCanReproduce(string commandText)
        {
            CommandBuilder builder = new CommandBuilder();
            ICommand command = builder.GetCommand(commandText);
            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(command);
            Assert.AreEqual(commandText, actual, "The command builder did not generate the original command text.");
        }
    }
}
