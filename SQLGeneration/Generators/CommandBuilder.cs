using System;
using System.Collections.Generic;
using System.Linq;
using SQLGeneration.Builders;
using SQLGeneration.Parsing;

namespace SQLGeneration.Generators
{
    /// <summary>
    /// Builds an ICommand from a SQL statement.
    /// </summary>
    public sealed class CommandBuilder : SqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of a SimpleFormatter.
        /// </summary>
        /// <param name="registry">The token registry to use.</param>
        public CommandBuilder(SqlTokenRegistry registry)
            : base(new SqlGrammar(registry))
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
        /// <returns>The command that was parsed.</returns>
        public ICommand GetCommand(string commandText)
        {
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
            return null;
        }

        private ICommand buildSelectStatement(MatchResult result)
        {
            MatchResult selectExpression = result.Matches[SqlGrammar.SelectStatement.SelectExpression];
            ISelectBuilder builder = buildSelectExpression(selectExpression);
            MatchResult orderBy = result.Matches[SqlGrammar.SelectStatement.OrderBy.Name];
            if (orderBy.IsMatch)
            {
                MatchResult orderByList = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByList];
                buildOrderByList(orderByList, builder);
            }
            return builder;
        }

        private ISelectBuilder buildSelectExpression(MatchResult result)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.SelectExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult expression = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.SelectExpression];
                return buildSelectExpression(result);
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
            MatchResult projectionList = result.Matches[SqlGrammar.SelectSpecification.ProjectionList];
            buildProjectionList(projectionList, builder);
            MatchResult where = result.Matches[SqlGrammar.SelectSpecification.Where.Name];
            if (where.IsMatch)
            {
                MatchResult filterList = where.Matches[SqlGrammar.SelectSpecification.Where.FilterList];
                buildFilterList(filterList, builder._where);
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
                buildFilterList(filterList, builder._having);
            }
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
            return DistinctQualifier.Default;
        }

        private Top buildTop(MatchResult result, SelectBuilder builder)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.SelectSpecification.Top.Expression];
            IProjectionItem expression = (IProjectionItem)buildArithmeticItem(expressionResult, builder.Sources);
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
                buildJoin(first, builder, false);
                MatchResult remaining = multiple.Matches[SqlGrammar.FromList.Multiple.Remaining];
                buildFromList(remaining, builder);
            }
            MatchResult single = result.Matches[SqlGrammar.FromList.Single];
            if (single.IsMatch)
            {
                buildJoin(single, builder, false);
            }
        }

        private void buildJoin(MatchResult result, SelectBuilder builder, bool wrap)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Join.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult join = wrapped.Matches[SqlGrammar.Join.Wrapped.Join];
                buildJoin(join, builder, true);
            }
            MatchResult joined = result.Matches[SqlGrammar.Join.Joined.Name];
            if (joined.IsMatch)
            {
                string alias;
                MatchResult joinItemResult = joined.Matches[SqlGrammar.Join.Joined.JoinItem];
                IRightJoinItem first = buildJoinItem(joinItemResult, out alias);
                MatchResult joinPrime = joined.Matches[SqlGrammar.Join.Joined.JoinPrime];
                IJoinItem joinItem = buildJoinPrime(joinPrime, first);
                Join join = joinItem as Join;
                if (wrap && join != null)
                {
                    join.WrapInParentheses = true;
                    builder.AddJoin(join);
                    return;
                }
                Table table = joinItem as Table;
                if (table != null)
                {
                    builder.AddTable(table, alias);
                    return;
                }
                ISelectBuilder select = joinItem as SelectBuilder;
                if (select != null)
                {
                    builder.AddSelect(select, alias);
                }
                Function functionCall = joinItem as Function;
                if (functionCall != null)
                {
                    builder.AddFunction(functionCall, alias);
                }
            }
        }

        private IRightJoinItem buildJoinItem(MatchResult result, out string alias)
        {
            alias = null;
            MatchResult aliasExpression = result.Matches[SqlGrammar.JoinItem.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult aliasResult = result.Matches[SqlGrammar.JoinItem.AliasExpression.Alias];
                alias = getToken(aliasResult);
            }
            MatchResult tableResult = result.Matches[SqlGrammar.JoinItem.Table];
            if (tableResult.IsMatch)
            {
                List<string> parts = new List<string>();
                buildMultipartIdentifier(tableResult, parts);
                Namespace qualifier = getNamespace(parts.Take(parts.Count - 1));
                string tableName = parts[parts.Count - 1];
                Table table = new Table(qualifier, tableName);
                return table;
            }
            MatchResult select = result.Matches[SqlGrammar.JoinItem.SelectExpression];
            if (select.IsMatch)
            {
            }
            MatchResult functionCall = result.Matches[SqlGrammar.JoinItem.FunctionCall];
            if (functionCall.IsMatch)
            {
            }
            throw new NotImplementedException();
        }

        private IJoinItem buildJoinPrime(MatchResult result, IRightJoinItem leftHand)
        {
            MatchResult filtered = result.Matches[SqlGrammar.JoinPrime.Filtered.Name];
            if (filtered.IsMatch)
            {
                
            }
            MatchResult cross = result.Matches[SqlGrammar.JoinPrime.Cross.Name];
            if (cross.IsMatch)
            {
            }
            return leftHand;
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
        }

        private void buildProjectionItem(MatchResult result, SelectBuilder builder)
        {
            MatchResult expression = result.Matches[SqlGrammar.ProjectionItem.Expression.Name];
            if (expression.IsMatch)
            {
                MatchResult itemResult = expression.Matches[SqlGrammar.ProjectionItem.Expression.Item];
                IProjectionItem item = (IProjectionItem)buildArithmeticItem(itemResult, builder.Sources);
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
                MatchResult qualifier = expression.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Name];
                if (qualifier.IsMatch)
                {
                    MatchResult columnSource = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.ColumnSource];
                    List<string> parts = new List<string>();
                    buildMultipartIdentifier(columnSource, parts);
                    string name = parts[parts.Count - 1];
                    source = builder.Sources[name];
                }
                AllColumns all = new AllColumns(source);
                builder.AddProjection(all);
                return;
            }
        }

        private void buildGroupByList(MatchResult result, SelectBuilder builder)
        {
            throw new NotImplementedException();
        }

        private void buildFilterList(MatchResult result, FilterGroup filterGroup)
        {
            throw new NotImplementedException();
        }

        private SelectCombiner buildSelectCombiner(MatchResult result, ISelectBuilder leftHand, ISelectBuilder rightHand)
        {
            MatchResult union = result.Matches[SqlGrammar.SelectCombiner.Union];
            if (union.IsMatch)
            {
                return new Union(leftHand, rightHand);
            }
            MatchResult intersect = result.Matches[SqlGrammar.SelectCombiner.Intersect];
            if (union.IsMatch)
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
            return null;
        }

        private void buildOrderByList(MatchResult result, ISelectBuilder builder)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrderByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult first = result.Matches[SqlGrammar.OrderByList.Multiple.First];
                buildOrderByItem(first, builder);
                MatchResult remaining = result.Matches[SqlGrammar.OrderByList.Multiple.Remaining];
                buildOrderByList(remaining, builder);
            }
            MatchResult single = result.Matches[SqlGrammar.OrderByList.Single];
            if (single.IsMatch)
            {
                buildOrderByItem(single, builder);
            }
        }

        private void buildOrderByItem(MatchResult result, ISelectBuilder builder)
        {
            MatchResult expressionResult = result.Matches[SqlGrammar.OrderByItem.Expression];
            IProjectionItem expression = (IProjectionItem)buildArithmeticItem(expressionResult, null);
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
            builder.AddOrderBy(orderBy);
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
            return Order.Default;
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
            return NullPlacement.Default;
        }

        private ICommand buildInsertStatement(MatchResult result)
        {
            throw new NotImplementedException();
        }

        private ICommand buildUpdateStatement(MatchResult result)
        {
            throw new NotImplementedException();
        }

        private ICommand buildDeleteStatement(MatchResult result)
        {
            throw new NotImplementedException();
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
            }
            MatchResult single = result.Matches[SqlGrammar.MultipartIdentifier.Single];
            if (single.IsMatch)
            {
                parts.Add(getToken(single));
            }
        }

        private object buildArithmeticItem(MatchResult result, SourceCollection sources)
        {
            MatchResult expression = result.Matches[SqlGrammar.ArithmeticItem.ArithmeticExpression];
            return buildAdditiveExpression(expression, sources);
        }

        private object buildAdditiveExpression(MatchResult result, SourceCollection sources)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.AdditiveExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult expressionResult = wrapped.Matches[SqlGrammar.AdditiveExpression.Wrapped.Expression];
                object expression = buildAdditiveExpression(expressionResult, sources);
                ArithmeticExpression arithmetic = expression as ArithmeticExpression;
                if (arithmetic != null)
                {
                    arithmetic.WrapInParentheses = true;
                }
                return expression;
            }
            MatchResult multiple = result.Matches[SqlGrammar.AdditiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult firstResult = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.First];
                IProjectionItem first = (IProjectionItem)buildMultiplicitiveExpression(firstResult, sources);
                MatchResult remainingResult = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Remaining];
                IProjectionItem remaining = (IProjectionItem)buildAdditiveExpression(remainingResult, sources);
                MatchResult operatorResult = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Operator];
                return buildAdditiveOperator(operatorResult, first, remaining);
            }
            MatchResult single = result.Matches[SqlGrammar.AdditiveExpression.Single];
            if (single.IsMatch)
            {
                return buildMultiplicitiveExpression(single, sources);
            }
            return null;
        }

        private object buildAdditiveOperator(MatchResult result, IProjectionItem leftHand, IProjectionItem rightHand)
        {
            MatchResult plusResult = result.Matches[SqlGrammar.AdditiveOperator.PlusOperator];
            if (plusResult.IsMatch)
            {
                return new Addition(leftHand, rightHand);
            }
            MatchResult minusResult = result.Matches[SqlGrammar.AdditiveOperator.MinusOperator];
            if (minusResult.IsMatch)
            {
                return new Subtraction(leftHand, rightHand);
            }
            return null;
        }

        private object buildMultiplicitiveExpression(MatchResult result, SourceCollection sources)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                MatchResult firstResult = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.First];
                IProjectionItem first = (IProjectionItem)buildItem(firstResult, sources);
                MatchResult remainingResult = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Remaining];
                IProjectionItem remaining = (IProjectionItem)buildMultiplicitiveExpression(remainingResult, sources);
                MatchResult operatorResult = multiple.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Operator];
                return buildMultiplicitiveOperator(operatorResult, first, remaining);
            }
            MatchResult single = result.Matches[SqlGrammar.MultiplicitiveExpression.Single];
            if (single.IsMatch)
            {
                return buildItem(single, sources);
            }
            return null;
        }

        private object buildMultiplicitiveOperator(MatchResult result, IProjectionItem leftHand, IProjectionItem rightHand)
        {
            MatchResult multiply = result.Matches[SqlGrammar.MultiplicitiveOperator.Multiply];
            if (multiply.IsMatch)
            {
                return new Multiplication(leftHand, rightHand);
            }
            MatchResult divide = result.Matches[SqlGrammar.MultiplicitiveOperator.Divide];
            if (divide.IsMatch)
            {
                return new Division(leftHand, rightHand);
            }
            return null;
        }

        private object buildItem(MatchResult result, SourceCollection sources)
        {
            MatchResult numberResult = result.Matches[SqlGrammar.Item.Number];
            if (numberResult.IsMatch)
            {
                string numberString = getToken(numberResult);
                double value = Double.Parse(numberString);
                return new NumericLiteral((decimal)value);
            }
            MatchResult stringResult = result.Matches[SqlGrammar.Item.String];
            if (stringResult.IsMatch)
            {
                string value = getToken(stringResult);
                value = value.Substring(1, value.Length - 1);
                value = value.Replace("''", "'");
                return new StringLiteral(value);
            }
            throw new NotImplementedException();
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
    }
}
