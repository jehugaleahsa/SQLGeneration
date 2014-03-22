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

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_Select()
        {
            string commandText = "SELECT Column FROM Table WHERE Column > ALL (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_ValueList()
        {
            string commandText = "SELECT Column FROM Table WHERE Column > ALL (1, 2, 3)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a BETWEEN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_BetweenFilter()
        {
            string commandText = "SELECT Column FROM Table WHERE Column BETWEEN 1 AND 10";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a LIKE filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_LikeFilter()
        {
            string commandText = "SELECT Column FROM Table WHERE Column LIKE '%ABC'";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a LIKE filter
        /// that sees if a column is like another column.
        /// </summary>
        [TestMethod]
        public void TestSelect_LikeFilter_CompareTwoColumns()
        {
            string commandText = "SELECT Column FROM Table WHERE Column1 LIKE Column2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a LIKE filter
        /// that sees if a column is like a parameter.
        /// </summary>
        [TestMethod]
        public void TestSelect_LikeFilter_CompareToParameter()
        {
            string commandText = "SELECT Column FROM Table WHERE Column1 LIKE @Parameter";
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
            string commandText = "SELECT Column FROM Table WHERE Column IN (1, 2, 3)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an IN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_InFilter_Select()
        {
            string commandText = "SELECT Column FROM Table1 WHERE Column IN (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an IN filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_InFilter_FunctionCall()
        {
            string commandText = "SELECT Column FROM Table WHERE Column IN GetData()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with an EXISTS filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_ExistsFilter()
        {
            string commandText = "SELECT Column FROM Table1 WHERE EXISTS(SELECT 1 FROM Table2 WHERE Table1.Column = Table2.Column)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_Any()
        {
            string commandText = "SELECT Column FROM Table WHERE Column > ANY (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_Some()
        {
            string commandText = "SELECT Column FROM Table WHERE Column > SOME (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_EqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column = ALL (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_NotEqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column <> ALL (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_LessThanEqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column <= ALL (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_GreaterThanEqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column >= ALL (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a quantifying filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_QuantifyingFilter_LessThan()
        {
            string commandText = "SELECT Column FROM Table WHERE Column < ALL (SELECT Column FROM Table2)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_NotEqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column <> 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_LessThanEqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column <= 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_GreaterThanEqualTo()
        {
            string commandText = "SELECT Column FROM Table WHERE Column >= 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_LessThan()
        {
            string commandText = "SELECT Column FROM Table WHERE Column < 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select with a order comparison filter.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderFilter_GreaterThan()
        {
            string commandText = "SELECT Column FROM Table WHERE Column > 123";
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
            string commandText = "SELECT Column1, Column2, Column3 FROM Table ORDER BY Column1, Column2, Column3";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column in descending order.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByDescending()
        {
            string commandText = "SELECT Column FROM Table ORDER BY Column DESC";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column in descending order.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByAscending()
        {
            string commandText = "SELECT Column FROM Table ORDER BY Column ASC";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column with nulls first.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByNullsFirst()
        {
            string commandText = "SELECT Column FROM Table ORDER BY Column NULLS FIRST";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column with nulls last.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByNullsLast()
        {
            string commandText = "SELECT Column FROM Table ORDER BY Column NULLS LAST";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that orders a column descending with nulls first.
        /// </summary>
        [TestMethod]
        public void TestSelect_OrderByDescendingNullsLast()
        {
            string commandText = "SELECT Column FROM Table ORDER BY Column DESC NULLS FIRST";
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
            string commandText = "SELECT Server.Database.Owner.Table.Column FROM Server.Database.Owner.Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a select that doesn't qualify a column when multiple tables are present.
        /// </summary>
        [TestMethod]
        public void TestSelect_UnqualifiedColumn_MultipleSources()
        {
            string commandText = "SELECT Column FROM Table1, Table2";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We should be able to build a function that applies ROW_NUMBER to each row to
        /// return a window.
        /// </summary>
        [TestMethod]
        public void TestSelect_FunctionWithOrderingWindow()
        {
            string commandText = "SELECT inner.c1 FROM (SELECT Table.Column1 AS c1, ROW_NUMBER() OVER (ORDER BY Table.Column2, Table.Column3) AS rn FROM Table) inner WHERE inner.rn BETWEEN 11 AND 20";
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
            string commandText = "SELECT CASE Table.Column WHEN 0 THEN 'Sunday' WHEN 1 THEN 'Monday' WHEN 2 THEN 'Tuesday' WHEN 3 THEN 'Wednesday' WHEN 4 THEN 'Thursday' WHEN 5 THEN 'Friday' WHEN 6 THEN 'Saturday' END FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with an ELSE branch.
        /// </summary>
        [TestMethod]
        public void TestSelect_MatchCase_Else()
        {
            string commandText = "SELECT CASE Table.Column WHEN 'Admin' THEN 'Administrator' ELSE 'User' END FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with just one case.
        /// </summary>
        [TestMethod]
        public void TestSelect_ConditionalCase_MultipleCases()
        {
            string commandText = "SELECT CASE WHEN Table.Column = 0 THEN 'Sunday' WHEN Table.Column = 1 THEN 'Monday' WHEN Table.Column = 2 THEN 'Tuesday' WHEN Table.Column = 3 THEN 'Wednesday' WHEN Table.Column = 4 THEN 'Thursday' WHEN Table.Column = 5 THEN 'Friday' WHEN Table.Column = 6 THEN 'Saturday' END FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// We can create a CASE expression with an ELSE branch.
        /// </summary>
        [TestMethod]
        public void TestSelect_ConditionalCase_Else()
        {
            string commandText = "SELECT CASE WHEN Table.Column = 'Admin' THEN 'Administrator' ELSE 'User' END FROM Table";
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

        #endregion

        #region Insert

        /// <summary>
        /// This sees whether we can reproduce a simple insert statement.
        /// </summary>
        [TestMethod]
        public void TestInsert_NoColumns_NoValues()
        {
            string commandText = "INSERT INTO Table VALUES()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement where the table is aliased.
        /// </summary>
        [TestMethod]
        public void TestInsert_AliasedTable()
        {
            string commandText = "INSERT INTO Table t VALUES()";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement with columns and values listed.
        /// </summary>
        [TestMethod]
        public void TestInsert_ColumnsAndValues()
        {
            string commandText = "INSERT INTO Table (Column1, Column2, Column3) VALUES(123, 'hello', NULL)";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an insert statement whose values comes from a select statement.
        /// </summary>
        [TestMethod]
        public void TestInsert_ColumnsAndSelect()
        {
            string commandText = "INSERT INTO Table (Column1, Column2, Column3) (SELECT 123, 'hello', NULL)";
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
            string commandText = "UPDATE Table SET Column = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with an aliased table.
        /// </summary>
        [TestMethod]
        public void TestUpdate_AliasedTable()
        {
            string commandText = "UPDATE Table t SET Column = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with a where clause.
        /// </summary>
        [TestMethod]
        public void TestUpdate_WhereClause()
        {
            string commandText = "UPDATE Table SET Column2 = 'hello' WHERE Column1 = 123";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce an update statement with multiple setters.
        /// </summary>
        [TestMethod]
        public void TestUpdate_MultipleSetters()
        {
            string commandText = "UPDATE Table SET Column2 = 'hello', Column3 = NULL WHERE Column1 = 123";
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
            string commandText = "DELETE FROM Table";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a delete statement with an aliased table.
        /// </summary>
        [TestMethod]
        public void TestDelete_AliasedTable()
        {
            string commandText = "DELETE FROM Table t";
            assertCanReproduce(commandText);
        }

        /// <summary>
        /// This sees whether we can reproduce a delete statement with where clause.
        /// </summary>
        [TestMethod]
        public void TestDelete_WhereClause()
        {
            string commandText = "DELETE FROM Table WHERE Column = 123";
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
INNER JOIN Table2 ON Table1.Column = Table2.Column
WHERE Column3 = '123'";
            CommandBuilder commandBuilder = new CommandBuilder();
            SelectBuilder select = (SelectBuilder)commandBuilder.GetCommand(commandText);

            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(select);
            string expected = "SELECT * FROM Table1 INNER JOIN Table2 ON Table1.Column = Table2.Column WHERE Column3 = '123'";
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
	r.RouteId,
    r.RouteNumber,
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
INNER JOIN Route r ON b.CompanyId = r.CompanyId
INNER JOIN RouteSchedule rs ON r.RouteId = rs.RouteId
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
WHERE rc.EffectiveDate BETWEEN @startDate AND @endDate AND r.RouteId IN (1, 2, 3)
ORDER BY b.CompanyId, r.RouteId, vm.VendingMachineId, p.ProductLookupId, rc.EffectiveDate DESC";
            CommandBuilder commandBuilder = new CommandBuilder();
            SelectBuilder select = (SelectBuilder)commandBuilder.GetCommand(commandText);
            select.AddWhere(new EqualToFilter(new NumericLiteral(1), new NumericLiteral(1)));

            Formatter formatter = new Formatter();
            string actual = formatter.GetCommandText(select);
            string expected = "SELECT"
                + " r.RouteId,"
                + " r.RouteNumber,"
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
                + " INNER JOIN Route r ON b.CompanyId = r.CompanyId"
                + " INNER JOIN RouteSchedule rs ON r.RouteId = rs.RouteId"
                + " INNER JOIN RouteCard rc ON rs.RouteScheduleId = rc.RouteScheduleId"
                + " INNER JOIN (SELECT rc.RouteCardId, rcvc.ProductLookupId, SUM(rcvc.FillLevel) AS FillLevel FROM RouteSchedule rs INNER JOIN RouteCard rc ON rs.RouteScheduleId = rc.RouteScheduleId INNER JOIN RouteCardVendColumn rcvc ON rc.RouteCardId = rcvc.RouteCardId WHERE rs.RouteId IN (1, 2, 3) AND rc.EffectiveDate BETWEEN @startDate AND @stopDate GROUP BY rc.RouteCardId, rcvc.ProductLookupId) rcvc ON rc.RouteCardId = rcvc.RouteCardId"
                + " INNER JOIN ProductLookup p ON rcvc.ProductLookupId = p.ProductLookupId"
                + " INNER JOIN VendingMachine vm ON rc.VendingMachineId = vm.VendingMachineId"
                + " INNER JOIN MachineTypeLookup m ON vm.MachineTypeLookupId = m.MachineTypeLookupId"
                + " INNER JOIN Customer o ON vm.CustomerId = o.CustomerId"
                + " INNER JOIN ServiceTransaction svc ON (rc.VendingMachineId = svc.VendingMachineId AND rc.EffectiveDate = svc.ServiceTransactionDate)"
                + " INNER JOIN SettlementTransactionSKU st ON (svc.ServiceTransactionId = st.ServiceTransactionId AND p.ProductLookupId = st.ProductLookupId)"
                + " WHERE rc.EffectiveDate BETWEEN @startDate AND @endDate AND r.RouteId IN (1, 2, 3) AND 1 = 1"
                + " ORDER BY b.CompanyId, r.RouteId, vm.VendingMachineId, p.ProductLookupId, rc.EffectiveDate DESC";
            Assert.AreEqual(expected, actual, "The SELECT statement was not reproduced as expected.");
        }

        #endregion

        #region Bug Fixes

        /// <summary>
        /// @danizator discovered that the NOT was not being persisted within a IS NULL filter.
        /// </summary>
        [TestMethod]
        public void TestNullFilter_NotPersistsRoundtrip()
        {
            string commandText = "SELECT CustomerId, FirstName, LastName, Created FROM Customer WHERE FirstName IS NOT NULL";
            assertCanReproduce(commandText);
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
