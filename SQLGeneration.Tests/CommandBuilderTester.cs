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

        /// <summary>
        /// This sees whether we can reproduce a select statement with a simple WHERE clause.
        /// </summary>
        [TestMethod]
        public void TestSelect_Where()
        {
            string commandText = "SELECT 1 WHERE 1 = 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a simple GROUP BY clause.
        /// </summary>
        [TestMethod]
        public void TestSelect_GroupBy()
        {
            string commandText = "SELECT COUNT(1) FROM Table GROUP BY Table.Column";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a simple HAVING clause.
        /// </summary>
        [TestMethod]
        public void TestSelect_Having()
        {
            string commandText = "SELECT 1 HAVING 1 = 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement that selects from multiple tables.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleSelect()
        {
            string commandText = "SELECT 1 FROM Table1, Table2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a wrapped join.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedJoin()
        {
            string commandText = "SELECT 1 FROM (Table1 INNER JOIN Table2 ON Table1.Column = Table2.Column)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with multiple wrapped joins.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleWrappedJoins()
        {
            string commandText = "SELECT 1 FROM ((Table1 INNER JOIN Table2 ON Table1.Column = Table2.Column) INNER JOIN Table3 ON Table2.Column = Table3.Column)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with an aliased table.
        /// </summary>
        [TestMethod]
        public void TestSelect_AliasedTable()
        {
            string commandText = "SELECT t.Column FROM Table t";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with an aliased table.
        /// </summary>
        [TestMethod]
        public void TestSelect_TwoAliasedTables()
        {
            string commandText = "SELECT t1.Column FROM Table1 t1 CROSS JOIN Table2 t2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement that uses a select statement as a source.
        /// </summary>
        [TestMethod]
        public void TestSelect_FromSelect()
        {
            string commandText = "SELECT 1 FROM (SELECT 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement that uses a function call as a source.
        /// </summary>
        [TestMethod]
        public void TestSelect_FromFunctionCall()
        {
            string commandText = "SELECT 1 FROM GetData()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a left outer join.
        /// </summary>
        [TestMethod]
        public void TestSelect_LeftOuterJoin()
        {
            string commandText = "SELECT t1.Column FROM Table1 t1 LEFT OUTER JOIN Table2 t2 ON t1.Column = t2.Column";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a right outer join.
        /// </summary>
        [TestMethod]
        public void TestSelect_RightOuterJoin()
        {
            string commandText = "SELECT t1.Column FROM Table1 t1 RIGHT OUTER JOIN Table2 t2 ON t1.Column = t2.Column";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a full outer join.
        /// </summary>
        [TestMethod]
        public void TestSelect_FullOuterJoin()
        {
            string commandText = "SELECT t1.Column FROM Table1 t1 FULL OUTER JOIN Table2 t2 ON t1.Column = t2.Column";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can select mulitple projection items.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleProjectionItems()
        {
            string commandText = "SELECT 3.14, 'Hello', NULL, SUM(1), Table.Column, (SELECT 123) FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an aliased projection.
        /// </summary>
        [TestMethod]
        public void TestSelect_AliasedProjection()
        {
            string commandText = "SELECT Table.Column AS c FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a star select.
        /// </summary>
        [TestMethod]
        public void TestSelect_Star()
        {
            string commandText = "SELECT * FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a star select.
        /// </summary>
        [TestMethod]
        public void TestSelect_QualifiedStar()
        {
            string commandText = "SELECT Table.* FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple group by items.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleGroupByItems()
        {
            string commandText = "SELECT Column1, Column2, COUNT(1) FROM Table GROUP BY Column1, Column2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a wrapped set of filters.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedFilters()
        {
            string commandText = "SELECT Column1 FROM Table WHERE (Column2 = 123 AND Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that combines two filters with an OR.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrFilters()
        {
            string commandText = "SELECT Column1 FROM Table WHERE (Column2 = 123 OR Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a single wrapped filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedLeftFilter()
        {
            string commandText = "SELECT Column1 FROM Table WHERE (Column2 = 123) OR Column3 IS NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a single wrapped filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedRightFilter()
        {
            string commandText = "SELECT Column1 FROM Table WHERE Column2 = 123 AND (Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a multiple wrapped filters.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleWrappedFilter()
        {
            string commandText = "SELECT Column1 FROM Table WHERE ((Column2 = 123) AND (Column3 IS NULL))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an OR filter negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrFilterNegated()
        {
            string commandText = "SELECT Column1 FROM Table WHERE NOT (Column2 = 123 OR Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an AND filter negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_AndFilterNegated()
        {
            string commandText = "SELECT Column1 FROM Table WHERE NOT (Column2 = 123 AND Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalRightOrFilterNegated()
        {
            string commandText = "SELECT Column1 FROM Table WHERE Column1 = 'abc' OR NOT (Column2 = 123 AND Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalLeftOrFilterNegated()
        {
            string commandText = "SELECT Column1 FROM Table WHERE NOT (Column1 = 'abc' OR Column2 = 123) AND Column3 IS NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalRightAndFilterNegated()
        {
            string commandText = "SELECT Column1 FROM Table WHERE Column1 = 'abc' AND NOT (Column2 = 123 OR Column3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalLeftAndFilterNegated()
        {
            string commandText = "SELECT Column1 FROM Table WHERE NOT (Column1 = 'abc' AND Column2 = 123) OR Column3 IS NULL";
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
