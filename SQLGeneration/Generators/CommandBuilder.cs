using System;
using System.Collections.Generic;
using System.Linq;
using SQLGeneration.Builders;
using SQLGeneration.Parsing;
using SQLGeneration.Properties;

namespace SQLGeneration.Generators
{
    /// <summary>
    /// Builds an ICommand from a SQL statement.
    /// </summary>
    public sealed class CommandBuilder : SqlGenerator
    {
        private SourceScope scope;
        private CommandBuilderOptions options;

        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="registry">The token registry to use.</param>
        public CommandBuilder(SqlTokenRegistry registry)
            : this(new SqlGrammar(registry))
        {
        }

        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        public CommandBuilder(SqlGrammar grammar = null)
            : base(grammar)
        {
        }

        /// <summary>
        /// Parses the given command text to build a command builder.
        /// </summary>
        /// <param name="commandText">The command text to parse.</param>
        /// <param name="options">Configures the behavior of the command builder.</param>
        /// <returns>The command that was parsed.</returns>
        public ICommand GetCommand(string commandText, CommandBuilderOptions options = null)
        {
            this.scope = new SourceScope();
            this.options = options ?? new CommandBuilderOptions();
            ITokenSource tokenSource = Grammar.TokenRegistry.CreateTokenSource(commandText);
            MatchResult result = GetResult(tokenSource);
            return buildStart(result);
        }

        private ICommand buildStart(MatchResult result)
        {
            MatchResult select = result.Matches[SqlGrammar.Start.SelectStatement];
            if (select.IsMatch)
            {
                return buildSelectStatement(select);
            }
            MatchResult insert = result.Matches[SqlGrammar.Start.InsertStatement];
            if (insert.IsMatch)
            {
                return buildInsertStatement(insert);
            }
            MatchResult update = result.Matches[SqlGrammar.Start.UpdateStatement];
            if (update.IsMatch)
            {
                return buildUpdateStatement(update);
            }
            MatchResult delete = result.Matches[SqlGrammar.Start.DeleteStatement];
            if (delete.IsMatch)
            {
                return buildDeleteStatement(delete);
            }
            throw new InvalidOperationException();
        }

        private ISelectBuilder buildSelectStatement(MatchResult result)
        {
            MatchResult selectExpression = result.Matches[SqlGrammar.SelectStatement.SelectExpression];
            ISelectBuilder builder = buildSelectExpression(selectExpression);
            MatchResult orderBy = result.Matches[SqlGrammar.SelectStatement.OrderBy.Name];
            if (orderBy.IsMatch)
            {
                scope.Push(builder.Sources);
                MatchResult orderByList = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByList];
                buildOrderByList(orderByList, builder.OrderByList);
                scope.Pop();
            }
            return builder;
        }

        private ISelectBuilder buildSelectExpression(MatchResult result)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.SelectExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult expression = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.SelectExpression];
                return buildSelectExpression(expression);
            }
            MatchResult specification = result.Matches[SqlGrammar.SelectExpression.SelectSpecification];
            ISelectBuilder builder = buildSelectSpecification(specification);
            MatchResult remaining = result.Matches[SqlGrammar.SelectExpression.Remaining.Name];
            if (remaining.IsMatch)
            {
                MatchResult expression = remaining.Matches[SqlGrammar.SelectExpression.Remaining.SelectExpression];
                ISelectBuilder rightHand = buildSelectExpression(expression);
                MatchResult combinerResult = remaining.Matches[SqlGrammar.SelectExpression.Remaining.Combiner];
                SelectCombiner combiner = buildSelectCombiner(combinerResult, builder, rightHand);
                MatchResult qualifierResult = remaining.Matches[SqlGrammar.SelectExpression.Remaining.DistinctQualifier];
                if (qualifierResult.IsMatch)
                {
                    combiner.Distinct = buildDistinctQualifier(qualifierResult);
                }
                builder = combiner;
            }
            return builder;
        }

        private ISelectBuilder buildSelectSpecification(MatchResult result)
        {
            SelectBuilder builder = new SelectBuilder();
            MatchResult distinctQualifier = result.Matches[SqlGrammar.SelectSpecification.DistinctQualifier];
            if (distinctQualifier.IsMatch)
            {
                builder.Distinct = buildDistinctQualifier(distinctQualifier);
            }
            MatchResult top = result.Matches[SqlGrammar.SelectSpecification.Top.Name];
            if (top.IsMatch)
            {
                builder.Top = buildTop(top, builder);
            }
            MatchResult from = result.Matches[SqlGrammar.SelectSpecification.From.Name];
            if (from.IsMatch)
            {
                MatchResult fromList = from.Matches[SqlGrammar.SelectSpecification.From.FromList];
                buildFromList(fromList, builder);
            }
            scope.Push(builder.Sources);
            MatchResult projectionList = result.Matches[SqlGrammar.SelectSpecification.ProjectionList];
            buildProjectionList(projectionList, builder);
            MatchResult where = result.Matches[SqlGrammar.SelectSpecification.Where.Name];
            if (where.IsMatch)
            {
                MatchResult filterList = where.Matches[SqlGrammar.SelectSpecification.Where.FilterList];
                IFilter innerFilter = buildOrFilter(filterList);
                builder.WhereFilterGroup.AddFilter(innerFilter);
                builder.WhereFilterGroup.Optimize();
            }
            MatchResult groupBy = result.Matches[SqlGrammar.SelectSpecification.GroupBy.Name];
            if (groupBy.IsMatch)
            {
                MatchResult groupByList = groupBy.Matches[SqlGrammar.SelectSpecification.GroupBy.GroupByList];
                buildGroupByList(groupByList, builder);
            }
            MatchResult having = result.Matches[SqlGrammar.SelectSpecification.Having.Name];
            if (having.IsMatch)
            {
                MatchResult filterList = having.Matches[SqlGrammar.SelectSpecification.Having.FilterList];
                IFilter innerFilter = buildOrFilter(filterList);
                builder.HavingFilterGroup.AddFilter(innerFilter);
                builder.HavingFilterGroup.Optimize();
            }
            scope.Pop();
            return builder;
        }

        private DistinctQualifier buildDistinctQualifier(MatchResult result)
        {
            DistinctQualifierConverter converter = new DistinctQualifierConverter();
            MatchResult distinct = result.Matches[SqlGrammar.DistinctQualifier.Distinct];
            if (distinct.IsMatch)
            {
                return DistinctQualifier.Distinct;
            }
            MatchResult all = result.Matches[SqlGrammar.DistinctQualifier.All];
            if (all.IsMatch)
            {
                return DistinctQualifier.All;
            }
            throw new InvalidOperationException();
        }

        private Top buildTop(MatchResult result, SelectBuilder builder)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.SelectSpecification.Top.Expression];
            IProjectionItem expression = (IProjectionItem)buildArithmeticItem(expressionResult);
            Top top = new Top(expression);
            MatchResult percentResult = result.Matches[SqlGrammar.SelectSpecification.Top.PercentKeyword];
            top.IsPercent = percentResult.IsMatch;
            MatchResult withTiesResult = result.Matches[SqlGrammar.SelectSpecification.Top.WithTiesKeyword];
            top.WithTies = withTiesResult.IsMatch;
            return top;
        }

        private void buildFromList(MatchResult result, SelectBuilder builder)
        {
            MatchResult multiple = result.Matches[SqlGrammar.FromList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.FromList.Multiple.First];
                Join join = buildJoin(first, false);
                addJoinItem(builder, join);
                MatchResult remaining = multiple.Matches[SqlGrammar.FromList.Multiple.Remaining];
                buildFromList(remaining, builder);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.FromList.Single];
            if (single.IsMatch)
            {
                Join join = buildJoin(single, false);
                addJoinItem(builder, join);
                return;
            }
            throw new InvalidOperationException();
        }

        private Join buildJoin(MatchResult result, bool wrap)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Join.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult joinResult = wrapped.Matches[SqlGrammar.Join.Wrapped.Join];
                Join first = buildJoin(joinResult, true);
                first.WrapInParentheses = true;
                scope.Push(first.Sources);
                MatchResult joinPrime = wrapped.Matches[SqlGrammar.Join.Wrapped.JoinPrime];
                Join join = buildJoinPrime(joinPrime, first);
                scope.Pop();
                return join;
            }
            MatchResult joined = result.Matches[SqlGrammar.Join.Joined.Name];
            if (joined.IsMatch)
            {
                string alias;
                MatchResult joinItemResult = joined.Matches[SqlGrammar.Join.Joined.JoinItem];
                IRightJoinItem first = buildJoinItem(joinItemResult, out alias);
                Join start = Join.From(first, alias);
                scope.Push(start.Sources);
                MatchResult joinPrime = joined.Matches[SqlGrammar.Join.Joined.JoinPrime];
                Join join = buildJoinPrime(joinPrime, start);
                scope.Pop();
                return join;
            }
            throw new InvalidOperationException();
        }

        private IRightJoinItem buildJoinItem(MatchResult result, out string alias)
        {
            alias = null;
            MatchResult aliasExpression = result.Matches[SqlGrammar.JoinItem.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult aliasResult = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.Alias];
                alias = getToken(aliasResult);
            }
            MatchResult tableResult = result.Matches[SqlGrammar.JoinItem.Table];
            if (tableResult.IsMatch)
            {
                Table table = buildTable(tableResult);
                return table;
            }
            MatchResult select = result.Matches[SqlGrammar.JoinItem.Select.Name];
            if (select.IsMatch)
            {
                MatchResult selectStatement = select.Matches[SqlGrammar.JoinItem.Select.SelectStatement];
                return buildSelectStatement(selectStatement);
            }
            MatchResult functionCall = result.Matches[SqlGrammar.JoinItem.FunctionCall];
            if (functionCall.IsMatch)
            {
                return buildFunctionCall(functionCall);
            }
            throw new InvalidOperationException();
        }

        private Join buildJoinPrime(MatchResult result, Join join)
        {
            MatchResult filtered = result.Matches[SqlGrammar.JoinPrime.Filtered.Name];
            if (filtered.IsMatch)
            {
                MatchResult joinItemResult = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinItem];
                string alias;
                IRightJoinItem joinItem = buildJoinItem(joinItemResult, out alias);
                MatchResult joinTypeResult = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinType];
                FilteredJoin filteredJoin = buildFilteredJoin(joinTypeResult, join, joinItem, alias);
                scope.Push(filteredJoin.Sources);
                MatchResult onResult = filtered.Matches[SqlGrammar.JoinPrime.Filtered.On.Name];
                MatchResult filterListResult = onResult.Matches[SqlGrammar.JoinPrime.Filtered.On.FilterList];
                IFilter innerFilter = buildOrFilter(filterListResult);
                filteredJoin.OnFilterGroup.AddFilter(innerFilter);
                filteredJoin.OnFilterGroup.Optimize();
                MatchResult joinPrimeResult = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinPrime];
                Join prime = buildJoinPrime(joinPrimeResult, filteredJoin);
                scope.Pop();
                return prime;
            }
            MatchResult cross = result.Matches[SqlGrammar.JoinPrime.Cross.Name];
            if (cross.IsMatch)
            {
                MatchResult joinItemResult = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinItem];
                string alias;
                IRightJoinItem joinItem = buildJoinItem(joinItemResult, out alias);
                Join crossJoin = join.CrossJoin(joinItem, alias);
                scope.Push(crossJoin.Sources);
                MatchResult joinPrimeResult = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinPrime];
                Join prime = buildJoinPrime(joinPrimeResult, crossJoin);
                scope.Pop();
                return prime;
            }
            MatchResult empty = result.Matches[SqlGrammar.JoinPrime.Empty];
            if (empty.IsMatch)
            {
                return join;
            }
            throw new InvalidOperationException();
        }

        private FilteredJoin buildFilteredJoin(MatchResult result, Join join, IRightJoinItem joinItem, string alias)
        {
            MatchResult innerResult = result.Matches[SqlGrammar.FilteredJoinType.InnerJoin];
            if (innerResult.IsMatch)
            {
                return join.InnerJoin(joinItem, alias);
            }
            MatchResult leftResult = result.Matches[SqlGrammar.FilteredJoinType.LeftOuterJoin];
            if (leftResult.IsMatch)
            {
                return join.LeftOuterJoin(joinItem, alias);
            }
            MatchResult rightResult = result.Matches[SqlGrammar.FilteredJoinType.RightOuterJoin];
            if (rightResult.IsMatch)
            {
                return join.RightOuterJoin(joinItem, alias);
            }
            MatchResult fullResult = result.Matches[SqlGrammar.FilteredJoinType.FullOuterJoin];
            if (fullResult.IsMatch)
            {
                return join.FullOuterJoin(joinItem, alias);
            }
            throw new InvalidOperationException();
        }

        private void addJoinItem(SelectBuilder builder, Join join)
        {
            JoinStart start = join as JoinStart;
            if (start == null)
            {
                builder.AddJoin(join);
                return;
            }
            AliasedSource source = start.Source;
            Table table = source.Source as Table;
            if (table != null)
            {
                builder.AddTable(table, source.Alias);
                return;
            }
            ISelectBuilder select = source.Source as SelectBuilder;
            if (select != null)
            {
                builder.AddSelect(select, source.Alias);
                return;
            }
            Function functionCall = source.Source as Function;
            if (functionCall != null)
            {
                builder.AddFunction(functionCall, source.Alias);
                return;
            }
            throw new InvalidOperationException();
        }

        private void buildProjectionList(MatchResult result, SelectBuilder builder)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ProjectionList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.ProjectionList.Multiple.First];
                buildProjectionItem(first, builder);
                MatchResult remaining = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Remaining];
                buildProjectionList(remaining, builder);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ProjectionList.Single];
            if (single.IsMatch)
            {
                buildProjectionItem(single, builder);
                return;
            }
            throw new InvalidOperationException();
        }

        private void buildProjectionItem(MatchResult result, SelectBuilder builder)
        {
            MatchResult expression = result.Matches[SqlGrammar.ProjectionItem.Expression.Name];
            if (expression.IsMatch)
            {
                MatchResult itemResult = expression.Matches[SqlGrammar.ProjectionItem.Expression.Item];
                IProjectionItem item = (IProjectionItem)buildArithmeticItem(itemResult);
                string alias = null;
                MatchResult aliasExpression = expression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Name];
                if (aliasExpression.IsMatch)
                {
                    MatchResult aliasResult = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Alias];
                    alias = getToken(aliasResult);
                }
                builder.AddProjection(item, alias);
                return;
            }
            MatchResult star = result.Matches[SqlGrammar.ProjectionItem.Star.Name];
            if (star.IsMatch)
            {
                AliasedSource source = null;
                MatchResult qualifier = star.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Name];
                if (qualifier.IsMatch)
                {
                    MatchResult columnSource = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.ColumnSource];
                    List<string> parts = new List<string>();
                    buildMultipartIdentifier(columnSource, parts);
                    string sourceName = parts[parts.Count - 1];
                    source = scope.GetSource(sourceName);
                }
                AllColumns all = new AllColumns(source);
                builder.AddProjection(all);
                return;
            }
            throw new InvalidOperationException();
        }

        private void buildGroupByList(MatchResult result, SelectBuilder builder)
        {
            MatchResult multiple = result.Matches[SqlGrammar.GroupByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult firstResult = multiple.Matches[SqlGrammar.GroupByList.Multiple.First];
                IGroupByItem first = (IGroupByItem)buildArithmeticItem(firstResult);
                builder.AddGroupBy(first);
                MatchResult remainingResult = multiple.Matches[SqlGrammar.GroupByList.Multiple.Remaining];
                buildGroupByList(remainingResult, builder);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.GroupByList.Single];
            if (single.IsMatch)
            {
                IGroupByItem item = (IGroupByItem)buildArithmeticItem(single);
                builder.AddGroupBy(item);
                return;
            }
            throw new InvalidOperationException();
        }

        private IFilter buildOrFilter(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrFilter.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.OrFilter.Multiple.First];
                IFilter firstFilter = buildAndFilter(first);
                MatchResult remaining = multiple.Matches[SqlGrammar.OrFilter.Multiple.Remaining];
                IFilter remainingFilter = buildOrFilter(remaining);
                return new FilterGroup(Conjunction.Or, firstFilter, remainingFilter);
            }
            MatchResult single = result.Matches[SqlGrammar.OrFilter.Single];
            if (single.IsMatch)
            {
                return buildAndFilter(single);
            }
            throw new InvalidOperationException();
        }

        private IFilter buildAndFilter(MatchResult result)
        {
            MatchResult multiple = result.Matches[SqlGrammar.AndFilter.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.AndFilter.Multiple.First];
                IFilter firstFilter = buildFilter(first);
                MatchResult remaining = multiple.Matches[SqlGrammar.AndFilter.Multiple.Remaining];
                IFilter remainingFilter = buildOrFilter(remaining);
                return new FilterGroup(Conjunction.And, firstFilter, remainingFilter);
            }
            MatchResult single = result.Matches[SqlGrammar.AndFilter.Single];
            if (single.IsMatch)
            {
                return buildFilter(single);
            }
            throw new InvalidOperationException();
        }

        private IFilter buildFilter(MatchResult result)
        {
            MatchResult notResult = result.Matches[SqlGrammar.Filter.Not.Name];
            if (notResult.IsMatch)
            {
                MatchResult filterResult = notResult.Matches[SqlGrammar.Filter.Not.Filter];
                IFilter filter = buildFilter(filterResult);
                return new NotFilter(filter);
            }
            MatchResult wrappedResult = result.Matches[SqlGrammar.Filter.Wrapped.Name];
            if (wrappedResult.IsMatch)
            {
                MatchResult filterResult = wrappedResult.Matches[SqlGrammar.Filter.Wrapped.Filter];
                FilterGroup nested = new FilterGroup();
                IFilter innerFilter = buildOrFilter(filterResult);
                nested.AddFilter(innerFilter);
                nested.WrapInParentheses = true;
                return nested;
            }
            MatchResult quantifyResult = result.Matches[SqlGrammar.Filter.Quantify.Name];
            if (quantifyResult.IsMatch)
            {
                MatchResult expressionResult = quantifyResult.Matches[SqlGrammar.Filter.Quantify.Expression];
                IFilterItem filterItem = (IFilterItem)buildArithmeticItem(expressionResult);
                MatchResult quantifierResult = quantifyResult.Matches[SqlGrammar.Filter.Quantify.Quantifier];
                Quantifier quantifier = buildQuantifier(quantifierResult);
                IValueProvider valueProvider = null;
                MatchResult selectResult = quantifyResult.Matches[SqlGrammar.Filter.Quantify.SelectStatement];
                if (selectResult.IsMatch)
                {
                    valueProvider = buildSelectStatement(selectResult);
                }
                MatchResult valueListResult = quantifyResult.Matches[SqlGrammar.Filter.Quantify.ValueList];
                if (valueListResult.IsMatch)
                {
                    ValueList values = new ValueList();
                    buildValueList(valueListResult, values);
                    valueProvider = values;
                }
                MatchResult operatorResult = quantifyResult.Matches[SqlGrammar.Filter.Quantify.ComparisonOperator];
                return buildQuantifierFilter(operatorResult, filterItem, quantifier, valueProvider);
            }
            MatchResult functionResult = result.Matches[SqlGrammar.Filter.Function.Name];
            if (functionResult.IsMatch)
            {
                MatchResult expressionResult = functionResult.Matches[SqlGrammar.Filter.Function.Expression];
                return buildFunctionCall(expressionResult);
            }
            MatchResult orderResult = result.Matches[SqlGrammar.Filter.Order.Name];
            if (orderResult.IsMatch)
            {
                MatchResult leftResult = orderResult.Matches[SqlGrammar.Filter.Order.Left];
                IFilterItem left = (IFilterItem)buildArithmeticItem(leftResult);
                MatchResult rightResult = orderResult.Matches[SqlGrammar.Filter.Order.Right];
                IFilterItem right = (IFilterItem)buildArithmeticItem(rightResult);
                MatchResult operatorResult = orderResult.Matches[SqlGrammar.Filter.Order.ComparisonOperator];
                return buildOrderFilter(operatorResult, left, right);
            }
            MatchResult betweenResult = result.Matches[SqlGrammar.Filter.Between.Name];
            if (betweenResult.IsMatch)
            {
                MatchResult expressionResult = betweenResult.Matches[SqlGrammar.Filter.Between.Expression];
                IFilterItem expression = (IFilterItem)buildArithmeticItem(expressionResult);
                MatchResult lowerBoundResult = betweenResult.Matches[SqlGrammar.Filter.Between.LowerBound];
                IFilterItem lowerBound = (IFilterItem)buildArithmeticItem(lowerBoundResult);
                MatchResult upperBoundResult = betweenResult.Matches[SqlGrammar.Filter.Between.UpperBound];
                IFilterItem upperBound = (IFilterItem)buildArithmeticItem(upperBoundResult);
                BetweenFilter filter = new BetweenFilter(expression, lowerBound, upperBound);
                MatchResult betweenNotResult = betweenResult.Matches[SqlGrammar.Filter.Between.NotKeyword];
                filter.Not = betweenNotResult.IsMatch;
                return filter;
            }
            MatchResult likeResult = result.Matches[SqlGrammar.Filter.Like.Name];
            if (likeResult.IsMatch)
            {
                MatchResult leftResult = likeResult.Matches[SqlGrammar.Filter.Like.Left];
                IFilterItem left = (IFilterItem)buildArithmeticItem(leftResult);
                MatchResult rightResult = likeResult.Matches[SqlGrammar.Filter.Like.Right];
                IFilterItem right = (IFilterItem)buildArithmeticItem(rightResult);
                LikeFilter filter = new LikeFilter(left, right);
                MatchResult likeNotResult = likeResult.Matches[SqlGrammar.Filter.Like.NotKeyword];
                filter.Not = likeNotResult.IsMatch;
                return filter;
            }
            MatchResult isResult = result.Matches[SqlGrammar.Filter.Is.Name];
            if (isResult.IsMatch)
            {
                MatchResult expressionResult = isResult.Matches[SqlGrammar.Filter.Is.Expression];
                IFilterItem expression = (IFilterItem)buildArithmeticItem(expressionResult);
                NullFilter filter = new NullFilter(expression);
                MatchResult isNotResult = isResult.Matches[SqlGrammar.Filter.Is.NotKeyword];
                filter.Not = isNotResult.IsMatch;
                return filter;
            }
            MatchResult inResult = result.Matches[SqlGrammar.Filter.In.Name];
            if (inResult.IsMatch)
            {
                MatchResult expressionResult = inResult.Matches[SqlGrammar.Filter.In.Expression];
                IFilterItem expression = (IFilterItem)buildArithmeticItem(expressionResult);
                IValueProvider valueProvider = null;
                MatchResult valuesResult = inResult.Matches[SqlGrammar.Filter.In.Values.Name];
                if (valuesResult.IsMatch)
                {
                    MatchResult valueListResult = valuesResult.Matches[SqlGrammar.Filter.In.Values.ValueList];
                    ValueList values = new ValueList();
                    buildValueList(valueListResult, values);
                    valueProvider = values;
                }
                MatchResult selectResult = inResult.Matches[SqlGrammar.Filter.In.Select.Name];
                if (selectResult.IsMatch)
                {
                    MatchResult selectStatementResult = selectResult.Matches[SqlGrammar.Filter.In.Select.SelectStatement];
                    valueProvider = buildSelectStatement(selectStatementResult);
                }
                MatchResult functionCall = inResult.Matches[SqlGrammar.Filter.In.FunctionCall];
                if (functionCall.IsMatch)
                {
                    valueProvider = buildFunctionCall(functionCall);
                }
                InFilter filter = new InFilter(expression, valueProvider);
                MatchResult inNotResult = inResult.Matches[SqlGrammar.Filter.In.NotKeyword];
                filter.Not = inNotResult.IsMatch;
                return filter;
            }
            MatchResult existsResult = result.Matches[SqlGrammar.Filter.Exists.Name];
            if (existsResult.IsMatch)
            {
                MatchResult selectExpressionResult = existsResult.Matches[SqlGrammar.Filter.Exists.SelectStatement];
                ISelectBuilder builder = buildSelectStatement(selectExpressionResult);
                ExistsFilter filter = new ExistsFilter(builder);
                return filter;
            }
            throw new InvalidOperationException();
        }

        private Quantifier buildQuantifier(MatchResult result)
        {
            MatchResult all = result.Matches[SqlGrammar.Quantifier.All];
            if (all.IsMatch)
            {
                return Quantifier.All;
            }
            MatchResult any = result.Matches[SqlGrammar.Quantifier.Any];
            if (any.IsMatch)
            {
                return Quantifier.Any;
            }
            MatchResult some = result.Matches[SqlGrammar.Quantifier.Some];
            if (some.IsMatch)
            {
                return Quantifier.Some;
            }
            throw new InvalidOperationException();
        }

        private IFilter buildQuantifierFilter(MatchResult result, IFilterItem leftHand, Quantifier quantifier, IValueProvider valueProvider)
        {
            MatchResult equalToResult = result.Matches[SqlGrammar.ComparisonOperator.EqualTo];
            if (equalToResult.IsMatch)
            {
                return new EqualToQuantifierFilter(leftHand, quantifier, valueProvider);
            }
            MatchResult notEqualToResult = result.Matches[SqlGrammar.ComparisonOperator.NotEqualTo];
            if (notEqualToResult.IsMatch)
            {
                return new NotEqualToQuantifierFilter(leftHand, quantifier, valueProvider);
            }
            MatchResult lessThanEqualToResult = result.Matches[SqlGrammar.ComparisonOperator.LessThanEqualTo];
            if (lessThanEqualToResult.IsMatch)
            {
                return new LessThanEqualToQuantifierFilter(leftHand, quantifier, valueProvider);
            }
            MatchResult greaterThanEqualToResult = result.Matches[SqlGrammar.ComparisonOperator.GreaterThanEqualTo];
            if (greaterThanEqualToResult.IsMatch)
            {
                return new GreaterThanEqualToQuantifierFilter(leftHand, quantifier, valueProvider);
            }
            MatchResult lessThanResult = result.Matches[SqlGrammar.ComparisonOperator.LessThan];
            if (lessThanResult.IsMatch)
            {
                return new LessThanQuantifierFilter(leftHand, quantifier, valueProvider);
            }
            MatchResult greaterThanResult = result.Matches[SqlGrammar.ComparisonOperator.GreaterThan];
            if (greaterThanResult.IsMatch)
            {
                return new GreaterThanQuantifierFilter(leftHand, quantifier, valueProvider);
            }
            throw new InvalidOperationException();
        }

        private IFilter buildOrderFilter(MatchResult result, IFilterItem left, IFilterItem right)
        {
            MatchResult equalToResult = result.Matches[SqlGrammar.ComparisonOperator.EqualTo];
            if (equalToResult.IsMatch)
            {
                return new EqualToFilter(left, right);
            }
            MatchResult notEqualToResult = result.Matches[SqlGrammar.ComparisonOperator.NotEqualTo];
            if (notEqualToResult.IsMatch)
            {
                return new NotEqualToFilter(left, right);
            }
            MatchResult lessThanEqualToResult = result.Matches[SqlGrammar.ComparisonOperator.LessThanEqualTo];
            if (lessThanEqualToResult.IsMatch)
            {
                return new LessThanEqualToFilter(left, right);
            }
            MatchResult greaterThanEqualToResult = result.Matches[SqlGrammar.ComparisonOperator.GreaterThanEqualTo];
            if (greaterThanEqualToResult.IsMatch)
            {
                return new GreaterThanEqualToFilter(left, right);
            }
            MatchResult lessThanResult = result.Matches[SqlGrammar.ComparisonOperator.LessThan];
            if (lessThanResult.IsMatch)
            {
                return new LessThanFilter(left, right);
            }
            MatchResult greaterThanResult = result.Matches[SqlGrammar.ComparisonOperator.GreaterThan];
            if (greaterThanResult.IsMatch)
            {
                return new GreaterThanFilter(left, right);
            }
            throw new InvalidOperationException();
        }

        private SelectCombiner buildSelectCombiner(MatchResult result, ISelectBuilder leftHand, ISelectBuilder rightHand)
        {
            MatchResult union = result.Matches[SqlGrammar.SelectCombiner.Union];
            if (union.IsMatch)
            {
                return new Union(leftHand, rightHand);
            }
            MatchResult intersect = result.Matches[SqlGrammar.SelectCombiner.Intersect];
            if (intersect.IsMatch)
            {
                return new Intersect(leftHand, rightHand);
            }
            MatchResult except = result.Matches[SqlGrammar.SelectCombiner.Except];
            if (except.IsMatch)
            {
                return new Except(leftHand, rightHand);
            }
            MatchResult minus = result.Matches[SqlGrammar.SelectCombiner.Minus];
            if (minus.IsMatch)
            {
                return new Minus(leftHand, rightHand);
            }
            throw new InvalidOperationException();
        }

        private void buildOrderByList(MatchResult result, List<OrderBy> orderByList)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrderByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.OrderByList.Multiple.First];
                buildOrderByItem(first, orderByList);
                MatchResult remaining = multiple.Matches[SqlGrammar.OrderByList.Multiple.Remaining];
                buildOrderByList(remaining, orderByList);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.OrderByList.Single];
            if (single.IsMatch)
            {
                buildOrderByItem(single, orderByList);
                return;
            }
            throw new InvalidOperationException();
        }

        private void buildOrderByItem(MatchResult result, List<OrderBy> orderByList)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.OrderByItem.Expression];
            IProjectionItem expression = (IProjectionItem)buildArithmeticItem(expressionResult);
            Order order = Order.Default;
            MatchResult directionResult = result.Matches[SqlGrammar.OrderByItem.OrderDirection];
            if (directionResult.IsMatch)
            {
                order = buildOrderDirection(directionResult);
            }
            NullPlacement placement = NullPlacement.Default;
            MatchResult placementResult = result.Matches[SqlGrammar.OrderByItem.NullPlacement];
            if (placementResult.IsMatch)
            {
                placement = buildNullPlacement(placementResult);
            }
            OrderBy orderBy = new OrderBy(expression, order, placement);
            orderByList.Add(orderBy);
        }

        private Order buildOrderDirection(MatchResult result)
        {
            MatchResult descending = result.Matches[SqlGrammar.OrderDirection.Descending];
            if (descending.IsMatch)
            {
                return Order.Descending;
            }
            MatchResult ascending = result.Matches[SqlGrammar.OrderDirection.Ascending];
            if (ascending.IsMatch)
            {
                return Order.Ascending;
            }
            throw new InvalidOperationException();
        }

        private NullPlacement buildNullPlacement(MatchResult result)
        {
            MatchResult nullsFirst = result.Matches[SqlGrammar.NullPlacement.NullsFirst];
            if (nullsFirst.IsMatch)
            {
                return NullPlacement.First;
            }
            MatchResult nullsLast = result.Matches[SqlGrammar.NullPlacement.NullsLast];
            if (nullsLast.IsMatch)
            {
                return NullPlacement.Last;
            }
            throw new InvalidOperationException();
        }

        private ICommand buildInsertStatement(MatchResult result)
        {
            MatchResult tableResult = result.Matches[SqlGrammar.InsertStatement.Table];
            Table table = buildTable(tableResult);
            IValueProvider valueProvider = null;
            MatchResult valuesResult = result.Matches[SqlGrammar.InsertStatement.Values.Name];
            if (valuesResult.IsMatch)
            {
                ValueList values = new ValueList();
                MatchResult valueListResult = valuesResult.Matches[SqlGrammar.InsertStatement.Values.ValueList];
                if (valueListResult.IsMatch)
                {
                    buildValueList(valueListResult, values);
                }
                valueProvider = values;
            }
            MatchResult selectResult = result.Matches[SqlGrammar.InsertStatement.Select.Name];
            if (selectResult.IsMatch)
            {
                MatchResult selectStatementResult = selectResult.Matches[SqlGrammar.InsertStatement.Select.SelectStatement];
                ISelectBuilder selectBuilder = buildSelectStatement(selectStatementResult);
                valueProvider = selectBuilder;
            }
            string alias = null;
            MatchResult aliasExpressionResult = result.Matches[SqlGrammar.InsertStatement.AliasExpression.Name];
            if (aliasExpressionResult.IsMatch)
            {
                MatchResult aliasResult = aliasExpressionResult.Matches[SqlGrammar.InsertStatement.AliasExpression.Alias];
                alias = getToken(aliasResult);
            }
            InsertBuilder builder = new InsertBuilder(table, valueProvider, alias);
            SourceCollection collection = new SourceCollection();
            collection.AddSource(builder.Table.GetSourceName(), builder.Table);
            scope.Push(collection);
            MatchResult columnsResult = result.Matches[SqlGrammar.InsertStatement.Columns.Name];
            if (columnsResult.IsMatch)
            {
                MatchResult columnListResult = columnsResult.Matches[SqlGrammar.InsertStatement.Columns.ColumnList];
                buildColumnsList(columnListResult, builder);
            }
            scope.Pop();
            return builder;
        }

        private void buildColumnsList(MatchResult result, InsertBuilder builder)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ColumnList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.ColumnList.Multiple.First];
                Column column = buildColumn(first);
                builder.AddColumn(column);
                MatchResult remaining = multiple.Matches[SqlGrammar.ColumnList.Multiple.Remaining];
                buildColumnsList(remaining, builder);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ColumnList.Single];
            if (single.IsMatch)
            {
                Column column = buildColumn(single);
                builder.AddColumn(column);
                return;
            }
            throw new InvalidOperationException();
        }

        private ICommand buildUpdateStatement(MatchResult result)
        {
            MatchResult tableResult = result.Matches[SqlGrammar.UpdateStatement.Table];
            Table table = buildTable(tableResult);
            string alias = null;
            MatchResult aliasExpressionResult = result.Matches[SqlGrammar.UpdateStatement.AliasExpression.Name];
            if (aliasExpressionResult.IsMatch)
            {
                MatchResult aliasResult = aliasExpressionResult.Matches[SqlGrammar.UpdateStatement.AliasExpression.Alias];
                alias = getToken(aliasResult);
            }
            UpdateBuilder builder = new UpdateBuilder(table, alias);
            SourceCollection collection = new SourceCollection();
            collection.AddSource(builder.Table.GetSourceName(), builder.Table);
            scope.Push(collection);
            MatchResult setterListResult = result.Matches[SqlGrammar.UpdateStatement.SetterList];
            buildSetterList(setterListResult, builder);
            MatchResult whereResult = result.Matches[SqlGrammar.UpdateStatement.Where.Name];
            if (whereResult.IsMatch)
            {
                MatchResult filterListResult = whereResult.Matches[SqlGrammar.UpdateStatement.Where.FilterList];
                IFilter innerFilter = buildOrFilter(filterListResult);
                builder.WhereFilterGroup.AddFilter(innerFilter);
                builder.WhereFilterGroup.Optimize();
            }
            scope.Pop();
            return builder;
        }

        private void buildSetterList(MatchResult result, UpdateBuilder builder)
        {
            MatchResult multiple = result.Matches[SqlGrammar.SetterList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.SetterList.Multiple.First];
                Setter setter = buildSetter(first);
                builder.AddSetter(setter);
                MatchResult remaining = multiple.Matches[SqlGrammar.SetterList.Multiple.Remaining];
                buildSetterList(remaining, builder);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.SetterList.Single];
            if (single.IsMatch)
            {
                Setter setter = buildSetter(single);
                builder.AddSetter(setter);
                return;
            }
            throw new InvalidOperationException();
        }

        private Setter buildSetter(MatchResult result)
        {
            MatchResult columnResult = result.Matches[SqlGrammar.Setter.Column];
            Column column = buildColumn(columnResult);
            MatchResult valueResult = result.Matches[SqlGrammar.Setter.Value];
            IProjectionItem value = (IProjectionItem)buildArithmeticItem(valueResult);
            Setter setter = new Setter(column, value);
            return setter;
        }

        private ICommand buildDeleteStatement(MatchResult result)
        {
            MatchResult tableResult = result.Matches[SqlGrammar.DeleteStatement.Table];
            Table table = buildTable(tableResult);
            string alias = null;
            MatchResult aliasExpressionResult = result.Matches[SqlGrammar.DeleteStatement.AliasExpression.Name];
            if (aliasExpressionResult.IsMatch)
            {
                MatchResult aliasResult = aliasExpressionResult.Matches[SqlGrammar.DeleteStatement.AliasExpression.Alias];
                alias = getToken(aliasResult);
            }
            DeleteBuilder builder = new DeleteBuilder(table, alias);
            SourceCollection collection = new SourceCollection();
            collection.AddSource(builder.Table.GetSourceName(), builder.Table);
            scope.Push(collection);
            MatchResult whereResult = result.Matches[SqlGrammar.DeleteStatement.Where.Name];
            if (whereResult.IsMatch)
            {
                MatchResult filterListResult = whereResult.Matches[SqlGrammar.DeleteStatement.Where.FilterList];
                IFilter innerFilter = buildOrFilter(filterListResult);
                builder.WhereFilterGroup.AddFilter(innerFilter);
                builder.WhereFilterGroup.Optimize();
            }
            scope.Pop();
            return builder;
        }

        private void buildMultipartIdentifier(MatchResult result, List<string> parts)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultipartIdentifier.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.First];
                parts.Add(getToken(first));
                MatchResult remaining = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Remaining];
                buildMultipartIdentifier(remaining, parts);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.MultipartIdentifier.Single];
            if (single.IsMatch)
            {
                parts.Add(getToken(single));
                return;
            }
            throw new InvalidOperationException();
        }

        private object buildArithmeticItem(MatchResult result)
        {
            MatchResult expression = result.Matches[SqlGrammar.ArithmeticItem.ArithmeticExpression];
            return buildAdditiveExpression(expression, false);
        }

        private object buildAdditiveExpression(MatchResult result, bool wrap)
        {
            MatchResult multiple = result.Matches[SqlGrammar.AdditiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult firstResult = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.First];
                IProjectionItem first = (IProjectionItem)buildMultiplicitiveExpression(firstResult, false);
                MatchResult remainingResult = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Remaining];
                IProjectionItem remaining = (IProjectionItem)buildAdditiveExpression(remainingResult, false);
                MatchResult operatorResult = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Operator];
                return buildAdditiveOperator(operatorResult, first, remaining, wrap);
            }
            MatchResult single = result.Matches[SqlGrammar.AdditiveExpression.Single];
            if (single.IsMatch)
            {
                return buildMultiplicitiveExpression(single, wrap);
            }
            throw new InvalidOperationException();
        }

        private object buildAdditiveOperator(MatchResult result, IProjectionItem leftHand, IProjectionItem rightHand, bool wrap)
        {
            MatchResult plusResult = result.Matches[SqlGrammar.AdditiveOperator.PlusOperator];
            if (plusResult.IsMatch)
            {
                Addition addition = new Addition(leftHand, rightHand);
                addition.WrapInParentheses = wrap;
                return addition;
            }
            MatchResult minusResult = result.Matches[SqlGrammar.AdditiveOperator.MinusOperator];
            if (minusResult.IsMatch)
            {
                Subtraction subtraction = new Subtraction(leftHand, rightHand);
                subtraction.WrapInParentheses = wrap;
                return subtraction;
            }
            throw new InvalidOperationException();
        }

        private object buildMultiplicitiveExpression(MatchResult result, bool wrap)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult firstResult = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.First];
                IProjectionItem first = (IProjectionItem)buildWrappedItem(firstResult);
                MatchResult remainingResult = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Remaining];
                IProjectionItem remaining = (IProjectionItem)buildMultiplicitiveExpression(remainingResult, false);
                MatchResult operatorResult = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Operator];
                return buildMultiplicitiveOperator(operatorResult, first, remaining, wrap);
            }
            MatchResult single = result.Matches[SqlGrammar.MultiplicitiveExpression.Single];
            if (single.IsMatch)
            {
                return buildWrappedItem(single);
            }
            throw new InvalidOperationException();
        }

        private object buildMultiplicitiveOperator(MatchResult result, IProjectionItem leftHand, IProjectionItem rightHand, bool wrap)
        {
            MatchResult multiply = result.Matches[SqlGrammar.MultiplicitiveOperator.Multiply];
            if (multiply.IsMatch)
            {
                Multiplication multiplication = new Multiplication(leftHand, rightHand);
                multiplication.WrapInParentheses = wrap;
                return multiplication;
            }
            MatchResult divide = result.Matches[SqlGrammar.MultiplicitiveOperator.Divide];
            if (divide.IsMatch)
            {
                Division division = new Division(leftHand, rightHand);
                division.WrapInParentheses = wrap;
                return division;
            }
            MatchResult modulo = result.Matches[SqlGrammar.MultiplicitiveOperator.Modulus];
            if (modulo.IsMatch)
            {
                Modulus modulus = new Modulus(leftHand, rightHand);
                modulus.WrapInParentheses = wrap;
                return modulus;
            }
            throw new InvalidOperationException();
        }

        private object buildWrappedItem(MatchResult result)
        {
            MatchResult negatedResult = result.Matches[SqlGrammar.WrappedItem.Negated.Name];
            if (negatedResult.IsMatch)
            {
                MatchResult expressionResult = negatedResult.Matches[SqlGrammar.WrappedItem.Negated.Item];
                IProjectionItem item = (IProjectionItem)buildWrappedItem(expressionResult);
                return new Negation(item);
            }
            MatchResult wrappedResult = result.Matches[SqlGrammar.WrappedItem.Wrapped.Name];
            if (wrappedResult.IsMatch)
            {
                MatchResult expressionResult = wrappedResult.Matches[SqlGrammar.WrappedItem.Wrapped.AdditiveExpression];
                object expression = buildAdditiveExpression(expressionResult, true);
                return expression;
            }
            MatchResult itemResult = result.Matches[SqlGrammar.WrappedItem.Item];
            if (itemResult.IsMatch)
            {
                return buildItem(itemResult);
            }
            throw new InvalidOperationException();
        }

        private object buildItem(MatchResult result)
        {
            MatchResult numberResult = result.Matches[SqlGrammar.Item.Number];
            if (numberResult.IsMatch)
            {
                return buildNumericLiteral(numberResult);
            }
            MatchResult stringResult = result.Matches[SqlGrammar.Item.String];
            if (stringResult.IsMatch)
            {
                return buildStringLiteral(stringResult);
            }
            MatchResult nullResult = result.Matches[SqlGrammar.Item.Null];
            if (nullResult.IsMatch)
            {
                return new NullLiteral();
            }
            MatchResult functionCallResult = result.Matches[SqlGrammar.Item.FunctionCall];
            if (functionCallResult.IsMatch)
            {
                return buildFunctionCall(functionCallResult);
            }
            MatchResult columnResult = result.Matches[SqlGrammar.Item.Column];
            if (columnResult.IsMatch)
            {
                return buildNamedItem(columnResult);
            }
            MatchResult matchCaseResult = result.Matches[SqlGrammar.Item.MatchCase];
            if (matchCaseResult.IsMatch)
            {
                return buildMatchCase(matchCaseResult);
            }
            MatchResult conditionCaseResult = result.Matches[SqlGrammar.Item.ConditionCase];
            if (conditionCaseResult.IsMatch)
            {
                return buildConditionalCase(conditionCaseResult);
            }
            MatchResult selectResult = result.Matches[SqlGrammar.Item.Select.Name];
            if (selectResult.IsMatch)
            {
                MatchResult selectExpressionResult = selectResult.Matches[SqlGrammar.Item.Select.SelectStatement];
                return buildSelectStatement(selectExpressionResult);
            }
            throw new NotImplementedException();
        }

        private NumericLiteral buildNumericLiteral(MatchResult result)
        {
            string numberString = getToken(result);
            double value = Double.Parse(numberString);
            return new NumericLiteral(value);
        }

        private object buildNamedItem(MatchResult result)
        {
            List<string> parts = new List<string>();
            buildMultipartIdentifier(result, parts);
            if (parts.Count > 1)
            {
                // if is a period-separated, multiple-part identifier, it is a column
                Namespace qualifier = getNamespace(parts.Take(parts.Count - 2));
                string tableName = parts[parts.Count - 2];
                AliasedSource source = scope.GetSource(tableName);
                string columnName = parts[parts.Count - 1];
                return source.Column(columnName);
            }
            string name = parts[0];
            if (options.PlaceholderPrefix != null && name.StartsWith(options.PlaceholderPrefix))
            {
                // if the identifier begins with the placeholder prefix, treat is as a placeholder
                Placeholder placeholder = new Placeholder(name);
                return placeholder;
            }
            AliasedSource singleSource;
            if (scope.HasSingleSource(out singleSource))
            {
                // there is only one source in the query, so assume a column
                Column column = singleSource.Column(name);
                column.Qualify = false;
                return column;
            }
            // otherwise, we have no idea what the name represents
            // in order for the SQL to roundtrip without alteration, just use a placeholder
            Placeholder unknown = new Placeholder(name);
            return unknown;
        }

        private Column buildColumn(MatchResult result)
        {
            List<string> parts = new List<string>();
            buildMultipartIdentifier(result, parts);
            if (parts.Count > 1)
            {
                Namespace qualifier = getNamespace(parts.Take(parts.Count - 2));
                string tableName = parts[parts.Count - 2];
                AliasedSource source = scope.GetSource(tableName);
                string columnName = parts[parts.Count - 1];
                return source.Column(columnName);
            }
            else
            {
                string columnName = parts[0];
                Column column;
                AliasedSource source;
                if (scope.HasSingleSource(out source))
                {
                    column = source.Column(columnName);
                    column.Qualify = false;
                }
                else
                {
                    column = new Column(columnName);
                }
                return column;
            }
        }

        private StringLiteral buildStringLiteral(MatchResult result)
        {
            string value = getToken(result);
            value = value.Substring(1, value.Length - 2);
            value = value.Replace("''", "'");
            return new StringLiteral(value);
        }

        private Function buildFunctionCall(MatchResult result)
        {
            MatchResult functionNameResult = result.Matches[SqlGrammar.FunctionCall.FunctionName];
            List<string> parts = new List<string>();
            buildMultipartIdentifier(functionNameResult, parts);
            Namespace qualifier = getNamespace(parts.Take(parts.Count - 1));
            string functionName = parts[parts.Count - 1];
            Function function = new Function(qualifier, functionName);
            MatchResult argumentsResult = result.Matches[SqlGrammar.FunctionCall.Arguments];
            if (argumentsResult.IsMatch)
            {
                ValueList arguments = new ValueList();
                buildValueList(argumentsResult, arguments);
                foreach (IProjectionItem value in arguments.Values)
                {
                    function.AddArgument(value);
                }
            }
            MatchResult windowResult = result.Matches[SqlGrammar.FunctionCall.Window.Name];
            if (windowResult.IsMatch)
            {
                FunctionWindow window = new FunctionWindow();
                MatchResult partitioning = windowResult.Matches[SqlGrammar.FunctionCall.Window.Partitioning.Name];
                if (partitioning.IsMatch)
                {
                    MatchResult valueListResult = partitioning.Matches[SqlGrammar.FunctionCall.Window.Partitioning.ValueList];
                    ValueList valueList = new ValueList();
                    buildValueList(valueListResult, valueList);
                    foreach (IProjectionItem value in valueList.Values)
                    {
                        window.AddPartition(value);
                    }
                }
                MatchResult ordering = windowResult.Matches[SqlGrammar.FunctionCall.Window.Ordering.Name];
                if (ordering.IsMatch)
                {
                    MatchResult orderByListResult = ordering.Matches[SqlGrammar.FunctionCall.Window.Ordering.OrderByList];
                    buildOrderByList(orderByListResult, window.OrderByList);
                }
                MatchResult framing = windowResult.Matches[SqlGrammar.FunctionCall.Window.Framing.Name];
                if (framing.IsMatch)
                {
                    MatchResult precedingOnlyFrameResult = framing.Matches[SqlGrammar.FunctionCall.Window.Framing.PrecedingFrame];
                    if (precedingOnlyFrameResult.IsMatch)
                    {
                        IPrecedingFrame precedingFrame = buildPrecedingFrame(precedingOnlyFrameResult);
                        window.Frame = new PrecedingOnlyWindowFrame(precedingFrame);
                    }
                    MatchResult betweenFrameResult = framing.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.Name];
                    if (betweenFrameResult.IsMatch)
                    {
                        MatchResult precedingFrameResult = betweenFrameResult.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.PrecedingFrame];
                        IPrecedingFrame precedingFrame = buildPrecedingFrame(precedingFrameResult);
                        MatchResult followingFrameResult = betweenFrameResult.Matches[SqlGrammar.FunctionCall.Window.Framing.BetweenFrame.FollowingFrame];
                        IFollowingFrame followingFrame = buildFollowingFrame(followingFrameResult);
                        window.Frame = new BetweenWindowFrame(precedingFrame, followingFrame);
                    }
                    MatchResult frameTypeResult = framing.Matches[SqlGrammar.FunctionCall.Window.Framing.FrameType];
                    window.Frame.FrameType = buildFrameType(frameTypeResult);
                }
                function.FunctionWindow = window;
            }
            return function;
        }

        private FrameType buildFrameType(MatchResult result)
        {
            MatchResult rows = result.Matches[SqlGrammar.FrameType.Rows];
            if (rows.IsMatch)
            {
                return FrameType.Row;
            }
            MatchResult range = result.Matches[SqlGrammar.FrameType.Range];
            if (range.IsMatch)
            {
                return FrameType.Range;
            }
            throw new InvalidOperationException();
        }

        private IPrecedingFrame buildPrecedingFrame(MatchResult result)
        {
            MatchResult unbound = result.Matches[SqlGrammar.PrecedingFrame.UnboundedPreceding.Name];
            if (unbound.IsMatch)
            {
                return new PrecedingUnboundFrame();
            }
            MatchResult bound = result.Matches[SqlGrammar.PrecedingFrame.BoundedPreceding.Name];
            if (bound.IsMatch)
            {
                MatchResult rowCountResult = bound.Matches[SqlGrammar.PrecedingFrame.BoundedPreceding.Number];
                NumericLiteral rowCount = buildNumericLiteral(rowCountResult);
                return new PrecedingBoundFrame((int)rowCount.Value);
            }
            MatchResult currentRow = result.Matches[SqlGrammar.PrecedingFrame.CurrentRow];
            if (currentRow.IsMatch)
            {
                return new CurrentRowFrame();
            }
            throw new InvalidOperationException();
        }

        private IFollowingFrame buildFollowingFrame(MatchResult result)
        {
            MatchResult unbound = result.Matches[SqlGrammar.FollowingFrame.UnboundedFollowing.Name];
            if (unbound.IsMatch)
            {
                return new FollowingUnboundFrame();
            }
            MatchResult bound = result.Matches[SqlGrammar.FollowingFrame.BoundedFollowing.Name];
            if (bound.IsMatch)
            {
                MatchResult rowCountResult = bound.Matches[SqlGrammar.FollowingFrame.BoundedFollowing.Number];
                NumericLiteral rowCount = buildNumericLiteral(rowCountResult);
                return new FollowingBoundFrame((int)rowCount.Value);
            }
            MatchResult currentRow = result.Matches[SqlGrammar.FollowingFrame.CurrentRow];
            if (currentRow.IsMatch)
            {
                return new CurrentRowFrame();
            }
            throw new InvalidOperationException();
        }

        private MatchCase buildMatchCase(MatchResult result)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.MatchCase.Expression];
            IProjectionItem expression = (IProjectionItem)buildArithmeticItem(expressionResult);
            MatchCase options = new MatchCase(expression);
            MatchResult matchListResult = result.Matches[SqlGrammar.MatchCase.MatchList];
            buildMatchList(matchListResult, options);
            return options;
        }

        private void buildMatchList(MatchResult result, MatchCase options)
        {
            MatchResult match = result.Matches[SqlGrammar.MatchList.Match];
            buildMatch(match, options);
            MatchResult matchListPrime = result.Matches[SqlGrammar.MatchList.MatchListPrime];
            buildMatchListPrime(matchListPrime, options);
        }

        private void buildMatchListPrime(MatchResult result, MatchCase options)
        {
            MatchResult matchResult = result.Matches[SqlGrammar.MatchListPrime.Match.Name];
            if (matchResult.IsMatch)
            {
                MatchResult firstResult = matchResult.Matches[SqlGrammar.MatchListPrime.Match.First];
                buildMatch(firstResult, options);
                MatchResult remainingResult = matchResult.Matches[SqlGrammar.MatchListPrime.Match.Remaining];
                buildMatchListPrime(remainingResult, options);
                return;
            }
            MatchResult elseResult = result.Matches[SqlGrammar.MatchListPrime.Else.Name];
            if (elseResult.IsMatch)
            {
                MatchResult valueResult = elseResult.Matches[SqlGrammar.MatchListPrime.Else.Value];
                options.Default = (IProjectionItem)buildArithmeticItem(valueResult);
                return;
            }
            MatchResult emptyResult = result.Matches[SqlGrammar.MatchListPrime.Empty];
            if (emptyResult.IsMatch)
            {
                return;
            }
            throw new InvalidOperationException();
        }

        private void buildMatch(MatchResult result, MatchCase options)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.Match.Expression];
            IProjectionItem expression = (IProjectionItem)buildArithmeticItem(expressionResult);
            MatchResult valueResult = result.Matches[SqlGrammar.Match.Value];
            IProjectionItem value = (IProjectionItem)buildArithmeticItem(valueResult);
            options.AddBranch(expression, value);
        }

        private ConditionalCase buildConditionalCase(MatchResult result)
        {
            ConditionalCase options = new ConditionalCase();
            MatchResult conditionListResult = result.Matches[SqlGrammar.ConditionalCase.ConditionList];
            buildConditionList(conditionListResult, options);
            return options;
        }

        private void buildConditionList(MatchResult result, ConditionalCase options)
        {
            MatchResult condition = result.Matches[SqlGrammar.ConditionList.Condition];
            buildCondition(condition, options);
            MatchResult conditionListPrime = result.Matches[SqlGrammar.ConditionList.ConditionListPrime];
            buildConditionListPrime(conditionListPrime, options);
        }

        private void buildConditionListPrime(MatchResult result, ConditionalCase options)
        {
            MatchResult conditionResult = result.Matches[SqlGrammar.ConditionListPrime.Condition.Name];
            if (conditionResult.IsMatch)
            {
                MatchResult firstResult = conditionResult.Matches[SqlGrammar.ConditionListPrime.Condition.First];
                buildCondition(firstResult, options);
                MatchResult remainingResult = conditionResult.Matches[SqlGrammar.ConditionListPrime.Condition.Remaining];
                buildConditionListPrime(remainingResult, options);
                return;
            }
            MatchResult elseResult = result.Matches[SqlGrammar.ConditionListPrime.Else.Name];
            if (elseResult.IsMatch)
            {
                MatchResult valueResult = elseResult.Matches[SqlGrammar.ConditionListPrime.Else.Value];
                options.Default = (IProjectionItem)buildArithmeticItem(valueResult);
                return;
            }
            MatchResult emptyResult = result.Matches[SqlGrammar.ConditionListPrime.Empty];
            if (emptyResult.IsMatch)
            {
                return;
            }
            throw new InvalidOperationException();
        }

        private void buildCondition(MatchResult result, ConditionalCase options)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.Condition.Filter];
            IFilter innerFilter = buildOrFilter(expressionResult);
            MatchResult valueResult = result.Matches[SqlGrammar.Condition.Value];
            IProjectionItem value = (IProjectionItem)buildArithmeticItem(valueResult);
            FilterGroup filterGroup = new FilterGroup(Conjunction.And, innerFilter);
            filterGroup.Optimize();
            options.AddBranch(filterGroup, value);
        }

        private void buildValueList(MatchResult result, ValueList values)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ValueList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = multiple.Matches[SqlGrammar.ValueList.Multiple.First];
                IProjectionItem value = (IProjectionItem)buildArithmeticItem(first);
                values.AddValue(value);
                MatchResult remaining = multiple.Matches[SqlGrammar.ValueList.Multiple.Remaining];
                buildValueList(remaining, values);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ValueList.Single];
            if (single.IsMatch)
            {
                IProjectionItem value = (IProjectionItem)buildArithmeticItem(single);
                values.AddValue(value);
                return;
            }
            throw new NotImplementedException();
        }

        private Table buildTable(MatchResult tableResult)
        {
            List<string> parts = new List<string>();
            buildMultipartIdentifier(tableResult, parts);
            Namespace qualifier = getNamespace(parts.Take(parts.Count - 1));
            string tableName = parts[parts.Count - 1];
            Table table = new Table(qualifier, tableName);
            return table;
        }

        private Namespace getNamespace(IEnumerable<string> qualifiers)
        {
            if (!qualifiers.Any())
            {
                return null;
            }
            Namespace schema = new Namespace();
            foreach (string qualifier in qualifiers)
            {
                schema.AddQualifier(qualifier);
            }
            return schema;
        }

        private string getToken(MatchResult result)
        {
            TokenResult tokenResult = (TokenResult)result.Context;
            return tokenResult.Value;
        }

        private sealed class SourceScope
        {
            private readonly List<SourceCollection> stack;

            public SourceScope()
            {
                stack = new List<SourceCollection>();

            }

            public void Push(SourceCollection collection)
            {
                stack.Add(collection);
            }

            public void Pop()
            {
                stack.RemoveAt(stack.Count - 1);
            }

            public AliasedSource GetSource(string sourceName)
            {
                int index = stack.Count;
                while (index != 0)
                {
                    --index;
                    SourceCollection collection = stack[index];
                    if (collection.Exists(sourceName))
                    {
                        return collection[sourceName];
                    }
                }
                string message = String.Format(Resources.UnknownSource, sourceName);
                throw new SQLGenerationException(message);
            }

            public bool HasSingleSource(out AliasedSource source)
            {
                if (stack.Count == 0)
                {
                    source = null;
                    return false;
                }
                SourceCollection collection = stack[stack.Count - 1];
                if (collection.Count != 1)
                {
                    source = null;
                    return false;
                }
                source = collection.Sources.Single();
                return true;
            }
        }
    }
}
