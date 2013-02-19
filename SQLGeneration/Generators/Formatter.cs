using System;
using System.IO;
using System.Text;
using SQLGeneration.Builders;
using SQLGeneration.Parsing;

namespace SQLGeneration.Generators
{
    /// <summary>
    /// Generates simple SQL from a token source.
    /// </summary>
    public sealed class Formatter : SqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="registry">The token registry to use.</param>
        public Formatter(SqlTokenRegistry registry)
            : base(new SqlGrammar(registry))
        {
        }

        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        public Formatter(SqlGrammar grammar = null)
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
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                TokenStream tokenStream = command.GetCommandTokens(options);
                MatchResult result = GetResult(tokenStream.CreateTokenSource());
                buildStart(result, writer);
            }
            return builder.ToString();
        }

        private void buildStart(MatchResult result, TextWriter writer)
        {
            MatchResult select = result.Matches[SqlGrammar.Start.SelectStatement];
            if (select.IsMatch)
            {
                buildSelectStatement(select, writer);
                return;
            }
            MatchResult insert = result.Matches[SqlGrammar.Start.InsertStatement];
            if (insert.IsMatch)
            {
                buildInsertStatement(insert, writer);
                return;
            }
            MatchResult update = result.Matches[SqlGrammar.Start.UpdateStatement];
            if (update.IsMatch)
            {
                buildUpdateStatement(update, writer);
                return;
            }
            MatchResult delete = result.Matches[SqlGrammar.Start.DeleteStatement];
            if (delete.IsMatch)
            {
                buildDeleteStatement(delete, writer);
                return;
            }
        }

        private void buildSelectStatement(MatchResult result, TextWriter writer)
        {
            MatchResult selectExpression = result.Matches[SqlGrammar.SelectStatement.SelectExpression];
            buildSelectExpression(selectExpression, writer);
            MatchResult orderBy = result.Matches[SqlGrammar.SelectStatement.OrderBy.Name];
            if (orderBy.IsMatch)
            {
                writer.Write(' ');
                MatchResult orderByKeyword = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByKeyword];
                writeToken(orderByKeyword, writer);
                writer.Write(' ');
                MatchResult orderByList = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByList];
                buildOrderByList(orderByList, writer);
            }
        }

        private void buildSelectExpression(MatchResult result, TextWriter writer)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.SelectExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult selectExpression = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.SelectExpression];
                buildSelectExpression(selectExpression, writer);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.RightParenthesis];
                writeToken(rightParenthesis, writer);
                return;
            }
            MatchResult specification = result.Matches[SqlGrammar.SelectExpression.SelectSpecification];
            buildSelectSpecification(specification, writer);
            MatchResult remaining = result.Matches[SqlGrammar.SelectExpression.Remaining.Name];
            if (remaining.IsMatch)
            {
                writer.Write(' ');
                MatchResult combiner = remaining.Matches[SqlGrammar.SelectExpression.Remaining.Combiner];
                buildSelectCombiner(combiner, writer);
                writer.Write(' ');
                MatchResult distinctQualifier = remaining.Matches[SqlGrammar.SelectExpression.Remaining.DistinctQualifier];
                if (distinctQualifier.IsMatch)
                {
                    buildDistinctQualifier(distinctQualifier, writer);
                    writer.Write(' ');
                }
                MatchResult selectExpression = remaining.Matches[SqlGrammar.SelectExpression.Remaining.SelectExpression];
                buildSelectExpression(selectExpression, writer);
            }
        }

        private void buildSelectSpecification(MatchResult result, TextWriter writer)
        {
            MatchResult selectKeyword = result.Matches[SqlGrammar.SelectSpecification.SelectKeyword];
            writeToken(selectKeyword, writer);
            writer.Write(' ');
            MatchResult distinctQualifier = result.Matches[SqlGrammar.SelectSpecification.DistinctQualifier];
            if (distinctQualifier.IsMatch)
            {
                buildDistinctQualifier(distinctQualifier, writer);
                writer.Write(' ');
            }
            MatchResult top = result.Matches[SqlGrammar.SelectSpecification.Top.Name];
            if (top.IsMatch)
            {
                buildTop(top, writer);
            }
            MatchResult projectionList = result.Matches[SqlGrammar.SelectSpecification.ProjectionList];
            buildProjectionList(projectionList, writer);
            MatchResult from = result.Matches[SqlGrammar.SelectSpecification.From.Name];
            if (from.IsMatch)
            {
                writer.Write(' ');
                MatchResult fromKeyword = from.Matches[SqlGrammar.SelectSpecification.From.FromKeyword];
                writeToken(fromKeyword, writer);
                writer.Write(' ');
                MatchResult fromList = from.Matches[SqlGrammar.SelectSpecification.From.FromList];
                buildFromList(fromList, writer);
            }
            MatchResult where = result.Matches[SqlGrammar.SelectSpecification.Where.Name];
            if (where.IsMatch)
            {
                writer.Write(' ');
                MatchResult whereKeyword = where.Matches[SqlGrammar.SelectSpecification.Where.WhereKeyword];
                writeToken(whereKeyword, writer);
                writer.Write(' ');
                MatchResult filterList = where.Matches[SqlGrammar.SelectSpecification.Where.FilterList];
                buildOrFilter(filterList, writer);
            }
            MatchResult groupBy = result.Matches[SqlGrammar.SelectSpecification.GroupBy.Name];
            if (groupBy.IsMatch)
            {
                writer.Write(' ');
                MatchResult groupByKeyword = groupBy.Matches[SqlGrammar.SelectSpecification.GroupBy.GroupByKeyword];
                writeToken(groupByKeyword, writer);
                writer.Write(' ');
                MatchResult groupByList = groupBy.Matches[SqlGrammar.SelectSpecification.GroupBy.GroupByList];
                buildGroupByList(groupByList, writer);
            }
            MatchResult having = result.Matches[SqlGrammar.SelectSpecification.Having.Name];
            if (having.IsMatch)
            {
                writer.Write(' ');
                MatchResult havingKeyword = having.Matches[SqlGrammar.SelectSpecification.Having.HavingKeyword];
                writeToken(havingKeyword, writer);
                writer.Write(' ');
                MatchResult filterList = having.Matches[SqlGrammar.SelectSpecification.Having.FilterList];
                buildOrFilter(filterList, writer);
            }
        }

        private void buildDistinctQualifier(MatchResult result, TextWriter writer)
        {
            MatchResult distinct = result.Matches[SqlGrammar.DistinctQualifier.Distinct];
            if (distinct.IsMatch)
            {
                writeToken(distinct, writer);
                return;
            }
            MatchResult all = result.Matches[SqlGrammar.DistinctQualifier.All];
            if (all.IsMatch)
            {
                writeToken(all, writer);
                return;
            }
        }

        private void buildTop(MatchResult result, TextWriter writer)
        {
            MatchResult topKeyword = result.Matches[SqlGrammar.SelectSpecification.Top.TopKeyword];
            writeToken(topKeyword, writer);
            writer.Write(' ');
            MatchResult expression = result.Matches[SqlGrammar.SelectSpecification.Top.Expression];
            buildArithmeticItem(expression, writer);
            writer.Write(' ');
            MatchResult percentKeyword = result.Matches[SqlGrammar.SelectSpecification.Top.PercentKeyword];
            if (percentKeyword.IsMatch)
            {
                writeToken(percentKeyword, writer);
                writer.Write(' ');
            }
            MatchResult withTiesKeyword = result.Matches[SqlGrammar.SelectSpecification.Top.WithTiesKeyword];
            if (withTiesKeyword.IsMatch)
            {
                writeToken(withTiesKeyword, writer);
                writer.Write(' ');
            }
        }

        private void buildProjectionList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ProjectionList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.ProjectionList.Multiple.First];
                buildProjectionItem(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Remaining];
                buildProjectionList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ProjectionList.Single];
            buildProjectionItem(single, writer);
        }

        private void buildProjectionItem(MatchResult result, TextWriter writer)
        {
            MatchResult expression = result.Matches[SqlGrammar.ProjectionItem.Expression.Name];
            if (expression.IsMatch)
            {
                MatchResult item = expression.Matches[SqlGrammar.ProjectionItem.Expression.Item];
                buildArithmeticItem(item, writer);
                MatchResult aliasExpression = expression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Name];
                if (aliasExpression.IsMatch)
                {
                    MatchResult asKeyword = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.AliasIndicator];
                    if (asKeyword.IsMatch)
                    {
                        writer.Write(' ');
                        writeToken(asKeyword, writer);
                    }
                    writer.Write(' ');
                    MatchResult alias = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Alias];
                    writeToken(alias, writer);
                }
                return;
            }
            MatchResult star = result.Matches[SqlGrammar.ProjectionItem.Star.Name];
            if (star.IsMatch)
            {
                MatchResult qualifier = star.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Name];
                if (qualifier.IsMatch)
                {
                    MatchResult columnSource = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.ColumnSource];
                    buildMultipartIdentifier(columnSource, writer);
                    MatchResult dot = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Dot];
                    writeToken(dot, writer);
                }
                MatchResult starToken = star.Matches[SqlGrammar.ProjectionItem.Star.StarToken];
                writeToken(starToken, writer);
            }
        }

        private void buildFromList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.FromList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.FromList.Multiple.First];
                buildJoin(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.FromList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.FromList.Multiple.Remaining];
                buildFromList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.FromList.Single];
            if (single.IsMatch)
            {
                buildJoin(single, writer);
                return;
            }
        }

        private void buildJoin(MatchResult result, TextWriter writer)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Join.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.Join.Wrapped.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult join = wrapped.Matches[SqlGrammar.Join.Wrapped.Join];
                buildJoin(join, writer);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.Join.Wrapped.RightParenthesis];
                writeToken(rightParenthesis, writer);
                MatchResult joinPrime = wrapped.Matches[SqlGrammar.Join.Wrapped.JoinPrime];
                buildJoinPrime(joinPrime, writer);
                return;
            }
            MatchResult joined = result.Matches[SqlGrammar.Join.Joined.Name];
            if (joined.IsMatch)
            {
                MatchResult joinItem = joined.Matches[SqlGrammar.Join.Joined.JoinItem];
                buildJoinItem(joinItem, writer);
                MatchResult joinPrime = joined.Matches[SqlGrammar.Join.Joined.JoinPrime];
                buildJoinPrime(joinPrime, writer);
                return;
            }
        }

        private void buildJoinItem(MatchResult result, TextWriter writer)
        {
            MatchResult functionCall = result.Matches[SqlGrammar.JoinItem.FunctionCall];
            if (functionCall.IsMatch)
            {
                buildFunctionCall(functionCall, writer);
            }
            MatchResult table = result.Matches[SqlGrammar.JoinItem.Table];
            if (table.IsMatch)
            {
                buildMultipartIdentifier(table, writer);
            }
            MatchResult select = result.Matches[SqlGrammar.JoinItem.Select.Name];
            if (select.IsMatch)
            {
                MatchResult leftParenthesis = select.Matches[SqlGrammar.JoinItem.Select.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult selectStatement = select.Matches[SqlGrammar.JoinItem.Select.SelectStatement];
                buildSelectStatement(selectStatement, writer);
                MatchResult rightParenthesis = select.Matches[SqlGrammar.JoinItem.Select.RightParenthesis];
                writeToken(rightParenthesis, writer);
            }
            MatchResult aliasExpression = result.Matches[SqlGrammar.JoinItem.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult asKeyword = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.AliasIndicator];
                if (asKeyword.IsMatch)
                {
                    writer.Write(' ');
                    writeToken(asKeyword, writer);
                }
                MatchResult alias = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.Alias];
                writer.Write(' ');
                writeToken(alias, writer);
            }
        }

        private void buildJoinPrime(MatchResult result, TextWriter writer)
        {
            MatchResult filtered = result.Matches[SqlGrammar.JoinPrime.Filtered.Name];
            if (filtered.IsMatch)
            {
                writer.Write(' ');
                MatchResult joinType = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinType];
                buildFilteredJoinType(joinType, writer);
                writer.Write(' ');
                MatchResult joinItem = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinItem];
                buildJoinItem(joinItem, writer);
                writer.Write(' ');
                MatchResult on = filtered.Matches[SqlGrammar.JoinPrime.Filtered.On.Name];
                MatchResult onKeyword = on.Matches[SqlGrammar.JoinPrime.Filtered.On.OnKeyword];
                writeToken(onKeyword, writer);
                writer.Write(' ');
                MatchResult filterList = on.Matches[SqlGrammar.JoinPrime.Filtered.On.FilterList];
                buildOrFilter(filterList, writer);
                MatchResult joinPrime = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinPrime];
                buildJoinPrime(joinPrime, writer);
                return;
            }
            MatchResult cross = result.Matches[SqlGrammar.JoinPrime.Cross.Name];
            if (cross.IsMatch)
            {
                writer.Write(' ');
                MatchResult joinType = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinType];
                writeToken(joinType, writer);
                writer.Write(' ');
                MatchResult joinItem = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinItem];
                buildJoinItem(joinItem, writer);
                MatchResult joinPrime = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinPrime];
                buildJoinPrime(joinPrime, writer);
                return;
            }
        }

        private void buildFilteredJoinType(MatchResult result, TextWriter writer)
        {
            MatchResult inner = result.Matches[SqlGrammar.FilteredJoinType.InnerJoin];
            if (inner.IsMatch)
            {
                writeToken(inner, writer);
                return;
            }
            MatchResult left = result.Matches[SqlGrammar.FilteredJoinType.LeftOuterJoin];
            if (left.IsMatch)
            {
                writeToken(left, writer);
                return;
            }
            MatchResult right = result.Matches[SqlGrammar.FilteredJoinType.RightOuterJoin];
            if (right.IsMatch)
            {
                writeToken(right, writer);
                return;
            }
            MatchResult full = result.Matches[SqlGrammar.FilteredJoinType.FullOuterJoin];
            if (full.IsMatch)
            {
                writeToken(full, writer);
                return;
            }
        }

        private void buildGroupByList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.GroupByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.GroupByList.Multiple.First];
                buildArithmeticItem(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.GroupByList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.GroupByList.Multiple.Remaining];
                buildGroupByList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.GroupByList.Single];
            if (single.IsMatch)
            {
                buildArithmeticItem(single, writer);
                return;
            }
        }

        private void buildSelectCombiner(MatchResult result, TextWriter writer)
        {
            MatchResult union = result.Matches[SqlGrammar.SelectCombiner.Union];
            if (union.IsMatch)
            {
                writeToken(union, writer);
                return;
            }
            MatchResult intersect = result.Matches[SqlGrammar.SelectCombiner.Intersect];
            if (intersect.IsMatch)
            {
                writeToken(intersect, writer);
                return;
            }
            MatchResult except = result.Matches[SqlGrammar.SelectCombiner.Except];
            if (except.IsMatch)
            {
                writeToken(except, writer);
                return;
            }
            MatchResult minus = result.Matches[SqlGrammar.SelectCombiner.Minus];
            if (minus.IsMatch)
            {
                writeToken(minus, writer);
                return;
            }
        }

        private void buildOrderByList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrderByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.OrderByList.Multiple.First];
                buildOrderByItem(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.OrderByList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.OrderByList.Multiple.Remaining];
                buildOrderByList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.OrderByList.Single];
            if (single.IsMatch)
            {
                buildOrderByItem(single, writer);
                return;
            }
        }

        private void buildOrderByItem(MatchResult result, TextWriter writer)
        {
            MatchResult expression = result.Matches[SqlGrammar.OrderByItem.Expression];
            buildArithmeticItem(expression, writer);
            MatchResult direction = result.Matches[SqlGrammar.OrderByItem.OrderDirection];
            if (direction.IsMatch)
            {
                writer.Write(' ');
                buildOrderDirection(direction, writer);
            }
            MatchResult nullPlacement = result.Matches[SqlGrammar.OrderByItem.NullPlacement];
            if (nullPlacement.IsMatch)
            {
                writer.Write(' ');
                buildNullPlacement(nullPlacement, writer);
            }
        }

        private void buildOrderDirection(MatchResult result, TextWriter writer)
        {
            MatchResult descending = result.Matches[SqlGrammar.OrderDirection.Descending];
            if (descending.IsMatch)
            {
                writeToken(descending, writer);
                return;
            }
            MatchResult ascending = result.Matches[SqlGrammar.OrderDirection.Ascending];
            if (ascending.IsMatch)
            {
                writeToken(ascending, writer);
                return;
            }
        }

        private void buildNullPlacement(MatchResult result, TextWriter writer)
        {
            MatchResult nullsFirst = result.Matches[SqlGrammar.NullPlacement.NullsFirst];
            if (nullsFirst.IsMatch)
            {
                writeToken(nullsFirst, writer);
                return;
            }
            MatchResult nullsLast = result.Matches[SqlGrammar.NullPlacement.NullsLast];
            if (nullsLast.IsMatch)
            {
                writeToken(nullsLast, writer);
                return;
            }
        }

        private void buildInsertStatement(MatchResult result, TextWriter writer)
        {
            MatchResult insertKeyword = result.Matches[SqlGrammar.InsertStatement.InsertKeyword];
            writeToken(insertKeyword, writer);
            writer.Write(' ');
            MatchResult intoKeyword = result.Matches[SqlGrammar.InsertStatement.IntoKeyword];
            if (intoKeyword.IsMatch)
            {
                writeToken(intoKeyword, writer);
                writer.Write(' ');
            }
            MatchResult table = result.Matches[SqlGrammar.InsertStatement.Table];
            buildMultipartIdentifier(table, writer);
            MatchResult aliasExpression = result.Matches[SqlGrammar.InsertStatement.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult asKeyword = aliasExpression.Matches[SqlGrammar.InsertStatement.AliasExpression.AliasIndicator];
                if (asKeyword.IsMatch)
                {
                    writer.Write(' ');
                    writeToken(asKeyword, writer);
                }
                writer.Write(' ');
                MatchResult alias = aliasExpression.Matches[SqlGrammar.InsertStatement.AliasExpression.Alias];
                writeToken(alias, writer);
            }
            MatchResult columns = result.Matches[SqlGrammar.InsertStatement.Columns.Name];
            if (columns.IsMatch)
            {
                writer.Write(' ');
                MatchResult leftParenthesis = columns.Matches[SqlGrammar.InsertStatement.Columns.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult columnList = columns.Matches[SqlGrammar.InsertStatement.Columns.ColumnList];
                buildColumnList(columnList, writer);
                MatchResult rightParenthesis = columns.Matches[SqlGrammar.InsertStatement.Columns.RightParenthesis];
                writeToken(rightParenthesis, writer);
            }
            writer.Write(' ');
            MatchResult values = result.Matches[SqlGrammar.InsertStatement.Values.Name];
            if (values.IsMatch)
            {
                MatchResult valuesKeyword = values.Matches[SqlGrammar.InsertStatement.Values.ValuesKeyword];
                writeToken(valuesKeyword, writer);
                MatchResult leftParenthesis = values.Matches[SqlGrammar.InsertStatement.Values.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult valueList = values.Matches[SqlGrammar.InsertStatement.Values.ValueList];
                buildValueList(valueList, writer);
                MatchResult rightParenthesis = values.Matches[SqlGrammar.InsertStatement.Values.RightParenthesis];
                writeToken(rightParenthesis, writer);
            }
            MatchResult select = result.Matches[SqlGrammar.InsertStatement.Select.Name];
            if (select.IsMatch)
            {
                MatchResult leftParenthesis = select.Matches[SqlGrammar.InsertStatement.Select.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult selectStatement = select.Matches[SqlGrammar.InsertStatement.Select.SelectStatement];
                buildSelectStatement(selectStatement, writer);
                MatchResult rightParenthesis = select.Matches[SqlGrammar.InsertStatement.Select.RightParenthesis];
                writeToken(rightParenthesis, writer);
            }
        }

        private void buildColumnList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ColumnList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.ColumnList.Multiple.First];
                buildMultipartIdentifier(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.ColumnList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ColumnList.Multiple.Remaining];
                buildColumnList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ColumnList.Single];
            if (single.IsMatch)
            {
                buildMultipartIdentifier(single, writer);
                return;
            }
        }

        private void buildUpdateStatement(MatchResult result, TextWriter writer)
        {
            MatchResult updateKeyword = result.Matches[SqlGrammar.UpdateStatement.UpdateKeyword];
            writeToken(updateKeyword, writer);
            writer.Write(' ');
            MatchResult table = result.Matches[SqlGrammar.UpdateStatement.Table];
            buildMultipartIdentifier(table, writer);
            writer.Write(' ');
            MatchResult aliasExpression = result.Matches[SqlGrammar.UpdateStatement.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult asKeyword = aliasExpression.Matches[SqlGrammar.UpdateStatement.AliasExpression.AliasIndicator];
                if (asKeyword.IsMatch)
                {
                    writeToken(asKeyword, writer);
                    writer.Write(' ');
                }
                MatchResult alias = aliasExpression.Matches[SqlGrammar.UpdateStatement.AliasExpression.Alias];
                writeToken(alias, writer);
                writer.Write(' ');
            }
            MatchResult setKeyword = result.Matches[SqlGrammar.UpdateStatement.SetKeyword];
            writeToken(setKeyword, writer);
            writer.Write(' ');
            MatchResult setterList = result.Matches[SqlGrammar.UpdateStatement.SetterList];
            buildSetterList(setterList, writer);
            MatchResult where = result.Matches[SqlGrammar.UpdateStatement.Where.Name];
            if (where.IsMatch)
            {
                writer.Write(' ');
                MatchResult whereKeyword = where.Matches[SqlGrammar.UpdateStatement.Where.WhereKeyword];
                writeToken(whereKeyword, writer);
                writer.Write(' ');
                MatchResult filterList = where.Matches[SqlGrammar.UpdateStatement.Where.FilterList];
                buildOrFilter(filterList, writer);
            }
        }

        private void buildSetterList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.SetterList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.SetterList.Multiple.First];
                buildSetter(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.SetterList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.SetterList.Multiple.Remaining];
                buildSetterList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.SetterList.Single];
            if (single.IsMatch)
            {
                buildSetter(single, writer);
                return;
            }
        }

        private void buildSetter(MatchResult result, TextWriter writer)
        {
            MatchResult column = result.Matches[SqlGrammar.Setter.Column];
            buildMultipartIdentifier(column, writer);
            writer.Write(' ');
            MatchResult assignmentOperator = result.Matches[SqlGrammar.Setter.Assignment];
            writeToken(assignmentOperator, writer);
            writer.Write(' ');
            MatchResult value = result.Matches[SqlGrammar.Setter.Value];
            buildArithmeticItem(value, writer);
        }

        private void buildDeleteStatement(MatchResult result, TextWriter writer)
        {
            MatchResult deleteKeyword = result.Matches[SqlGrammar.DeleteStatement.DeleteKeyword];
            writeToken(deleteKeyword, writer);
            MatchResult fromKeyword = result.Matches[SqlGrammar.DeleteStatement.FromKeyword];
            writer.Write(' ');
            if (fromKeyword.IsMatch)
            {
                writeToken(fromKeyword, writer);
                writer.Write(' ');
            }
            MatchResult table = result.Matches[SqlGrammar.DeleteStatement.Table];
            buildMultipartIdentifier(table, writer);
            MatchResult aliasExpression = result.Matches[SqlGrammar.DeleteStatement.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult asKeyword = aliasExpression.Matches[SqlGrammar.DeleteStatement.AliasExpression.AliasIndicator];
                if (asKeyword.IsMatch)
                {
                    writer.Write(' ');
                    writeToken(asKeyword, writer);
                }
                MatchResult alias = aliasExpression.Matches[SqlGrammar.DeleteStatement.AliasExpression.Alias];
                writer.Write(' ');
                writeToken(alias, writer);
            }
            MatchResult where = result.Matches[SqlGrammar.DeleteStatement.Where.Name];
            if (where.IsMatch)
            {
                writer.Write(' ');
                MatchResult whereKeyword = where.Matches[SqlGrammar.DeleteStatement.Where.WhereKeyword];
                writeToken(whereKeyword, writer);
                writer.Write(' ');
                MatchResult filterList = where.Matches[SqlGrammar.DeleteStatement.Where.FilterList];
                buildOrFilter(filterList, writer);
            }
        }

        private void buildOrFilter(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrFilter.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.OrFilter.Multiple.First];
                buildAndFilter(first, writer);
                writer.Write(' ');
                MatchResult or = multiple.Matches[SqlGrammar.OrFilter.Multiple.Or];
                writeToken(or, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.OrFilter.Multiple.Remaining];
                buildOrFilter(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.OrFilter.Single];
            if (single.IsMatch)
            {
                buildAndFilter(single, writer);
                return;
            }
        }

        private void buildAndFilter(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.AndFilter.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.AndFilter.Multiple.First];
                buildFilter(first, writer);
                writer.Write(' ');
                MatchResult and = multiple.Matches[SqlGrammar.AndFilter.Multiple.And];
                writeToken(and, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.AndFilter.Multiple.Remaining];
                buildOrFilter(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.AndFilter.Single];
            if (single.IsMatch)
            {
                buildFilter(single, writer);
                return;
            }
        }

        private void buildFilter(MatchResult result, TextWriter writer)
        {
            MatchResult notResult = result.Matches[SqlGrammar.Filter.Not.Name];
            if (notResult.IsMatch)
            {
                MatchResult notKeyword = notResult.Matches[SqlGrammar.Filter.Not.NotKeyword];
                writeToken(notKeyword, writer);
                writer.Write(' ');
                MatchResult filter = notResult.Matches[SqlGrammar.Filter.Not.Filter];
                buildFilter(filter, writer);
                return;
            }
            MatchResult wrapped = result.Matches[SqlGrammar.Filter.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.Filter.Wrapped.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult filter = wrapped.Matches[SqlGrammar.Filter.Wrapped.Filter];
                buildOrFilter(filter, writer);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.Filter.Wrapped.RightParenthesis];
                writeToken(rightParenthesis, writer);
                return;
            }
            MatchResult quantify = result.Matches[SqlGrammar.Filter.Quantify.Name];
            if (quantify.IsMatch)
            {
                MatchResult expression = quantify.Matches[SqlGrammar.Filter.Quantify.Expression];
                buildArithmeticItem(expression, writer);
                writer.Write(' ');
                MatchResult comparison = quantify.Matches[SqlGrammar.Filter.Quantify.ComparisonOperator];
                buildComparisonOperator(comparison, writer);
                writer.Write(' ');
                MatchResult quantifier = quantify.Matches[SqlGrammar.Filter.Quantify.Quantifier];
                buildQuantifier(quantifier, writer);
                writer.Write(' ');
                MatchResult leftParenthesis = quantify.Matches[SqlGrammar.Filter.Quantify.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult valueList = quantify.Matches[SqlGrammar.Filter.Quantify.ValueList];
                if (valueList.IsMatch)
                {
                    buildValueList(valueList, writer);
                }
                MatchResult select = quantify.Matches[SqlGrammar.Filter.Quantify.SelectStatement];
                if (select.IsMatch)
                {
                    buildSelectStatement(select, writer);
                }
                MatchResult rightParenthesis = quantify.Matches[SqlGrammar.Filter.Quantify.RightParenthesis];
                writeToken(rightParenthesis, writer);
                return;
            }
            MatchResult order = result.Matches[SqlGrammar.Filter.Order.Name];
            if (order.IsMatch)
            {
                MatchResult left = order.Matches[SqlGrammar.Filter.Order.Left];
                buildArithmeticItem(left, writer);
                writer.Write(' ');
                MatchResult operatorResult = order.Matches[SqlGrammar.Filter.Order.ComparisonOperator];
                buildComparisonOperator(operatorResult, writer);
                writer.Write(' ');
                MatchResult right = order.Matches[SqlGrammar.Filter.Order.Right];
                buildArithmeticItem(right, writer);
                return;
            }
            MatchResult between = result.Matches[SqlGrammar.Filter.Between.Name];
            if (between.IsMatch)
            {
                MatchResult expression = between.Matches[SqlGrammar.Filter.Between.Expression];
                buildArithmeticItem(expression, writer);
                writer.Write(' ');
                MatchResult notKeyword = between.Matches[SqlGrammar.Filter.Between.NotKeyword];
                if (notKeyword.IsMatch)
                {
                    writeToken(notKeyword, writer);
                    writer.Write(' ');
                }
                MatchResult betweenKeyword = between.Matches[SqlGrammar.Filter.Between.BetweenKeyword];
                writeToken(betweenKeyword, writer);
                writer.Write(' ');
                MatchResult lowerBound = between.Matches[SqlGrammar.Filter.Between.LowerBound];
                buildArithmeticItem(lowerBound, writer);
                writer.Write(' ');
                MatchResult andKeyword = between.Matches[SqlGrammar.Filter.Between.And];
                writeToken(andKeyword, writer);
                writer.Write(' ');
                MatchResult upperBound = between.Matches[SqlGrammar.Filter.Between.UpperBound];
                buildArithmeticItem(upperBound, writer);
                return;
            }
            MatchResult like = result.Matches[SqlGrammar.Filter.Like.Name];
            if (like.IsMatch)
            {
                MatchResult expression = like.Matches[SqlGrammar.Filter.Like.Expression];
                buildArithmeticItem(expression, writer);
                writer.Write(' ');
                MatchResult notKeyword = like.Matches[SqlGrammar.Filter.Like.NotKeyword];
                if (notKeyword.IsMatch)
                {
                    writeToken(notKeyword, writer);
                    writer.Write(' ');
                }
                MatchResult likeKeyword = like.Matches[SqlGrammar.Filter.Like.LikeKeyword];
                writeToken(likeKeyword, writer);
                writer.Write(' ');
                MatchResult value = like.Matches[SqlGrammar.Filter.Like.Value];
                writeToken(value, writer);
                return;
            }
            MatchResult isResult = result.Matches[SqlGrammar.Filter.Is.Name];
            if (isResult.IsMatch)
            {
                MatchResult expression = isResult.Matches[SqlGrammar.Filter.Is.Expression];
                buildArithmeticItem(expression, writer);
                writer.Write(' ');
                MatchResult isKeyword = isResult.Matches[SqlGrammar.Filter.Is.IsKeyword];
                writeToken(isKeyword, writer);
                writer.Write(' ');
                MatchResult notKeyword = isResult.Matches[SqlGrammar.Filter.Is.NotKeyword];
                if (notKeyword.IsMatch)
                {
                    writeToken(notKeyword, writer);
                    writer.Write(' ');
                }
                MatchResult nullKeyword = isResult.Matches[SqlGrammar.Filter.Is.NullKeyword];
                writeToken(nullKeyword, writer);
                return;
            }
            MatchResult inResult = result.Matches[SqlGrammar.Filter.In.Name];
            if (inResult.IsMatch)
            {
                MatchResult expression = inResult.Matches[SqlGrammar.Filter.In.Expression];
                buildArithmeticItem(expression, writer);
                writer.Write(' ');
                MatchResult notKeyword = inResult.Matches[SqlGrammar.Filter.In.NotKeyword];
                if (notKeyword.IsMatch)
                {
                    writeToken(notKeyword, writer);
                    writer.Write(' ');
                }
                MatchResult inKeyword = inResult.Matches[SqlGrammar.Filter.In.InKeyword];
                writeToken(inKeyword, writer);
                writer.Write(' ');
                MatchResult values = inResult.Matches[SqlGrammar.Filter.In.Values.Name];
                if (values.IsMatch)
                {
                    MatchResult leftParenthesis = values.Matches[SqlGrammar.Filter.In.Values.LeftParenthesis];
                    writeToken(leftParenthesis, writer);
                    MatchResult valueList = values.Matches[SqlGrammar.Filter.In.Values.ValueList];
                    if (valueList.IsMatch)
                    {
                        buildValueList(valueList, writer);
                    }
                    MatchResult rightParenthesis = values.Matches[SqlGrammar.Filter.In.Values.RightParenthesis];
                    writeToken(rightParenthesis, writer);
                }
                MatchResult select = inResult.Matches[SqlGrammar.Filter.In.Select.Name];
                if (select.IsMatch)
                {
                    MatchResult leftParenthesis = select.Matches[SqlGrammar.Filter.In.Select.LeftParenthesis];
                    writeToken(leftParenthesis, writer);
                    MatchResult selectStatement = select.Matches[SqlGrammar.Filter.In.Select.SelectStatement];
                    buildSelectStatement(selectStatement, writer);
                    MatchResult rightParenthesis = select.Matches[SqlGrammar.Filter.In.Select.RightParenthesis];
                    writeToken(rightParenthesis, writer);
                }
                MatchResult functionCall = inResult.Matches[SqlGrammar.Filter.In.FunctionCall];
                if (functionCall.IsMatch)
                {
                    buildFunctionCall(functionCall, writer);
                }
                return;
            }
            MatchResult exists = result.Matches[SqlGrammar.Filter.Exists.Name];
            if (exists.IsMatch)
            {
                MatchResult existsKeyword = exists.Matches[SqlGrammar.Filter.Exists.ExistsKeyword];
                writeToken(existsKeyword, writer);
                MatchResult leftParenthesis = exists.Matches[SqlGrammar.Filter.Exists.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult selectStatement = exists.Matches[SqlGrammar.Filter.Exists.SelectStatement];
                buildSelectStatement(selectStatement, writer);
                MatchResult rightParenthesis = exists.Matches[SqlGrammar.Filter.Exists.RightParenthesis];
                writeToken(rightParenthesis, writer);
                return;
            }
        }

        private void buildQuantifier(MatchResult result, TextWriter writer)
        {
            MatchResult all = result.Matches[SqlGrammar.Quantifier.All];
            if (all.IsMatch)
            {
                writeToken(all, writer);
                return;
            }
            MatchResult any = result.Matches[SqlGrammar.Quantifier.Any];
            if (any.IsMatch)
            {
                writeToken(any, writer);
                return;
            }
            MatchResult some = result.Matches[SqlGrammar.Quantifier.Some];
            if (some.IsMatch)
            {
                writeToken(some, writer);
                return;
            }
        }

        private void buildComparisonOperator(MatchResult result, TextWriter writer)
        {
            MatchResult equalTo = result.Matches[SqlGrammar.ComparisonOperator.EqualTo];
            if (equalTo.IsMatch)
            {
                writeToken(equalTo, writer);
                return;
            }
            MatchResult notEqualTo = result.Matches[SqlGrammar.ComparisonOperator.NotEqualTo];
            if (notEqualTo.IsMatch)
            {
                writeToken(notEqualTo, writer);
                return;
            }
            MatchResult lessThanEqualTo = result.Matches[SqlGrammar.ComparisonOperator.LessThanEqualTo];
            if (lessThanEqualTo.IsMatch)
            {
                writeToken(lessThanEqualTo, writer);
                return;
            }
            MatchResult greaterThanEqualTo = result.Matches[SqlGrammar.ComparisonOperator.GreaterThanEqualTo];
            if (greaterThanEqualTo.IsMatch)
            {
                writeToken(greaterThanEqualTo, writer);
                return;
            }
            MatchResult lessThan = result.Matches[SqlGrammar.ComparisonOperator.LessThan];
            if (lessThan.IsMatch)
            {
                writeToken(lessThan, writer);
                return;
            }
            MatchResult greaterThan = result.Matches[SqlGrammar.ComparisonOperator.GreaterThan];
            if (greaterThan.IsMatch)
            {
                writeToken(greaterThan, writer);
                return;
            }
        }

        private void buildArithmeticItem(MatchResult result, TextWriter writer)
        {
            MatchResult arithmeticExpression = result.Matches[SqlGrammar.ArithmeticItem.ArithmeticExpression];
            if (arithmeticExpression.IsMatch)
            {
                buildAdditiveExpression(arithmeticExpression, writer);
                return;
            }
        }

        private void buildAdditiveExpression(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.AdditiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.First];
                buildMultiplicitiveExpression(first, writer);
                writer.Write(' ');
                MatchResult additiveOperator = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Operator];
                buildAdditiveOperator(additiveOperator, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Remaining];
                buildAdditiveExpression(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.AdditiveExpression.Single];
            if (single.IsMatch)
            {
                buildMultiplicitiveExpression(single, writer);
                return;
            }
        }

        private void buildAdditiveOperator(MatchResult result, TextWriter writer)
        {
            MatchResult plus = result.Matches[SqlGrammar.AdditiveOperator.PlusOperator];
            if (plus.IsMatch)
            {
                writeToken(plus, writer);
                return;
            }
            MatchResult minus = result.Matches[SqlGrammar.AdditiveOperator.MinusOperator];
            if (minus.IsMatch)
            {
                writeToken(minus, writer);
                return;
            }
        }

        private void buildMultiplicitiveExpression(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.First];
                buildWrappedItem(first, writer);
                writer.Write(' ');
                MatchResult multiplicitiveOperator = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Operator];
                buildMultiplicitiveOperator(multiplicitiveOperator, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Remaining];
                buildMultiplicitiveExpression(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.MultiplicitiveExpression.Single];
            if (single.IsMatch)
            {
                buildWrappedItem(single, writer);
                return;
            }
        }

        private void buildMultiplicitiveOperator(MatchResult result, TextWriter writer)
        {
            MatchResult multiplication = result.Matches[SqlGrammar.MultiplicitiveOperator.Multiply];
            if (multiplication.IsMatch)
            {
                writeToken(multiplication, writer);
                return;
            }
            MatchResult division = result.Matches[SqlGrammar.MultiplicitiveOperator.Divide];
            if (division.IsMatch)
            {
                writeToken(division, writer);
                return;
            }
            MatchResult modulus = result.Matches[SqlGrammar.MultiplicitiveOperator.Modulus];
            if (modulus.IsMatch)
            {
                writeToken(modulus, writer);
                return;
            }
        }

        private void buildWrappedItem(MatchResult result, TextWriter writer)
        {
            MatchResult negated = result.Matches[SqlGrammar.WrappedItem.Negated.Name];
            if (negated.IsMatch)
            {
                MatchResult minus = negated.Matches[SqlGrammar.WrappedItem.Negated.Minus];
                writeToken(minus, writer);
                MatchResult wrappedItem = negated.Matches[SqlGrammar.WrappedItem.Negated.Item];
                buildWrappedItem(wrappedItem, writer);
            }
            MatchResult wrapped = result.Matches[SqlGrammar.WrappedItem.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.WrappedItem.Wrapped.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult additiveExpression = wrapped.Matches[SqlGrammar.WrappedItem.Wrapped.AdditiveExpression];
                buildAdditiveExpression(additiveExpression, writer);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.WrappedItem.Wrapped.RightParenthesis];
                writeToken(rightParenthesis, writer);
                return;
            }
            MatchResult item = result.Matches[SqlGrammar.WrappedItem.Item];
            if (item.IsMatch)
            {
                buildItem(item, writer);
                return;
            }
        }

        private void buildItem(MatchResult result, TextWriter writer)
        {
            MatchResult numberToken = result.Matches[SqlGrammar.Item.Number];
            if (numberToken.IsMatch)
            {
                writeToken(numberToken, writer);
                return;
            }
            MatchResult stringToken = result.Matches[SqlGrammar.Item.String];
            if (stringToken.IsMatch)
            {
                writeToken(stringToken, writer);
                return;
            }
            MatchResult nullToken = result.Matches[SqlGrammar.Item.Null];
            if (nullToken.IsMatch)
            {
                writeToken(nullToken, writer);
                return;
            }
            MatchResult functionCall = result.Matches[SqlGrammar.Item.FunctionCall];
            if (functionCall.IsMatch)
            {
                buildFunctionCall(functionCall, writer);
                return;
            }
            MatchResult column = result.Matches[SqlGrammar.Item.Column];
            if (column.IsMatch)
            {
                buildMultipartIdentifier(column, writer);
                return;
            }
            MatchResult caseExpression = result.Matches[SqlGrammar.Item.Case];
            if (caseExpression.IsMatch)
            {
                buildCase(caseExpression, writer);
                return;
            }
            MatchResult select = result.Matches[SqlGrammar.Item.Select.Name];
            if (select.IsMatch)
            {
                MatchResult leftParenthesis = select.Matches[SqlGrammar.Item.Select.LeftParenthesis];
                writeToken(leftParenthesis, writer);
                MatchResult selectStatement = select.Matches[SqlGrammar.Item.Select.SelectStatement];
                buildSelectStatement(selectStatement, writer);
                MatchResult rightParenthesis = select.Matches[SqlGrammar.Item.Select.RightParenthesis];
                writeToken(rightParenthesis, writer);
                return;
            }
        }

        private void buildCase(MatchResult result, TextWriter writer)
        {
            MatchResult caseResult = result.Matches[SqlGrammar.Case.CaseKeyword];
            writeToken(caseResult, writer);
            writer.Write(' ');
            MatchResult expression = result.Matches[SqlGrammar.Case.Expression];
            buildArithmeticItem(expression, writer);
            writer.Write(' ');
            MatchResult matchList = result.Matches[SqlGrammar.Case.MatchList];
            buildMatchList(matchList, writer);
            writer.Write(' ');
            MatchResult end = result.Matches[SqlGrammar.Case.EndKeyword];
            writeToken(end, writer);
        }

        private void buildMatchList(MatchResult result, TextWriter writer)
        {
            MatchResult match = result.Matches[SqlGrammar.MatchList.Match];
            buildMatch(match, writer);
            MatchResult matchListPrime = result.Matches[SqlGrammar.MatchList.MatchListPrime];
            buildMatchListPrime(matchListPrime, writer);
        }

        private void buildMatchListPrime(MatchResult result, TextWriter writer)
        {
            MatchResult match = result.Matches[SqlGrammar.MatchListPrime.Match.Name];
            if (match.IsMatch)
            {
                writer.Write(' ');
                MatchResult first = match.Matches[SqlGrammar.MatchListPrime.Match.First];
                buildMatch(first, writer);
                MatchResult remaining = match.Matches[SqlGrammar.MatchListPrime.Match.Remaining];
                buildMatchListPrime(remaining, writer);
                return;
            }
            MatchResult elseResult = result.Matches[SqlGrammar.MatchListPrime.Else.Name];
            if (elseResult.IsMatch)
            {
                writer.Write(' ');
                MatchResult elseKeyword = elseResult.Matches[SqlGrammar.MatchListPrime.Else.ElseKeyword];
                writeToken(elseKeyword, writer);
                writer.Write(' ');
                MatchResult value = elseResult.Matches[SqlGrammar.MatchListPrime.Else.Value];
                buildArithmeticItem(value, writer);
                return;
            }
            MatchResult empty = result.Matches[SqlGrammar.MatchListPrime.Empty];
            if (empty.IsMatch)
            {
                return;
            }
        }

        private void buildMatch(MatchResult result, TextWriter writer)
        {
            MatchResult when = result.Matches[SqlGrammar.Match.WhenKeyword];
            writeToken(when, writer);
            writer.Write(' ');
            MatchResult expression = result.Matches[SqlGrammar.Match.Expression];
            buildArithmeticItem(expression, writer);
            writer.Write(' ');
            MatchResult then = result.Matches[SqlGrammar.Match.ThenKeyword];
            writeToken(then, writer);
            writer.Write(' ');
            MatchResult value = result.Matches[SqlGrammar.Match.Value];
            buildArithmeticItem(value, writer);
        }

        private void buildFunctionCall(MatchResult result, TextWriter writer)
        {
            MatchResult functionName = result.Matches[SqlGrammar.FunctionCall.FunctionName];
            buildMultipartIdentifier(functionName, writer);
            MatchResult leftParenthesis = result.Matches[SqlGrammar.FunctionCall.LeftParethesis];
            writeToken(leftParenthesis, writer);
            MatchResult arguments = result.Matches[SqlGrammar.FunctionCall.Arguments];
            buildValueList(arguments, writer);
            MatchResult rightParenthesis = result.Matches[SqlGrammar.FunctionCall.RightParenthesis];
            writeToken(rightParenthesis, writer);
            MatchResult window = result.Matches[SqlGrammar.FunctionCall.Window.Name];
            if (window.IsMatch)
            {
                writer.Write(' ');
                MatchResult over = window.Matches[SqlGrammar.FunctionCall.Window.Over];
                writeToken(over, writer);
                writer.Write(' ');
                MatchResult windowLeftParenthesis = window.Matches[SqlGrammar.FunctionCall.Window.LeftParenthesis];
                writeToken(windowLeftParenthesis, writer);
                bool needsSpace = false;
                MatchResult partitioning = window.Matches[SqlGrammar.FunctionCall.Window.Partitioning.Name];
                if (partitioning.IsMatch)
                {
                    needsSpace = true;
                    MatchResult partitionBy = partitioning.Matches[SqlGrammar.FunctionCall.Window.Partitioning.PartitionBy];
                    writeToken(partitionBy, writer);
                    writer.Write(' ');
                    MatchResult valueList = partitioning.Matches[SqlGrammar.FunctionCall.Window.Partitioning.ValueList];
                    buildValueList(valueList, writer);
                }
                MatchResult ordering = window.Matches[SqlGrammar.FunctionCall.Window.Ordering.Name];
                if (ordering.IsMatch)
                {
                    if (needsSpace)
                    {
                        writer.Write(' ');
                    }
                    needsSpace = true;
                    MatchResult orderBy = ordering.Matches[SqlGrammar.FunctionCall.Window.Ordering.OrderByKeyword];
                    writeToken(orderBy, writer);
                    writer.Write(' ');
                    MatchResult orderByList = ordering.Matches[SqlGrammar.FunctionCall.Window.Ordering.OrderByList];
                    buildOrderByList(orderByList, writer);
                }
                MatchResult framing = window.Matches[SqlGrammar.FunctionCall.Window.Framing.Name];
                if (framing.IsMatch)
                {
                    if (needsSpace)
                    {
                        writer.Write(' ');
                    }
                    MatchResult frameType = framing.Matches[SqlGrammar.FunctionCall.Window.Framing.FrameType];
                    buildFrameType(frameType, writer);
                    writer.Write(' ');
                    MatchResult start = framing.Matches[SqlGrammar.FunctionCall.Window.Framing.PrecedingFrame];
                    if (start.IsMatch)
                    {
                        buildPrecedingFrame(start, writer);
                    }
                    MatchResult between = framing.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.Name];
                    if (between.IsMatch)
                    {
                        MatchResult betweenKeyword = between.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.BetweenKeyword];
                        writeToken(betweenKeyword, writer);
                        writer.Write(' ');
                        MatchResult precedingFrame = between.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.PrecedingFrame];
                        buildPrecedingFrame(precedingFrame, writer);
                        writer.Write(' ');
                        MatchResult andKeyword = between.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.AndKeyword];
                        writeToken(andKeyword, writer);
                        writer.Write(' ');
                        MatchResult followingFrame = between.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.FollowingFrame];
                        buildFollowingFrame(followingFrame, writer);
                    }
                }
                MatchResult windowRightParenthesis = window.Matches[SqlGrammar.FunctionCall.Window.RightParenthesis];
                writeToken(windowRightParenthesis, writer);
            }
        }

        private void buildFrameType(MatchResult result, TextWriter writer)
        {
            MatchResult rows = result.Matches[SqlGrammar.FrameType.Rows];
            if (rows.IsMatch)
            {
                writeToken(rows, writer);
                return;
            }
            MatchResult range = result.Matches[SqlGrammar.FrameType.Range];
            if (range.IsMatch)
            {
                writeToken(range, writer);
                return;
            }
        }

        private void buildPrecedingFrame(MatchResult result, TextWriter writer)
        {
            MatchResult unbounded = result.Matches[SqlGrammar.PrecedingFrame.UnboundedPreceding.Name];
            if (unbounded.IsMatch)
            {
                MatchResult unboundedKeyword = unbounded.Matches[SqlGrammar.PrecedingFrame.UnboundedPreceding.UnboundedKeyword];
                writeToken(unboundedKeyword, writer);
                writer.Write(' ');
                MatchResult precedingKeyword = unbounded.Matches[SqlGrammar.PrecedingFrame.UnboundedPreceding.PrecedingKeyword];
                writeToken(precedingKeyword, writer);
                return;
            }
            MatchResult bounded = result.Matches[SqlGrammar.PrecedingFrame.BoundedPreceding.Name];
            if (bounded.IsMatch)
            {
                MatchResult number = bounded.Matches[SqlGrammar.PrecedingFrame.BoundedPreceding.Number];
                writeToken(number, writer);
                writer.Write(' ');
                MatchResult precedingKeyword = bounded.Matches[SqlGrammar.PrecedingFrame.BoundedPreceding.PrecedingKeyword];
                writeToken(precedingKeyword, writer);
                return;
            }
            MatchResult currentRow = result.Matches[SqlGrammar.PrecedingFrame.CurrentRow];
            if (currentRow.IsMatch)
            {
                writeToken(currentRow, writer);
                return;
            }
        }

        private void buildFollowingFrame(MatchResult result, TextWriter writer)
        {
            MatchResult unbounded = result.Matches[SqlGrammar.FollowingFrame.UnboundedFollowing.Name];
            if (unbounded.IsMatch)
            {
                MatchResult unboundedKeyword = unbounded.Matches[SqlGrammar.FollowingFrame.UnboundedFollowing.UnboundedKeyword];
                writeToken(unboundedKeyword, writer);
                writer.Write(' ');
                MatchResult followingKeyword = unbounded.Matches[SqlGrammar.FollowingFrame.UnboundedFollowing.FollowingKeyword];
                writeToken(followingKeyword, writer);
                return;
            }
            MatchResult bounded = result.Matches[SqlGrammar.FollowingFrame.BoundedFollowing.Name];
            if (bounded.IsMatch)
            {
                MatchResult number = bounded.Matches[SqlGrammar.FollowingFrame.BoundedFollowing.Number];
                writeToken(number, writer);
                writer.Write(' ');
                MatchResult followingKeyword = bounded.Matches[SqlGrammar.FollowingFrame.BoundedFollowing.FollowingKeyword];
                writeToken(followingKeyword, writer);
                return;
            }
            MatchResult currentRow = result.Matches[SqlGrammar.FollowingFrame.CurrentRow];
            if (currentRow.IsMatch)
            {
                writeToken(currentRow, writer);
                return;
            }
        }

        private void buildValueList(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ValueList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.ValueList.Multiple.First];
                buildArithmeticItem(first, writer);
                MatchResult comma = multiple.Matches[SqlGrammar.ValueList.Multiple.Comma];
                writeToken(comma, writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ValueList.Multiple.Remaining];
                buildValueList(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ValueList.Single];
            if (single.IsMatch)
            {
                buildArithmeticItem(single, writer);
                return;
            }
        }

        private void buildMultipartIdentifier(MatchResult result, TextWriter writer)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultipartIdentifier.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.First];
                writeToken(first, writer);
                MatchResult dot = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Dot];
                writeToken(dot, writer);
                MatchResult remaining = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Remaining];
                buildMultipartIdentifier(remaining, writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.MultipartIdentifier.Single];
            if (single.IsMatch)
            {
                writeToken(single, writer);
                return;
            }
        }

        private void writeToken(MatchResult result, TextWriter writer)
        {
            TokenResult tokenResult = (TokenResult)result.Context;
            writer.Write(tokenResult.Value);
        }
    }
}
