using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGeneration.Builders;
using SQLGeneration.Generators;
using System.Linq;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the CommandBuilder creates commands as expected.
    /// </summary>
    [TestClass]
    public class CommandBuilderTester
    {
        #region Select

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
            string commandText = "SELECT 1 FROM [Table]";
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
            string commandText = "SELECT COUNT(1) FROM [Table] GROUP BY [Table].TestCol";
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
            string commandText = "SELECT 1 FROM (Table1 INNER JOIN Table2 ON Table1.TestCol = Table2.TestCol)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with multiple wrapped joins.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleWrappedJoins()
        {
            string commandText = "SELECT 1 FROM ((Table1 INNER JOIN Table2 ON Table1.TestCol = Table2.TestCol) INNER JOIN Table3 ON Table2.TestCol = Table3.TestCol)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with an aliased table.
        /// </summary>
        [TestMethod]
        public void TestSelect_AliasedTable()
        {
            string commandText = "SELECT t.TestCol FROM [Table] t";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with an aliased table.
        /// </summary>
        [TestMethod]
        public void TestSelect_TwoAliasedTables()
        {
            string commandText = "SELECT t1.TestCol FROM Table1 t1 CROSS JOIN Table2 t2";
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
            string commandText = "SELECT t1.TestCol FROM Table1 t1 LEFT OUTER JOIN Table2 t2 ON t1.TestCol = t2.TestCol";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a right outer join.
        /// </summary>
        [TestMethod]
        public void TestSelect_RightOuterJoin()
        {
            string commandText = "SELECT t1.TestCol FROM Table1 t1 RIGHT OUTER JOIN Table2 t2 ON t1.TestCol = t2.TestCol";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select statement with a full outer join.
        /// </summary>
        [TestMethod]
        public void TestSelect_FullOuterJoin()
        {
            string commandText = "SELECT t1.TestCol FROM Table1 t1 FULL OUTER JOIN Table2 t2 ON t1.TestCol = t2.TestCol";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can select mulitple projection items.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleProjectionItems()
        {
            string commandText = "SELECT 3.14, 'Hello', NULL, SUM(1), [Table].TestCol, (SELECT 123) FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an aliased projection.
        /// </summary>
        [TestMethod]
        public void TestSelect_AliasedProjection()
        {
            string commandText = "SELECT [Table].TestCol AS c FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a star select.
        /// </summary>
        [TestMethod]
        public void TestSelect_Star()
        {
            string commandText = "SELECT * FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a star select.
        /// </summary>
        [TestMethod]
        public void TestSelect_QualifiedStar()
        {
            string commandText = "SELECT [Table].* FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple group by items.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleGroupByItems()
        {
            string commandText = "SELECT TestCol1, TestCol2, COUNT(1) FROM [Table] GROUP BY TestCol1, TestCol2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a wrapped set of filters.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedFilters()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE (TestCol2 = 123 AND TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that combines two filters with an OR.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrFilters()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE (TestCol2 = 123 OR TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a single wrapped filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedLeftFilter()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE (TestCol2 = 123) OR TestCol3 IS NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a single wrapped filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_WrappedRightFilter()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE TestCol2 = 123 AND (TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a multiple wrapped filters.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleWrappedFilter()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE ((TestCol2 = 123) AND (TestCol3 IS NULL))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an OR filter negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrFilterNegated()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE NOT (TestCol2 = 123 OR TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an AND filter negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_AndFilterNegated()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE NOT (TestCol2 = 123 AND TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalRightOrFilterNegated()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE TestCol1 = 'abc' OR NOT (TestCol2 = 123 AND TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalLeftOrFilterNegated()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE NOT (TestCol1 = 'abc' OR TestCol2 = 123) AND TestCol3 IS NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalRightAndFilterNegated()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE TestCol1 = 'abc' AND NOT (TestCol2 = 123 OR TestCol3 IS NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple filters negated.
        /// </summary>
        [TestMethod]
        public void TestSelect_InternalLeftAndFilterNegated()
        {
            string commandText = "SELECT TestCol1 FROM [Table] WHERE NOT (TestCol1 = 'abc' AND TestCol2 = 123) OR TestCol3 IS NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_Select()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol > ALL (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_ValueList()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol > ALL (1, 2, 3)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a BETWEEN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_BetweenFilter()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol BETWEEN 1 AND 10";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a LIKE filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_LikeFilter()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol LIKE '%ABC'";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a LIKE filter
        /// that sees if a TestCol is like another TestCol.
        /// </summary>
        [TestMethod]
        public void TestSelect_LikeFilter_CompareTwoColumns()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol1 LIKE TestCol2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a LIKE filter
        /// that sees if a column is like a parameter.
        /// </summary>
        [TestMethod]
        public void TestSelect_LikeFilter_CompareToParameter()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol1 LIKE @Parameter";
            CommandBuilderOptions options = new CommandBuilderOptions()
            {
                PlaceholderPrefix = "@"
            };
            assertCanReproduce(commandText, options);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an IN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_InFilter_ValueList()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol IN (1, 2, 3)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an IN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_InFilter_Select()
        {
            string commandText = "SELECT TestCol FROM Table1 WHERE TestCol IN (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an IN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_InFilter_FunctionCall()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol IN GetData()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an EXISTS filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_ExistsFilter()
        {
            string commandText = "SELECT TestCol FROM Table1 WHERE EXISTS(SELECT 1 FROM Table2 WHERE Table1.TestCol = Table2.TestCol)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_Any()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol > ANY (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_Some()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol > SOME (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_EqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol = ALL (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_NotEqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol <> ALL (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_LessThanEqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol <= ALL (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_GreaterThanEqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol >= ALL (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_LessThan()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol < ALL (SELECT TestCol FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_NotEqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol <> 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_LessThanEqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol <= 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_GreaterThanEqualTo()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol >= 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_LessThan()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol < 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_GreaterThan()
        {
            string commandText = "SELECT TestCol FROM [Table] WHERE TestCol > 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a combiner.
        /// </summary>
        [TestMethod]
        public void TestSelect_Intersect()
        {
            string commandText = "SELECT 1 INTERSECT SELECT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a combiner.
        /// </summary>
        [TestMethod]
        public void TestSelect_Except()
        {
            string commandText = "SELECT 1 EXCEPT SELECT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a combiner.
        /// </summary>
        [TestMethod]
        public void TestSelect_Minus()
        {
            string commandText = "SELECT 1 MINUS SELECT 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with multiple order bys.
        /// </summary>
        [TestMethod]
        public void TestSelect_MultipleOrderBys()
        {
            string commandText = "SELECT TestCol1, TestCol2, TestCol3 FROM [Table] ORDER BY TestCol1, TestCol2, TestCol3";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column in descending order.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByDescending()
        {
            string commandText = "SELECT TestCol FROM [Table] ORDER BY TestCol DESC";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column in descending order.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByAscending()
        {
            string commandText = "SELECT TestCol FROM [Table] ORDER BY TestCol ASC";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column with nulls first.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByNullsFirst()
        {
            string commandText = "SELECT TestCol FROM [Table] ORDER BY TestCol NULLS FIRST";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column with nulls last.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByNullsLast()
        {
            string commandText = "SELECT TestCol FROM [Table] ORDER BY TestCol NULLS LAST";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column descending with nulls first.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByDescendingNullsLast()
        {
            string commandText = "SELECT TestCol FROM [Table] ORDER BY TestCol DESC NULLS FIRST";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that adds: 1 + 1.
        /// </summary>
        [TestMethod]
        public void TestSelect_Addition()
        {
            string commandText = "SELECT (1 + 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that subtracts: 1 - 1.
        /// </summary>
        [TestMethod]
        public void TestSelect_Subtraction()
        {
            string commandText = "SELECT (1 - 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that multiplies: 1 * 1.
        /// </summary>
        [TestMethod]
        public void TestSelect_Multiplication()
        {
            string commandText = "SELECT (1 * 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that divides: 1 / 1.
        /// </summary>
        [TestMethod]
        public void TestSelect_Division()
        {
            string commandText = "SELECT (1 / 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that performs modulus: 1 % 1.
        /// </summary>
        [TestMethod]
        public void TestSelect_Modulus()
        {
            string commandText = "SELECT (1 % 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that has multiple expressions.
        /// </summary>
        [TestMethod]
        public void TestSelect_ParenthesesOnLeft()
        {
            string commandText = "SELECT (1 + 1) * 1";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that has multiple expressions.
        /// </summary>
        [TestMethod]
        public void TestSelect_ParenthesesOnRight()
        {
            string commandText = "SELECT 1 * (1 + 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that negates an expression.
        /// </summary>
        [TestMethod]
        public void TestSelect_NegateExpression()
        {
            string commandText = "SELECT -(1 + 1)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that fully-qualifies a column.
        /// </summary>
        [TestMethod]
        public void TestSelect_FullyQualifiedColumn()
        {
            string commandText = "SELECT Server.Db.Owner.Tbl.TestCol FROM Server.Db.Owner.Tbl";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that doesn't qualify a column when multiple tables are present.
        /// </summary>
        [TestMethod]
        public void TestSelect_UnqualifiedColumn_MultipleSources()
        {
            string commandText = "SELECT TestCol FROM Table1, Table2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to build a function that applies ROW_NUMBER to each row to
        /// return a window.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithOrderingWindow()
        {
            string commandText = "SELECT inner.c1 FROM (SELECT [Table].TestCol1 AS c1, ROW_NUMBER() OVER (ORDER BY [Table].TestCol2, [Table].TestCol3) AS rn FROM [Table]) inner WHERE inner.rn BETWEEN 11 AND 20";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to apply a function to a partition.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithPartitioning()
        {
            string commandText = "SELECT Employee.Type AS Type, COUNT(1) OVER (PARTITION BY Type) AS Count FROM Employee";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to apply a function to a partitioned, ordered frame.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithBetweenFraming()
        {
            string commandText = "SELECT sale.prod_id, sale.month_num, sale.sales, SUM(sale.sales) OVER (PARTITION BY sale.prod_id ORDER BY sale.month_num ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) FROM sale";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to apply a function to a partitioned, ordered frame.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithStartFraming()
        {
            string commandText = "SELECT sale.prod_id, sale.month_num, sale.sales, SUM(sale.sales) OVER (PARTITION BY sale.prod_id ORDER BY sale.month_num ROWS 12 PRECEDING) FROM sale";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to apply a function to a partitioned, ordered frame.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithBoundBetweenFraming()
        {
            string commandText = "SELECT sale.prod_id, sale.month_num, sale.sales, SUM(sale.sales) OVER (PARTITION BY sale.prod_id ORDER BY sale.month_num RANGE BETWEEN 12 PRECEDING AND 12 FOLLOWING) FROM sale";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to specify that a frame window starts and stops with the current row.
        /// This is pointless, but nonetheless possible.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithBetweenFramingCurrentRow()
        {
            string commandText = "SELECT sale.prod_id, sale.month_num, sale.sales, SUM(sale.sales) OVER (PARTITION BY sale.prod_id ORDER BY sale.month_num ROWS BETWEEN CURRENT ROW AND CURRENT ROW) FROM sale";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with just one case.
        /// </summary>
        [TestMethod]
        public void TestSelect_MatchCase_MultipleCases()
        {
            string commandText = "SELECT CASE [Table].TestCol WHEN 0 THEN 'Sunday' WHEN 1 THEN 'Monday' WHEN 2 THEN 'Tuesday' WHEN 3 THEN 'Wednesday' WHEN 4 THEN 'Thursday' WHEN 5 THEN 'Friday' WHEN 6 THEN 'Saturday' END FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with an ELSE branch.
        /// </summary>
        [TestMethod]
        public void TestSelect_MatchCase_Else()
        {
            string commandText = "SELECT CASE [Table].TestCol WHEN 'Admin' THEN 'Administrator' ELSE 'User' END FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with just one case.
        /// </summary>
        [TestMethod]
        public void TestSelect_ConditionalCase_MultipleCases()
        {
            string commandText = "SELECT CASE WHEN [Table].TestCol = 0 THEN 'Sunday' WHEN [Table].TestCol = 1 THEN 'Monday' WHEN [Table].TestCol = 2 THEN 'Tuesday' WHEN [Table].TestCol = 3 THEN 'Wednesday' WHEN [Table].TestCol = 4 THEN 'Thursday' WHEN [Table].TestCol = 5 THEN 'Friday' WHEN [Table].TestCol = 6 THEN 'Saturday' END FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with an ELSE branch.
        /// </summary>
        [TestMethod]
        public void TestSelect_ConditionalCase_Else()
        {
            string commandText = "SELECT CASE WHEN [Table].TestCol = 'Admin' THEN 'Administrator' ELSE 'User' END FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// A function can appear in the WHERE clause of a SQL statement.
        /// </summary>
        [TestMethod]
        public void TestSelect_FilterWithFunction()
        {
            string commandText = "SELECT contactid, firstname, lastname FROM contact WHERE CONTAINS(firstname, 'albert')";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// Tests whether we can reproduce a select statement without any sources.
        /// </summary>
        [TestMethod]
        public void TestSelect_NoSources()
        {
            string commandText = @"SELECT SomeFunction('ServerName') AS test1, SomeFunction('ServerName') AS test2";
            assertCanReproduce(commandText);
        }

        #endregion

        #region Insert

        /// <summary>
        /// This sees whether we can reproduce a simple insert statement.
        /// </summary>
        [TestMethod]
        public void TestInsert_NoColumns_NoValues()
        {
            string commandText = "INSERT INTO [Table] VALUES()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement where the table is aliased.
        /// </summary>
        [TestMethod]
        public void TestInsert_AliasedTable()
        {
            string commandText = "INSERT INTO [Table] t VALUES()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement with columns and values listed.
        /// </summary>
        [TestMethod]
        public void TestInsert_ColumnsAndValues()
        {
            string commandText = "INSERT INTO [Table] (Column1, Column2, Column3) VALUES(123, 'hello', NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement whose values comes from a select statement.
        /// </summary>
        [TestMethod]
        public void TestInsert_ColumnsAndSelect()
        {
            string commandText = "INSERT INTO [Table] (Column1, Column2, Column3) (SELECT 123, 'hello', NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement with an output clause.
        /// </summary>
        [TestMethod]
        public void TestInsert_ColumnsAndOutputInserted()
        {
            string commandText = "INSERT INTO [Table] (Column1, Column2, Column3) OUTPUT INSERTED.Column1, INSERTED.Column2, INSERTED.Column3 VALUES(1, 2, 3)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement with an output clause.
        /// </summary>
        [TestMethod]
        public void TestInsert_ColumnsAndOutputInserted_AllColumns()
        {
            string commandText = "INSERT INTO [Table] (Column1, Column2, Column3) OUTPUT INSERTED.* VALUES(1, 2, 3)";
            assertCanReproduce(commandText);
        }

        #endregion

        #region Update

        /// <summary>
        /// This sees whether we can reproduce a simple update statement.
        /// </summary>
        [TestMethod]
        public void TestUpdate()
        {
            string commandText = "UPDATE [Table] SET TestCol = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with an aliased [Table].
        /// </summary>
        [TestMethod]
        public void TestUpdate_AliasedTable()
        {
            string commandText = "UPDATE [Table] t SET TestCol = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with a where clause.
        /// </summary>
        [TestMethod]
        public void TestUpdate_WhereClause()
        {
            string commandText = "UPDATE [Table] SET TestCol2 = 'hello' WHERE TestCol1 = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with multiple setters.
        /// </summary>
        [TestMethod]
        public void TestUpdate_MultipleSetters()
        {
            string commandText = "UPDATE [Table] SET TestCol2 = 'hello', TestCol3 = NULL WHERE TestCol1 = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with an output clause.
        /// </summary>
        [TestMethod]
        public void TestUpdate_OutputColumns()
        {
            string commandText = "UPDATE [Table] SET TestCol = 123 OUTPUT DELETED.TestCol, INSERTED.TestCol";
            assertCanReproduce(commandText);
        }

        #endregion

        #region Delete

        /// <summary>
        /// This sees whether we can reproduce a simple delete statement.
        /// </summary>
        [TestMethod]
        public void TestDelete()
        {
            string commandText = "DELETE FROM [Table]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a delete statement with an aliased [Table].
        /// </summary>
        [TestMethod]
        public void TestDelete_AliasedTable()
        {
            string commandText = "DELETE FROM [Table] t";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a delete statement with where clause.
        /// </summary>
        [TestMethod]
        public void TestDelete_WhereClause()
        {
            string commandText = "DELETE FROM [Table] WHERE TestCol = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a delete statement with an output clause.
        /// </summary>
        [TestMethod]
        public void TestDelete_AndOutputColumns()
        {
            string commandText = "DELETE FROM [Table] OUTPUT DELETED.ID, DELETED.OtherColumn";
            assertCanReproduce(commandText);
        }       

        #endregion

        #region Batch

        /// <summary>
        /// This sees whether we can reproduce a batch of SQL statements.
        /// </summary>
        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void TestBatch_InvalidSql()
        {
            string commandText = @"SELECT FROM X";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a batch of SQL statements.
        /// </summary>
        [TestMethod]
        public void TestBatch_MixtureOfStatementsWithSemiColons()
        {
            string commandText = @"INSERT INTO TestTable (TestCol) VALUES(';');SELECT 1 UNION ALL SELECT 1;SELECT CASE WHEN TestTable.TestCol = 'Adm;in' THEN 'Administ;rator' ELSE 'Us;er' END FROM TestTable";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce multiple insert statements in a batch using a terminator.
        /// </summary>
        [TestMethod]
        public void TestBatch_MultipleInserts()
        {
            string commandText = @"INSERT INTO [Table] VALUES();INSERT INTO [Table] VALUES()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This tests whether the terminator is persisted when we parse and reproduce the sql.
        /// </summary>
        [TestMethod]
        public void TestBatch_TerminatorPersists()
        {
            string commandText = @"INSERT INTO [Table] VALUES();";
            assertCanReproduce(commandText);
        }

        #endregion

        #region Create

        /// <summary>
        /// This sees whether we can reproduce a simple create database statement.
        /// </summary>
        [TestMethod]
        public void TestCreateDatabase()
        {
            string commandText = "CREATE DATABASE NewDatabase";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a simple create database statement with collation.
        /// </summary>
        [TestMethod]
        public void TestCreateDatabase_Collation()
        {
            string commandText = "CREATE DATABASE NewDatabase COLLATE Latin_General";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a simple create table statement.
        /// </summary>
        [TestMethod]
        public void TestCreateTable()
        {
            string commandText = "CREATE TABLE NewTable";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a simple create table statement.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_MultiPartIdentifier()
        {
            string commandText = "CREATE TABLE somedatabase.dbo.NewTable";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement using quoted identifiers
        /// </summary>
        [TestMethod]
        public void TestCreateTable_QuotedMultiPartIdentifier()
        {
            string commandText = "CREATE TABLE [dbo].[NewTable]";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one simplistic column
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Simple()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA CHAR)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a COLLATION
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Collate()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA NCHAR COLLATE Latin1_General)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a precision and scale.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_DataType_PrecisionAndScale()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA DECIMAL(10,2))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a precision and scale.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_DataType_Max()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA VARCHAR(MAX))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has an identity.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Identity()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT IDENTITY(1,1))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has an identity and not for replication.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Identity_NotForReplication()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT IDENTITY(1,1) NOT FOR REPLICATION)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has an identity.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_RowGuid()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA UNIQUEIDENTIFIER ROWGUIDCOL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with multiple simple columns.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumns_Simple()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT, ColumnB CHAR)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a DEFAULT
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Default()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA NCHAR DEFAULT 'A')";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a DEFAULT and constraint name
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Default_Named_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA NCHAR CONSTRAINT my_constraint DEFAULT 'A')";
            assertCanReproduce(commandText);
        }


        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a NULL
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Null()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA NCHAR NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with one column that has a NOT NULL
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Not_Null()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA NCHAR NOT NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement using quoted identifiers
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumns_RealWorld()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA CHAR, ColumnB VARCHAR(150), ColumnC DECIMAL(10,2), ColumnD NTEXT COLLATE Latin1_General, ColumnE NCHAR COLLATE Latin1_General NOT NULL, ColumnF NVARCHAR COLLATE Latin1_General NULL DEFAULT 'Hello!', ColumnG NVARCHAR COLLATE Latin1_General NULL CONSTRAINT my_constraintname DEFAULT 'Wham!', ColumnH INT NOT NULL IDENTITY, ColumnI INT NOT NULL IDENTITY(1,1), ColumnJ INT NOT NULL DEFAULT 1, ColumnK UNIQUEIDENTIFIER ROWGUIDCOL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a primary key constriant.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_PrimaryKey_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT IDENTITY(1,1) PRIMARY KEY)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a named primary key constriant.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_PrimaryKey_Named_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT IDENTITY(1,1) CONSTRAINT my_mk PRIMARY KEY)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a unique key constriant.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Unique_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT IDENTITY(1,1) UNIQUE)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a named unique key constriant.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Unique_Named_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT IDENTITY(1,1) CONSTRAINT my_un UNIQUE)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint and no referenced column name.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_Constraint_NoColumnReference()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint not for replication.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_Constraint_NotForReplication()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) NOT FOR REPLICATION)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a named foreign key constraint.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_Named_Constraint()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT CONSTRAINT my_fk FOREIGN KEY REFERENCES dbo.mytable(idColumn))";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on delete no action.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnDelete_NoAction()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON DELETE NO ACTION)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on delete cascade.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnDelete_Cascade()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON DELETE CASCADE)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on delete set null.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnDelete_SetNull()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON DELETE SET NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on delete set default.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnDelete_SetDefault()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON DELETE SET DEFAULT)";
            assertCanReproduce(commandText);
        }


        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on update no action.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnUpdate_NoAction()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON UPDATE NO ACTION)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on update cascade.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnUpdate_Cascade()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON UPDATE CASCADE)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on update set null.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnUpdate_SetNull()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON UPDATE SET NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has a foreign key constraint with an on update set default.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_ForeignKey_OnUpdate_SetDefault()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON UPDATE SET DEFAULT)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a create table statement with a column that has multiple constraints applied of varying kinds.
        /// </summary>
        [TestMethod]
        public void TestCreateTable_WithColumn_Multiple_Constraints()
        {
            string commandText = @"CREATE TABLE [dbo].[NewTable](ColumnA INT NOT NULL CONSTRAINT my_default DEFAULT 1 CONSTRAINT my_fk FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON DELETE SET NULL ON UPDATE NO ACTION CONSTRAINT my_unique UNIQUE)";
            assertCanReproduce(commandText);
        }



        #endregion

        #region Alter

        /// <summary>
        /// This sees whether we can reproduce a simple alter database name statement.
        /// </summary>
        [TestMethod]
        public void TestAlterDatabase_ModifyName()
        {
            string commandText = "ALTER DATABASE NewDatabase MODIFY NAME = RenamedDatabase";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a simple alter database collation name statement.
        /// </summary>
        [TestMethod]
        public void TestAlterDatabase_Collate()
        {
            string commandText = "ALTER DATABASE NewDatabase COLLATE Latin_General";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a simple alter table column statement.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_DataType()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn INT";
            assertCanReproduce(commandText);
        }


        /// <summary>
        /// This sees whether we can reproduce an alter table column.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_DataType_Size_And_Collate_And_NotNull()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn VARCHAR(10) COLLATE latin_general NOT NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - add rowguidcol.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Add_RowGuidCol()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn ADD ROWGUIDCOL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - add Persisted.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Add_Persisted()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn ADD PERSISTED";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - add not for replication.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Add_Not_For_Replication()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn ADD NOT FOR REPLICATION";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a simple alter table column - add Sparse.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Add_Not_For_Sparse()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn ADD SPARSE";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - drop ROWGUIDCOL.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Drop_RowGuidCol()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn DROP ROWGUIDCOL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - drop Persisted.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Drop_Persisted()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn DROP PERSISTED";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - drop not for replication.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Drop_Not_For_Replication()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn DROP NOT FOR REPLICATION";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table column - drop Sparse.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AlterColumn_Drop_Not_For_Sparse()
        {
            string commandText = "ALTER TABLE MyTable ALTER COLUMN mycolumn DROP SPARSE";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table add column.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AddColumn()
        {
            string commandText = "ALTER TABLE MyTable ADD mycolumn VARCHAR(100) NOT NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table add columns.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_AddColumns()
        {
            string commandText = "ALTER TABLE MyTable ADD mycolumn VARCHAR(100) NOT NULL, myothercolumn INT NOT NULL PRIMARY KEY";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table drop column.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_DropColumn()
        {
            string commandText = "ALTER TABLE MyTable DROP COLUMN mycolumn";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table drop columns.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_DropColumns()
        {
            string commandText = "ALTER TABLE MyTable DROP COLUMN mycolumn, myothercolumn, andanothercolumn";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table drop constraint.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_DropConstraint()
        {
            string commandText = "ALTER TABLE MyTable DROP CONSTRAINT myconstraint";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table drop constraint with no constraint keyword.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_DropConstraint_NoConstraintKeyword()
        {
            string commandText = "ALTER TABLE MyTable DROP myconstraint";
            string assertText = "ALTER TABLE MyTable DROP CONSTRAINT myconstraint";
            CommandBuilder builder = new CommandBuilder();
            ICommand command = builder.GetCommand(commandText);
            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(command);
            Assert.AreEqual(assertText, actual, "The command builder did not generate the original command text.");
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table drop constraint.
        /// </summary>
        [TestMethod]
        public void TestAlterTable_DropConstraints()
        {
            string commandText = "ALTER TABLE MyTable DROP CONSTRAINT myconstraint, myotherconstraint, andanotherconstraint";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an alter table statement that drops columns and constraints
        /// </summary>
        [TestMethod]
        public void TestAlterTable_DropColumns_And_Constraints()
        {
            string commandText = "ALTER TABLE MyTable DROP CONSTRAINT myconstraint, myotherconstraint, COLUMN andacolumn, andanothercolumn, CONSTRAINT finalconstraint, COLUMN finalcolumn";
            assertCanReproduce(commandText);
        }

        #endregion

        private void assertCanReproduce(string commandText, CommandBuilderOptions options = null)
        {
            CommandBuilder builder = new CommandBuilder();
            ICommand command = builder.GetCommand(commandText, options);
            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(command);
            Assert.AreEqual(commandText, actual, "The command builder did not generate the original command text.");
        }

        #region Real World Examples

        /// <summary>
        /// This sees whether we can create a SelectBuilder and add to the where clause.
        /// </summary>
        [TestMethod]
        public void TestSelect_AddFilter()
        {
            const string commandText = "SELECT * FROM Customer";
            CommandBuilder commandBuilder = new CommandBuilder();
            SelectBuilder select = (SelectBuilder)commandBuilder.GetCommand(commandText);
            Column customerId = select.Sources["Customer"].Column("CustomerId");
            customerId.Qualify = false;
            Placeholder parameter = new Placeholder("@customerId");
            select.AddWhere(new EqualToFilter(customerId, parameter));

            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(select);
            string expected = "SELECT * FROM Customer WHERE CustomerId = @customerId";
            Assert.AreEqual(expected, actual, "The SELECT statement was not updated as expected.");
        }

        /// <summary>
        /// This sees whether we can reproduce a statement with leading and trailing whitespace.
        /// </summary>
        [TestMethod]
        public void TestSelect_ExtraWhitespace()
        {
            string commandText = "   SELECT  *     FROM    Customer    ";
            CommandBuilder commandBuilder = new CommandBuilder();
            SelectBuilder select = (SelectBuilder)commandBuilder.GetCommand(commandText);

            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(select);
            string expected = "SELECT * FROM Customer";
            Assert.AreEqual(expected, actual, "The SELECT statement was not updated as expected.");
        }

        /// <summary>
        /// This sees whether we can reproduce a statement with newlines.
        /// </summary>
        [TestMethod]
        public void TestSelect_Newlines()
        {
            string commandText =
@"SELECT
    *
FROM Table1
INNER JOIN Table2 ON Table1.TestCol = Table2.TestCol
WHERE Column3 = '123'";
            CommandBuilder commandBuilder = new CommandBuilder();
            SelectBuilder select = (SelectBuilder)commandBuilder.GetCommand(commandText);

            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(select);
            string expected = "SELECT * FROM Table1 INNER JOIN Table2 ON Table1.TestCol = Table2.TestCol WHERE Column3 = '123'";
            Assert.AreEqual(expected, actual, "The SELECT statement was not updated as expected.");
        }

        /// <summary>
        /// This sees whether we can reproduce an extremely complex statement.
        /// </summary>
        [TestMethod]
        public void TestSelect_ComplexCommand()
        {
            string commandText =
@"SELECT
	ra.RouteId,
    ra.RouteNumber,
	o.CustomerId,
	o.CustomerKey AS [Outlet#],
	o.Name AS CustomerName,
	vm.VendingMachineId,
	vm.AssetNumber AS [Equipment#],
	m.ModelType AS Model,
	rc.FillFrequency,
	rc.EffectiveDate AS SettlementDate,
	p.ProductLookupId,
	p.ProductSKU AS ProductCode,
	rcvc.FillLevel AS ProductCapacity,
	st.QuantityDelivered AS FillUnits
FROM Company b
INNER JOIN Route ra ON b.CompanyId = ra.CompanyId
INNER JOIN RouteSchedule rs ON ra.RouteId = rs.RouteId
INNER JOIN RouteCard rc ON rs.RouteScheduleId = rc.RouteScheduleId
INNER JOIN
(
	SELECT
		rc.RouteCardId,
		rcvc.ProductLookupId,
		SUM(rcvc.FillLevel) AS FillLevel
	FROM RouteSchedule rs
	INNER JOIN RouteCard rc ON rs.RouteScheduleId = rc.RouteScheduleId
	INNER JOIN RouteCardVendColumn rcvc ON rc.RouteCardId = rcvc.RouteCardId
	WHERE rs.RouteId IN (1, 2, 3) AND rc.EffectiveDate BETWEEN @startDate AND @stopDate
	GROUP BY rc.RouteCardId, rcvc.ProductLookupId
) as rcvc ON rc.RouteCardId = rcvc.RouteCardId
INNER JOIN ProductLookup p ON rcvc.ProductLookupId = p.ProductLookupId
INNER JOIN VendingMachine vm ON rc.VendingMachineId = vm.VendingMachineId
INNER JOIN MachineTypeLookup m ON vm.MachineTypeLookupId = m.MachineTypeLookupId
INNER JOIN Customer o ON vm.CustomerId = o.CustomerId
INNER JOIN ServiceTransaction svc ON
	(rc.VendingMachineId = svc.VendingMachineId AND rc.EffectiveDate = svc.ServiceTransactionDate)
INNER JOIN SettlementTransactionSKU st ON
	(svc.ServiceTransactionId = st.ServiceTransactionId AND p.ProductLookupId = st.ProductLookupId)
WHERE rc.EffectiveDate BETWEEN @startDate AND @endDate AND ra.RouteId IN (1, 2, 3)
ORDER BY b.CompanyId, ra.RouteId, vm.VendingMachineId, p.ProductLookupId, rc.EffectiveDate DESC";
            CommandBuilder commandBuilder = new CommandBuilder();
            SelectBuilder select = (SelectBuilder)commandBuilder.GetCommand(commandText);
            select.AddWhere(new EqualToFilter(new NumericLiteral(1), new NumericLiteral(1)));

            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(select);
            string expected = "SELECT"
                + " ra.RouteId,"
                + " ra.RouteNumber,"
                + " o.CustomerId,"
                + " o.CustomerKey AS [Outlet#],"
                + " o.Name AS CustomerName,"
                + " vm.VendingMachineId,"
                + " vm.AssetNumber AS [Equipment#],"
                + " m.ModelType AS Model,"
                + " rc.FillFrequency,"
                + " rc.EffectiveDate AS SettlementDate,"
                + " p.ProductLookupId,"
                + " p.ProductSKU AS ProductCode,"
                + " rcvc.FillLevel AS ProductCapacity,"
                + " st.QuantityDelivered AS FillUnits"
                + " FROM Company b"
                + " INNER JOIN Route ra ON b.CompanyId = ra.CompanyId"
                + " INNER JOIN RouteSchedule rs ON ra.RouteId = rs.RouteId"
                + " INNER JOIN RouteCard rc ON rs.RouteScheduleId = rc.RouteScheduleId"
                + " INNER JOIN (SELECT rc.RouteCardId, rcvc.ProductLookupId, SUM(rcvc.FillLevel) AS FillLevel FROM RouteSchedule rs INNER JOIN RouteCard rc ON rs.RouteScheduleId = rc.RouteScheduleId INNER JOIN RouteCardVendColumn rcvc ON rc.RouteCardId = rcvc.RouteCardId WHERE rs.RouteId IN (1, 2, 3) AND rc.EffectiveDate BETWEEN @startDate AND @stopDate GROUP BY rc.RouteCardId, rcvc.ProductLookupId) rcvc ON rc.RouteCardId = rcvc.RouteCardId"
                + " INNER JOIN ProductLookup p ON rcvc.ProductLookupId = p.ProductLookupId"
                + " INNER JOIN VendingMachine vm ON rc.VendingMachineId = vm.VendingMachineId"
                + " INNER JOIN MachineTypeLookup m ON vm.MachineTypeLookupId = m.MachineTypeLookupId"
                + " INNER JOIN Customer o ON vm.CustomerId = o.CustomerId"
                + " INNER JOIN ServiceTransaction svc ON (rc.VendingMachineId = svc.VendingMachineId AND rc.EffectiveDate = svc.ServiceTransactionDate)"
                + " INNER JOIN SettlementTransactionSKU st ON (svc.ServiceTransactionId = st.ServiceTransactionId AND p.ProductLookupId = st.ProductLookupId)"
                + " WHERE rc.EffectiveDate BETWEEN @startDate AND @endDate AND ra.RouteId IN (1, 2, 3) AND 1 = 1"
                + " ORDER BY b.CompanyId, ra.RouteId, vm.VendingMachineId, p.ProductLookupId, rc.EffectiveDate DESC";
            Assert.AreEqual(expected, actual, "The SELECT statement was not reproduced as expected.");
        }

        #endregion

        #region Bug Fixes

        /// <summary>
        /// @dazinator discovered that the NOT was not being persisted within a IS NULL filter.
        /// </summary>
        [TestMethod]
        public void TestNullFilter_NotPersistsRoundtrip()
        {
            string commandText = "SELECT CustomerId, FirstName, LastName, Created FROM Customer WHERE FirstName IS NOT NULL";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// @dazinator discovered a regression where a column in the WHERE filter was being parsed as a PlaceHolder rather than a Column.
        /// See 
        /// </summary>
        [TestMethod]
        public void TestDelete_WhereClause_ColumnNotPlaceHolder()
        {
            string commandText = "DELETE FROM [Table] WHERE TableId = 123";
            CommandBuilder builder = new CommandBuilder();
            ICommand command = builder.GetCommand(commandText, null);
            var deleteCommand = (DeleteBuilder)command;

            var filter = (EqualToFilter)deleteCommand.Where.First();
            Assert.IsInstanceOfType(filter.LeftHand, typeof(Column));
        }

        #endregion

        #region FilterGroup

        /// <summary>
        /// Under a few circumstances, a filter can be optimized/simplified
        /// by combining sibling and child filter groups that have the same
        /// conjunction.
        /// </summary>
        [TestMethod]
        public void TestFilterGroup_Optimize_SimplifiesConditions()
        {
            FilterGroup topFilter = new FilterGroup(Conjunction.Or,
                    new FilterGroup(Conjunction.And,
                        new EqualToFilter(new Column("FirstName"), new StringLiteral("Albert")),
                        new FilterGroup(Conjunction.And,
                            new EqualToFilter(new Column("LastName"), new StringLiteral("Einstein")))),
                    new FilterGroup(Conjunction.And,
                        new EqualToFilter(new Column("FirstName"), new StringLiteral("Max")),
                        new FilterGroup(Conjunction.And,
                            new EqualToFilter(new Column("LastName"), new StringLiteral("Planck")))));

            wrapInParentheses(topFilter, true);

            SelectBuilder selectBuilder = new SelectBuilder();
            selectBuilder.AddTable(new Table("Person"));
            selectBuilder.AddProjection(new Column("FirstName"));
            selectBuilder.AddProjection(new Column("LastName"));
            selectBuilder.AddWhere(topFilter);
            Formatter formatter = new Formatter();
            string beforeActual = formatter.GetCommandText(selectBuilder);
            const string beforeExpected = "SELECT FirstName, LastName FROM Person WHERE (((FirstName = 'Albert') AND ((LastName = 'Einstein'))) OR ((FirstName = 'Max') AND ((LastName = 'Planck'))))";
            Assert.AreEqual(beforeExpected, beforeActual, "The initial query had an unexpected string representation.");

            wrapInParentheses(topFilter, false);
            topFilter.Optimize();
            wrapInParentheses(topFilter, true);

            string afterActual = formatter.GetCommandText(selectBuilder, new CommandOptions() { WrapFiltersInParentheses = true });
            const string afterExpected = "SELECT FirstName, LastName FROM Person WHERE (((FirstName = 'Albert') AND (LastName = 'Einstein')) OR ((FirstName = 'Max') AND (LastName = 'Planck')))";
            Assert.AreEqual(afterExpected, afterActual, "The optimized query had an unexpected string representation.");
        }

        private void wrapInParentheses(IFilter filter, bool wrap)
        {
            filter.WrapInParentheses = wrap;
            FilterGroup group = filter as FilterGroup;
            if (group != null)
            {
                foreach (IFilter innerFilter in group.Filters)
                {
                    wrapInParentheses(innerFilter, wrap);
                }
            }
        }

        #endregion
    }
}
