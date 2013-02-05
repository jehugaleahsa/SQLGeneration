using System;
using System.Collections.Generic;
using System.Text;
using SQLGeneration.Parsing;

namespace SQLGeneration.Generators
{
    /// <summary>
    /// Generates simple SQL text from a token source.
    /// </summary>
    public sealed class SimpleFormatter : SqlResponder
    {
        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to use.</param>
        public SimpleFormatter(SqlTokenizer tokenizer)
            : base(new SqlGrammar(tokenizer))
        {
        }

        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        public SimpleFormatter(SqlGrammar grammar = null)
            : base(grammar)
        {
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <returns>The command text.</returns>
        public string GetCommandText(ICommand command, CommandOptions options = null)
        {
            if (options == null)
            {
                options = new CommandOptions();
            }
            IEnumerable<string> tokenStream = command.GetCommandTokens(options);
            object result = GetResult(tokenStream);
            return (string)result;
        }

        /// <summary>
        /// Generates output for the top-level statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnStart(MatchResult result)
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

        /// <summary>
        /// Generates output for the SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnSelectStatement(MatchResult result)
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

        /// <summary>
        /// Generates output for the SELECT expression.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnSelectExpression(MatchResult result)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.SelectExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.LeftParenthesis];
                MatchResult selectExpression = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.SelectExpression];
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.RightParenthesis];
                return builder.ToString();
            }
            MatchResult specification = result.Matches[SqlGrammar.SelectExpression.SelectSpecification];
            if (specification.IsMatch)
            {
                return specification.Context;
            }
            MatchResult remaining = specification.Matches[SqlGrammar.SelectExpression.Remaining.Name];
            if (remaining.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(' ');
                MatchResult combiner = result.Matches[SqlGrammar.SelectExpression.Remaining.Combiner];
                builder.Append(combiner.Context);
                builder.Append(' ');
                MatchResult selectExpression = result.Matches[SqlGrammar.SelectExpression.Remaining.SelectExpression];
                builder.Append(selectExpression.Context);
                return builder.ToString();
            }
            return null;
        }

        /// <summary>
        /// Generates output for the SELECT specification.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnSelectSpecification(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for the ORDER BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnOrderByList(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for an ORDER BY item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnOrderByItem(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for an arithmetic expression.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnArithmeticExpression(MatchResult result)
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

        /// <summary>
        /// Generates output for the projection list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnProjectionList(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ProjectionList.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult first = multiple.Matches[SqlGrammar.ProjectionList.Multiple.First];
                builder.Append(first.Context);
                MatchResult comma = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Comma];
                builder.Append(comma.Context);
                builder.Append(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Remaining];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult single = result.Matches[SqlGrammar.ProjectionList.Single];
            return single.Context;
        }

        /// <summary>
        /// Generates output for a projection item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnProjectionItem(MatchResult result)
        {
            MatchResult star = result.Matches[SqlGrammar.ProjectionItem.Star.Name];
            if (star.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult qualifier = star.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Name];
                if (qualifier.IsMatch)
                {
                    MatchResult columnSource = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.ColumnSource];
                    builder.Append(columnSource.Context);
                    MatchResult dot = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Dot];
                    builder.Append(dot.Context);
                }
                MatchResult starToken = star.Matches[SqlGrammar.ProjectionItem.Star.StarToken];
                builder.Append(starToken.Context);
                return builder.ToString();
            }
            MatchResult projectionItem = result.Matches[SqlGrammar.ProjectionItem.Expression.Name];
            if (projectionItem.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult item = projectionItem.Matches[SqlGrammar.ProjectionItem.Expression.Item];
                builder.Append(item.Context);
                MatchResult aliasExpression = projectionItem.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Name];
                if (aliasExpression.IsMatch)
                {
                    MatchResult aliasIndicator = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.AliasIndicator];
                    if (aliasIndicator.IsMatch)
                    {
                        builder.Append(' ');
                        builder.Append(aliasIndicator.Context);
                    }
                    builder.Append(' ');
                    MatchResult alias = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Alias];
                    builder.Append(alias.Context);
                }
                return builder.ToString();
            }
            return null;
        }

        /// <summary>
        /// Generates output for the FROM list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnFromList(MatchResult result)
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

        /// <summary>
        /// Generates output for a join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnJoinItem(MatchResult result)
        {
            StringBuilder builder = new StringBuilder();
            MatchResult table = result.Matches[SqlGrammar.JoinItem.Table];
            if (table.IsMatch)
            {
                builder.Append(table.Context);
            }
            MatchResult functionCall = result.Matches[SqlGrammar.JoinItem.FunctionCall];
            if (functionCall.IsMatch)
            {
                builder.Append(functionCall.Context);
            }
            MatchResult select = result.Matches[SqlGrammar.JoinItem.SelectExpression];
            if (select.IsMatch)
            {
                builder.Append(select.Context);
            }
            MatchResult aliasExpression = result.Matches[SqlGrammar.JoinItem.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult aliasIndicator = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.AliasIndicator];
                if (aliasIndicator.IsMatch)
                {
                    builder.Append(' ');
                    builder.Append(aliasIndicator.Context);
                }
                builder.Append(' ');
                MatchResult alias = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.Alias];
                builder.Append(alias.Context);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Generates output for a function call.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnFunctionCall(MatchResult result)
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

        /// <summary>
        /// Generates output for a join.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnJoin(MatchResult result)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Join.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.Join.Wrapped.LeftParenthesis];
                builder.Append(leftParenthesis.Context);
                MatchResult join = wrapped.Matches[SqlGrammar.Join.Wrapped.Join];
                builder.Append(join.Context);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.Join.Wrapped.RightParenthesis];
                builder.Append(rightParenthesis.Context);
                return builder.ToString();
            }
            MatchResult joined = result.Matches[SqlGrammar.Join.Joined.Name];
            if (joined.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult joinItem = joined.Matches[SqlGrammar.Join.Joined.JoinItem];
                builder.Append(joinItem.Context);
                MatchResult joinPrime = joined.Matches[SqlGrammar.Join.Joined.JoinPrime];
                builder.Append(joinPrime.Context);
                return builder.ToString();
            }
            return null;
        }

        /// <summary>
        /// Generates output for the next join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnJoinPrime(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.JoinPrime.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(' ');
                MatchResult joinType = multiple.Matches[SqlGrammar.JoinPrime.Multiple.JoinType];
                builder.Append(joinType.Context);
                builder.Append(' ');
                MatchResult nextItem = multiple.Matches[SqlGrammar.JoinPrime.Multiple.JoinItem];
                builder.Append(nextItem.Context);
                MatchResult onClause = multiple.Matches[SqlGrammar.JoinPrime.Multiple.On.Name];
                if (onClause.IsMatch)
                {
                    builder.Append(' ');
                    MatchResult onKeyword = onClause.Matches[SqlGrammar.JoinPrime.Multiple.On.OnToken];
                    builder.Append(onKeyword.Context);
                    builder.Append(' ');
                    MatchResult filterList = onClause.Matches[SqlGrammar.JoinPrime.Multiple.On.FilterList];
                    builder.Append(filterList.Context);
                }
                MatchResult remaining = multiple.Matches[SqlGrammar.JoinPrime.Multiple.JoinPrime];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult empty = result.Matches[SqlGrammar.JoinPrime.Empty];
            return empty.Context;
        }

        /// <summary>
        /// Generates output for a filter list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnFilterList(MatchResult result)
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

        /// <summary>
        /// Generates output for a filter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnFilter(MatchResult result)
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

        /// <summary>
        /// Generates output for a value list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnValueList(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for a GROUP BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnGroupByList(MatchResult result)
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

        /// <summary>
        /// Generates output for an item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnItem(MatchResult result)
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

        /// <summary>
        /// Generates output for the INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnInsertStatement(MatchResult result)
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

        /// <summary>
        /// Generates output for the column list in an INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnColumnList(MatchResult result)
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

        /// <summary>
        /// Generates output for the UDPATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnUpdateStatement(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for the list of setters in an UPDATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnSetterList(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for a setter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnSetter(MatchResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output for the DELETE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnDeleteStatement(MatchResult result)
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

        /// <summary>
        /// Generates output for a multipart identifier.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected override object OnMultipartIdentifier(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultipartIdentifier.Multiple.Name];
            if (multiple.IsMatch)
            {
                StringBuilder builder = new StringBuilder();
                MatchResult first = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.First];
                builder.Append(first.Context);
                MatchResult dot = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Dot];
                builder.Append(dot.Context);
                MatchResult remaining = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Remaining];
                builder.Append(remaining.Context);
                return builder.ToString();
            }
            MatchResult single = result.Matches[SqlGrammar.MultipartIdentifier.Single];
            return single.Context;
        }
    }
}
