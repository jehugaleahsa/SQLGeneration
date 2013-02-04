using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGeneration.Parsing;
using System.Text;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests that builder constructs work in tandem.
    /// </summary>
    [TestClass]
    public class CompoundTester
    {
        /// <summary>
        /// We should verify that the right tokens are being generated for a complex SELECT statement.
        /// </summary>
        [TestMethod]
        public void TestSelectBuilder()
        {
            Table table1 = new Table("Table1");
            Table table2 = new Table("Table2");

            SelectBuilder builder = new SelectBuilder();
            builder.AddJoin(
                Join.From(table1, "t1")
                    .InnerJoin(table2, "t2").On(j => j.Sources["t1"].Column("c1").EqualTo(j.Sources["t2"].Column("c1"))));
            builder.AddProjection(builder.Sources["t1"].Column("c1"));
            builder.AddProjection(builder.Sources["t2"].Column("c1"), "t2c1");

            string output = String.Join(" ", builder.GetCommandTokens(new CommandOptions()));

            SqlTokenizer tokenizer = new SqlTokenizer();
            SqlGrammar grammar = new SqlGrammar(tokenizer);
            Parser parser = new Parser(grammar);

            parser.RegisterHandler(SqlGrammar.ArithmeticExpression.Name, generateArithmeticExpression);
            parser.RegisterHandler(SqlGrammar.ColumnList.Name, generateColumnList);
            parser.RegisterHandler(SqlGrammar.DeleteStatement.Name, generateDeleteStatement);
            parser.RegisterHandler(SqlGrammar.Filter.Name, generateFilter);
            parser.RegisterHandler(SqlGrammar.FilterList.Name, generateFilterList);
            parser.RegisterHandler(SqlGrammar.FromList.Name, generateFromList);
            parser.RegisterHandler(SqlGrammar.FunctionCall.Name, generateFunctionCall);
            parser.RegisterHandler(SqlGrammar.GroupByList.Name, generateGroupByList);
            parser.RegisterHandler(SqlGrammar.InsertStatement.Name, generateInsertStatement);
            parser.RegisterHandler(SqlGrammar.Item.Name, generateItem);
            parser.RegisterHandler(SqlGrammar.Join.Name, generateJoin);
            parser.RegisterHandler(SqlGrammar.SelectStatement.Name, generateSelectStatement);
            parser.RegisterHandler(SqlGrammar.Start.Name, generateSqlStatement);

            ITokenSource tokenSource = tokenizer.CreateTokenSource(builder.GetCommandTokens(new CommandOptions()));
            object result = parser.Parse(SqlGrammar.Start.Name, tokenSource);
        }

        private static object generateArithmeticExpression(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ArithmeticExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult first = multiple.Matches[SqlGrammar.ArithmeticExpression.Multiple.First];
                builder.Append(first.Context);
                builder.Append(' ');
                MatchResult arithmeticOperator = multiple.Matches[SqlGrammar.ArithmeticExpression.Multiple.ArithemeticOperator];
                builder.Append(arithmeticOperator.Context);
                builder.Append(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ArithmeticExpression.Multiple.Remaining];
                builder.Append(remaining.Context);
            }
            MatchResult single = result.Matches[SqlGrammar.ArithmeticExpression.Single];
            return single.Context;
        }

        private static object generateColumnList(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ColumnList.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult first = multiple.Matches[SqlGrammar.ColumnList.Multiple.First];
                builder.Append(first.Context);
                MatchResult comma = multiple.Matches[SqlGrammar.ColumnList.Multiple.Comma];
                builder.Append(comma.Context);
                builder.Append(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ColumnList.Multiple.Remaining];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult single = result.Matches[SqlGrammar.ColumnList.Single];
            return single.Context;
        }

        private static object generateDeleteStatement(MatchResult result)
        {
            StringBuilder builder = new StringBuilder();
            MatchResult delete = result.Matches[SqlGrammar.DeleteStatement.DeleteKeyword];
            builder.Append(delete.Context);
            builder.Append(' ');
            MatchResult from = result.Matches[SqlGrammar.DeleteStatement.FromKeyword];
            if (from.IsMatch)
            {
                builder.Append(from.Context);
                builder.Append(' ');
            }
            MatchResult table = result.Matches[SqlGrammar.DeleteStatement.Table];
            builder.Append(table.Context);
            MatchResult where = result.Matches[SqlGrammar.DeleteStatement.Where.Name];
            if (where.IsMatch)
            {
                builder.Append(' ');
                MatchResult whereKeyword = result.Matches[SqlGrammar.DeleteStatement.Where.WhereKeyword];
                builder.Append(whereKeyword.Context);
                builder.Append(' ');
                MatchResult filterList = result.Matches[SqlGrammar.DeleteStatement.Where.FilterList];
                builder.Append(filterList.Context);
            }
            return builder.ToString();
        }

        private static object generateFilter(MatchResult result)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Filter.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.Filter.Wrapped.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult filter = wrapped.Matches[SqlGrammar.Filter.Wrapped.Filter];
                builder.Append(filter.Context);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.Filter.Wrapped.RightParenthesis];
                builder.Append(rightParenthesis.Context);
                return builder.ToString();
            }
            MatchResult not = result.Matches[SqlGrammar.Filter.Not.Name];
            if (not.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult notKeyword = not.Matches[SqlGrammar.Filter.Not.NotKeyword];
                builder.Append(notKeyword.Context);
                builder.Append(' ');
                MatchResult filter = not.Matches[SqlGrammar.Filter.Not.Filter];
                builder.Append(filter.Context);
                return builder.ToString();
            }
            MatchResult order = result.Matches[SqlGrammar.Filter.Order.Name];
            if (order.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult left = order.Matches[SqlGrammar.Filter.Order.Left];
                builder.Append(left.Context);
                builder.Append(' ');
                MatchResult comparisonOperator = order.Matches[SqlGrammar.Filter.Order.ComparisonOperator];
                builder.Append(comparisonOperator.Context);
                builder.Append(' ');
                MatchResult right = order.Matches[SqlGrammar.Filter.Order.Right];
                builder.Append(right.Context);
                return builder.ToString();
            }
            MatchResult between = result.Matches[SqlGrammar.Filter.Between.Name];
            if (between.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult expression = between.Matches[SqlGrammar.Filter.Between.Expression];
                builder.Append(expression.Context);
                builder.Append(' ');
                MatchResult notKeyword = between.Matches[SqlGrammar.Filter.Between.Not];
                if (notKeyword.IsMatch)
                {
                    builder.Append(notKeyword.Context);
                    builder.Append(' ');
                }
                MatchResult betweenKeyword = between.Matches[SqlGrammar.Filter.Between.BetweenKeyword];
                builder.Append(betweenKeyword.Context);
                builder.Append(' ');
                MatchResult lowerBound = between.Matches[SqlGrammar.Filter.Between.LowerBound];
                builder.Append(lowerBound.Context);
                MatchResult andKeyword = between.Matches[SqlGrammar.Filter.Between.And];
                builder.Append(andKeyword.Context);
                builder.Append(' ');
                MatchResult upperBound = between.Matches[SqlGrammar.Filter.Between.UpperBound];
                builder.Append(upperBound.Context);
                return builder.ToString();
            }
            MatchResult like = result.Matches[SqlGrammar.Filter.Like.Name];
            if (like.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult expression = like.Matches[SqlGrammar.Filter.Like.Expression];
                builder.Append(expression.Context);
                builder.Append(' ');
                MatchResult notKeyword = like.Matches[SqlGrammar.Filter.Like.Not];
                if (notKeyword.IsMatch)
                {
                    builder.Append(notKeyword.Context);
                    builder.Append(' ');
                }
                MatchResult likeKeyword = like.Matches[SqlGrammar.Filter.Like.LikeKeyword];
                builder.Append(likeKeyword.Context);
                builder.Append(' ');
                MatchResult value = like.Matches[SqlGrammar.Filter.Like.Value];
                builder.Append(value.Context);
                return builder.ToString();
            }
            MatchResult isResult = result.Matches[SqlGrammar.Filter.Is.Name];
            if (isResult.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult expression = isResult.Matches[SqlGrammar.Filter.Is.Expression];
                builder.Append(expression.Context);
                builder.Append(' ');
                MatchResult isKeyword = isResult.Matches[SqlGrammar.Filter.Is.IsKeyword];
                builder.Append(isKeyword.Context);
                builder.Append(' ');
                MatchResult notKeyword = isResult.Matches[SqlGrammar.Filter.Is.Not];
                if (notKeyword.IsMatch)
                {
                    builder.Append(notKeyword.Context);
                    builder.Append(' ');
                }
                MatchResult nullKeyword = isResult.Matches[SqlGrammar.Filter.Is.Null];
                builder.Append(nullKeyword.Context);
                return builder.ToString();
            }
            MatchResult inResult = result.Matches[SqlGrammar.Filter.In.Name];
            if (inResult.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult expression = inResult.Matches[SqlGrammar.Filter.In.Name];
                builder.Append(expression.Context);
                builder.Append(' ');
                MatchResult notKeyword = inResult.Matches[SqlGrammar.Filter.In.Not];
                if (notKeyword.IsMatch)
                {
                    builder.Append(notKeyword.Context);
                    builder.Append(' ');
                }
                MatchResult inKeyword = inResult.Matches[SqlGrammar.Filter.In.InKeyword];
                builder.Append(inKeyword.Context);
                builder.Append(' ');
                MatchResult leftParenthesis = inResult.Matches[SqlGrammar.Filter.In.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult selectExpression = inResult.Matches[SqlGrammar.Filter.In.SelectExpression];
                if (selectExpression.IsMatch)
                {
                    builder.Append(selectExpression.Context);
                }
                MatchResult valueList = inResult.Matches[SqlGrammar.Filter.In.ValueList];
                if (valueList.IsMatch)
                {
                    builder.Append(valueList.Context);
                }
                MatchResult rightParenthesis = inResult.Matches[SqlGrammar.Filter.In.RightParenthesis];
                builder.Append(rightParenthesis.Context);
                return builder.ToString();
            }
            return null;
        }

        private static object generateFilterList(MatchResult result)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.FilterList.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult notKeyword = wrapped.Matches[SqlGrammar.FilterList.Wrapped.Not];
                if (notKeyword.IsMatch)
                {
                    builder.Append(notKeyword.Context);
                    builder.Append(' ');
                }
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.FilterList.Wrapped.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult filterList = wrapped.Matches[SqlGrammar.FilterList.Wrapped.FilterList];
                builder.Append(filterList.Context);
                MatchResult rightParethesis = wrapped.Matches[SqlGrammar.FilterList.Wrapped.RightParenthesis];
                builder.Append(rightParethesis.Context);
                return builder.ToString();
            }
            MatchResult multiple = result.Matches[SqlGrammar.FilterList.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult first = multiple.Matches[SqlGrammar.FilterList.Multiple.First];
                builder.Append(first.Context);
                builder.Append(' ');
                MatchResult conjunction = multiple.Matches[SqlGrammar.FilterList.Multiple.Conjunction];
                builder.Append(conjunction.Context);
                builder.Append(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.FilterList.Multiple.Remaining];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult single = result.Matches[SqlGrammar.FilterList.Single];
            return single.Context;
        }

        private static object generateFromList(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.FromList.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult first = multiple.Matches[SqlGrammar.FromList.Multiple.Name];
                builder.Append(first.Context);
                MatchResult comma = multiple.Matches[SqlGrammar.FromList.Multiple.Comma];
                builder.Append(comma.Context);
                builder.Append(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.FromList.Multiple.Remaining];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult single = result.Matches[SqlGrammar.FromList.Single];
            return single.Context;
        }

        private static object generateFunctionCall(MatchResult result)
        {
            StringBuilder builder = new StringBuilder();
            MatchResult functionName = result.Matches[SqlGrammar.FunctionCall.FunctionName];
            builder.Append(functionName.Context);
            MatchResult leftParenthesis = result.Matches[SqlGrammar.FunctionCall.LeftParethesis];
            builder.Append(leftParenthesis.Context);
            MatchResult arguments = result.Matches[SqlGrammar.FunctionCall.Arguments];
            builder.Append(arguments);
            MatchResult rightParenthesis = result.Matches[SqlGrammar.FunctionCall.RightParenthesis];
            builder.Append(rightParenthesis);
            return builder.ToString();
        }

        private static object generateGroupByList(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.GroupByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult left = result.Matches[SqlGrammar.GroupByList.Multiple.First];
                builder.Append(left.Context);
                MatchResult comma = result.Matches[SqlGrammar.GroupByList.Multiple.Comma];
                builder.Append(comma.Context);
                builder.Append(' ');
                MatchResult remaining = result.Matches[SqlGrammar.GroupByList.Multiple.Remaining];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult single = result.Matches[SqlGrammar.GroupByList.Single];
            return single.Context;
        }

        private static object generateInsertStatement(MatchResult result)
        {
            StringBuilder builder = new StringBuilder();
            MatchResult insert = result.Matches[SqlGrammar.InsertStatement.InsertKeyword];
            builder.Append(insert.Context);
            builder.Append(' ');
            MatchResult into = result.Matches[SqlGrammar.InsertStatement.IntoKeyword];
            if (into.IsMatch)
            {
                builder.Append(into.Context);
                builder.Append(' ');
            }
            MatchResult table = result.Matches[SqlGrammar.InsertStatement.Table];
            builder.Append(table.Context);
            builder.Append(' ');
            MatchResult columns = result.Matches[SqlGrammar.InsertStatement.Columns.Name];
            if (columns.IsMatch)
            {
                MatchResult leftParenthesis = columns.Matches[SqlGrammar.InsertStatement.Columns.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult columnList = columns.Matches[SqlGrammar.InsertStatement.Columns.ColumnList];
                builder.Append(columnList.Context);
                MatchResult rightParenthesis = columns.Matches[SqlGrammar.InsertStatement.Columns.RightParenthesis];
                builder.Append(rightParenthesis.Context);
                builder.Append(' ');
            }
            MatchResult values = result.Matches[SqlGrammar.InsertStatement.Values.Name];
            if (values.IsMatch)
            {
                MatchResult valuesKeyword = values.Matches[SqlGrammar.InsertStatement.Values.ValuesKeyword];
                builder.Append(valuesKeyword.Context);
                MatchResult leftParenthesis = values.Matches[SqlGrammar.InsertStatement.Values.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult valuesList = values.Matches[SqlGrammar.InsertStatement.Values.ValueList];
                if (valuesList.IsMatch)
                {
                    builder.Append(valuesList.Context);
                }
                MatchResult rightParenthesis = values.Matches[SqlGrammar.InsertStatement.Values.RightParenthesis];
                builder.Append(rightParenthesis.Context);
            }
            MatchResult select = result.Matches[SqlGrammar.InsertStatement.Select.Name];
            if (select.IsMatch)
            {
                MatchResult leftParenthesis = select.Matches[SqlGrammar.InsertStatement.Select.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult selectExpression = select.Matches[SqlGrammar.InsertStatement.Select.SelectExpression];
                builder.Append(selectExpression.Context);
                MatchResult rightParenthesis = select.Matches[SqlGrammar.InsertStatement.Select.RightParenthesis];
                builder.Append(rightParenthesis.Context);
            }
            return builder.ToString();
        }

        private static object generateItem(MatchResult result)
        {
            MatchResult column = result.Matches[SqlGrammar.Item.Column];
            if (column.IsMatch)
            {
                return column.Context;
            }
            MatchResult functionCall = result.Matches[SqlGrammar.Item.FunctionCall];
            if (functionCall.IsMatch)
            {
                return column.Context;
            }
            MatchResult arithmetic = result.Matches[SqlGrammar.Item.ArithmeticExpression];
            if (arithmetic.IsMatch)
            {
                return arithmetic.Context;
            }
            MatchResult select = result.Matches[SqlGrammar.Item.SelectStatement];
            if (select.IsMatch)
            {
                return select.Context;
            }
            MatchResult numberResult = result.Matches[SqlGrammar.Item.Number];
            if (numberResult.IsMatch)
            {
                return numberResult.Context;
            }
            MatchResult stringResult = result.Matches[SqlGrammar.Item.String];
            if (stringResult.IsMatch)
            {
                return stringResult.Context;
            }
            MatchResult nullResult = result.Matches[SqlGrammar.Item.Null];
            if (nullResult.IsMatch)
            {
                return nullResult.Context;
            }
            return null;
        }

        private static object generateJoin(MatchResult result)
        {
            throw new NotImplementedException();
        }

        private object generateSelectStatement(MatchResult result)
        {
            StringBuilder builder = new StringBuilder();
            MatchResult expression = result.Matches[SqlGrammar.SelectStatement.SelectExpression];
            builder.Append(expression.Context);
            MatchResult orderBy = result.Matches[SqlGrammar.SelectStatement.OrderBy.Name];
            if (orderBy.IsMatch)
            {
                builder.Append(' ');
                MatchResult orderByKeyword = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByKeyword];
                builder.Append(orderByKeyword.Context);
                builder.Append(' ');
                MatchResult orderByList = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByList];
                builder.Append(orderByList.Context);
            }
            return builder.ToString();
        }

        private static object generateSqlStatement(MatchResult result)
        {
            MatchResult selectResult = result.Matches[SqlGrammar.Start.SelectStatement];
            if (selectResult.IsMatch)
            {
                return selectResult.Context;
            }
            MatchResult insertResult = result.Matches[SqlGrammar.Start.InsertStatement];
            if (insertResult.IsMatch)
            {
                return insertResult.Context;
            }
            MatchResult updateResult = result.Matches[SqlGrammar.Start.UpdateStatement];
            if (updateResult.IsMatch)
            {
                return updateResult.Context;
            }
            MatchResult deleteResult = result.Matches[SqlGrammar.Start.DeleteStatement];
            return deleteResult.Context;
        }
    }
}
