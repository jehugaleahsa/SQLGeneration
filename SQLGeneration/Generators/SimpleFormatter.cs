using System;
using System.Collections.Generic;
using System.Text;
using SQLGeneration.Parsing;
using System.IO;

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
        /// <param name="registry">The token registry to use.</param>
        public SimpleFormatter(SqlTokenRegistry registry)
            : base(new SqlGrammar(registry))
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
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                GetResult(tokenStream, writer);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Generates output for the top-level statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnStart(MatchResult result, object context)
        {
            MatchResult selectResult = result.Matches[SqlGrammar.Start.SelectStatement];
            if (selectResult.IsMatch)
            {
                selectResult.GetContext(context);
                return;
            }
            MatchResult insertResult = result.Matches[SqlGrammar.Start.InsertStatement];
            if (insertResult.IsMatch)
            {
                insertResult.GetContext(context);
                return;
            }
            MatchResult updateResult = result.Matches[SqlGrammar.Start.UpdateStatement];
            if (updateResult.IsMatch)
            {
                updateResult.GetContext(context);
                return;
            }
            MatchResult deleteResult = result.Matches[SqlGrammar.Start.DeleteStatement];
            if (deleteResult.IsMatch)
            {
                deleteResult.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for the SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnSelectStatement(MatchResult result, object context)
        {
            MatchResult expression = result.Matches[SqlGrammar.SelectStatement.SelectExpression];
            expression.GetContext(context);
            MatchResult orderBy = result.Matches[SqlGrammar.SelectStatement.OrderBy.Name];
            if (orderBy.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                writer.Write(' ');
                MatchResult orderByKeyword = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByKeyword];
                orderByKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult orderByList = orderBy.Matches[SqlGrammar.SelectStatement.OrderBy.OrderByList];
                orderByList.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for the SELECT expression.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnSelectExpression(MatchResult result, object context)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.SelectExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.LeftParenthesis];
                leftParenthesis.GetContext(context);
                MatchResult selectExpression = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.SelectExpression];
                selectExpression.GetContext(context);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.SelectExpression.Wrapped.RightParenthesis];
                rightParenthesis.GetContext(context);
                return;
            }
            MatchResult specification = result.Matches[SqlGrammar.SelectExpression.SelectSpecification];
            if (specification.IsMatch)
            {
                specification.GetContext(context);
                return;
            }
            MatchResult remaining = specification.Matches[SqlGrammar.SelectExpression.Remaining.Name];
            if (remaining.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                writer.Write(' ');
                MatchResult combiner = result.Matches[SqlGrammar.SelectExpression.Remaining.Combiner];
                combiner.GetContext(writer);
                writer.Write(' ');
                MatchResult selectExpression = result.Matches[SqlGrammar.SelectExpression.Remaining.SelectExpression];
                selectExpression.GetContext(writer);
                return;
            }
        }

        /// <summary>
        /// Generates output for the SELECT specification.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnSelectSpecification(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult select = result.Matches[SqlGrammar.SelectSpecification.SelectKeyword];
            select.GetContext(writer);
            writer.Write(' ');
            MatchResult distinctQualifier = result.Matches[SqlGrammar.SelectSpecification.DistinctQualifier];
            if (distinctQualifier.IsMatch)
            {
                distinctQualifier.GetContext(writer);
                writer.Write(' ');
            }
            MatchResult top = result.Matches[SqlGrammar.SelectSpecification.Top.Name];
            if (top.IsMatch)
            {
                MatchResult topKeyword = top.Matches[SqlGrammar.SelectSpecification.Top.TopKeyword];
                topKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult expression = top.Matches[SqlGrammar.SelectSpecification.Top.Expression];
                expression.GetContext(writer);
                writer.Write(' ');
                MatchResult percent = top.Matches[SqlGrammar.SelectSpecification.Top.PercentKeyword];
                if (percent.IsMatch)
                {
                    percent.GetContext(writer);
                    writer.Write(' ');
                }
                MatchResult tiesWith = top.Matches[SqlGrammar.SelectSpecification.Top.WithTiesKeyword];
                if (tiesWith.IsMatch)
                {
                    tiesWith.GetContext(writer);
                    writer.Write(' ');
                }
            }
            MatchResult projectionList = result.Matches[SqlGrammar.SelectSpecification.ProjectionList];
            projectionList.GetContext(writer);
            MatchResult from = result.Matches[SqlGrammar.SelectSpecification.From.Name];
            if (from.IsMatch)
            {
                writer.Write(' ');
                MatchResult fromKeyword = from.Matches[SqlGrammar.SelectSpecification.From.FromKeyword];
                fromKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult fromList = from.Matches[SqlGrammar.SelectSpecification.From.FromList];
                fromList.GetContext(writer);
            }
            MatchResult where = result.Matches[SqlGrammar.SelectSpecification.Where.Name];
            if (where.IsMatch)
            {
                writer.Write(' ');
                MatchResult whereKeyword = where.Matches[SqlGrammar.SelectSpecification.Where.WhereKeyword];
                whereKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult filterList = where.Matches[SqlGrammar.SelectSpecification.Where.FilterList];
                filterList.GetContext(writer);
            }
            MatchResult groupBy = result.Matches[SqlGrammar.SelectSpecification.GroupBy.Name];
            if (groupBy.IsMatch)
            {
                writer.Write(' ');
                MatchResult groupByKeyword = groupBy.Matches[SqlGrammar.SelectSpecification.GroupBy.GroupByKeyword];
                groupByKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult groupByList = groupBy.Matches[SqlGrammar.SelectSpecification.GroupBy.GroupByList];
                groupByList.GetContext(writer);
            }
            MatchResult having = result.Matches[SqlGrammar.SelectSpecification.Having.Name];
            if (having.IsMatch)
            {
                writer.Write(' ');
                MatchResult havingKeyword = having.Matches[SqlGrammar.SelectSpecification.Having.HavingKeyword];
                havingKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult filterList = having.Matches[SqlGrammar.SelectSpecification.Having.FilterList];
                filterList.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for the ORDER BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnOrderByList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrderByList.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.OrderByList.Multiple.First];
                first.GetContext(writer);
                MatchResult comma = multiple.Matches[SqlGrammar.OrderByList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.OrderByList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.OrderByList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for an ORDER BY item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnOrderByItem(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult expression = result.Matches[SqlGrammar.OrderByItem.Expression];
            expression.GetContext(writer);
            MatchResult direction = result.Matches[SqlGrammar.OrderByItem.OrderDirection];
            if (direction.IsMatch)
            {
                writer.Write(' ');
                direction.GetContext(writer);
            }
            MatchResult nullPlacement = result.Matches[SqlGrammar.OrderByItem.NullPlacement];
            if (nullPlacement.IsMatch)
            {
                writer.Write(' ');
                nullPlacement.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for an arithmetic expression adding or subtracting values.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnAdditiveEpression(MatchResult result, object context)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.AdditiveExpression.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.AdditiveExpression.Wrapped.LeftParenthesis];
                leftParenthesis.GetContext(context);
                MatchResult expression = wrapped.Matches[SqlGrammar.AdditiveExpression.Wrapped.Expression];
                expression.GetContext(context);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.AdditiveExpression.Wrapped.RightParenthesis];
                rightParenthesis.GetContext(context);
                return;
            }
            MatchResult multiple = result.Matches[SqlGrammar.AdditiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.First];
                first.GetContext(writer);
                writer.Write(' ');
                MatchResult additiveOperator = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Operator];
                additiveOperator.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.AdditiveExpression.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.AdditiveExpression.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for the plus or minus operator.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnAdditiveOperator(MatchResult result, object context)
        {
            MatchResult plusOperator = result.Matches[SqlGrammar.AdditiveOperator.PlusOperator];
            if (plusOperator.IsMatch)
            {
                plusOperator.GetContext(context);
                return;
            }
            MatchResult minusOperator = result.Matches[SqlGrammar.AdditiveOperator.MinusOperator];
            if (minusOperator.IsMatch)
            {
                minusOperator.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for an arithmetic expression multiplying or dividing values.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnMultiplicitiveExpression(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.First];
                first.GetContext(writer);
                writer.Write(' ');
                MatchResult multiplicitiveOperator = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Operator];
                multiplicitiveOperator.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = result.Matches[SqlGrammar.MultiplicitiveExpression.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.MultiplicitiveExpression.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a multiply or divide operator.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnMultiplicitiveOperator(MatchResult result, object context)
        {
            MatchResult multiplyOperator = result.Matches[SqlGrammar.MultiplicitiveOperator.Multiply];
            if (multiplyOperator.IsMatch)
            {
                multiplyOperator.GetContext(context);
                return;
            }
            MatchResult divideOperator = result.Matches[SqlGrammar.MultiplicitiveOperator.Divide];
            if (divideOperator.IsMatch)
            {
                divideOperator.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for the projection list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnProjectionList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ProjectionList.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.ProjectionList.Multiple.First];
                first.GetContext(writer);
                MatchResult comma = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ProjectionList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ProjectionList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a projection item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnProjectionItem(MatchResult result, object context)
        {
            MatchResult star = result.Matches[SqlGrammar.ProjectionItem.Star.Name];
            if (star.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult qualifier = star.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Name];
                if (qualifier.IsMatch)
                {
                    MatchResult columnSource = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.ColumnSource];
                    columnSource.GetContext(writer);
                    MatchResult dot = qualifier.Matches[SqlGrammar.ProjectionItem.Star.Qualifier.Dot];
                    dot.GetContext(writer);
                }
                MatchResult starToken = star.Matches[SqlGrammar.ProjectionItem.Star.StarToken];
                starToken.GetContext(writer);
                return;
            }
            MatchResult projectionItem = result.Matches[SqlGrammar.ProjectionItem.Expression.Name];
            if (projectionItem.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult item = projectionItem.Matches[SqlGrammar.ProjectionItem.Expression.Item];
                item.GetContext(writer);
                MatchResult aliasExpression = projectionItem.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Name];
                if (aliasExpression.IsMatch)
                {
                    MatchResult aliasIndicator = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.AliasIndicator];
                    if (aliasIndicator.IsMatch)
                    {
                        writer.Write(' ');
                        aliasIndicator.GetContext(writer);
                    }
                    writer.Write(' ');
                    MatchResult alias = aliasExpression.Matches[SqlGrammar.ProjectionItem.Expression.AliasExpression.Alias];
                    alias.GetContext(writer);
                }
                return;
            }
        }

        /// <summary>
        /// Generates output for the FROM list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnFromList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.FromList.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.FromList.Multiple.Name];
                first.GetContext(writer);
                MatchResult comma = multiple.Matches[SqlGrammar.FromList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.FromList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.FromList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnJoinItem(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult table = result.Matches[SqlGrammar.JoinItem.Table];
            if (table.IsMatch)
            {
                table.GetContext(writer);
            }
            MatchResult functionCall = result.Matches[SqlGrammar.JoinItem.FunctionCall];
            if (functionCall.IsMatch)
            {
                functionCall.GetContext(writer);
            }
            MatchResult select = result.Matches[SqlGrammar.JoinItem.SelectExpression];
            if (select.IsMatch)
            {
                select.GetContext(writer);
            }
            MatchResult aliasExpression = result.Matches[SqlGrammar.JoinItem.AliasExpression.Name];
            if (aliasExpression.IsMatch)
            {
                MatchResult aliasIndicator = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.AliasIndicator];
                if (aliasIndicator.IsMatch)
                {
                    writer.Write(' ');
                    aliasIndicator.GetContext(writer);
                }
                writer.Write(' ');
                MatchResult alias = aliasExpression.Matches[SqlGrammar.JoinItem.AliasExpression.Alias];
                alias.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for a function call.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnFunctionCall(MatchResult result, object context)
        {
            MatchResult functionName = result.Matches[SqlGrammar.FunctionCall.FunctionName];
            functionName.GetContext(context);
            MatchResult leftParenthesis = result.Matches[SqlGrammar.FunctionCall.LeftParethesis];
            leftParenthesis.GetContext(context);
            MatchResult arguments = result.Matches[SqlGrammar.FunctionCall.Arguments];
            arguments.GetContext(context);
            MatchResult rightParenthesis = result.Matches[SqlGrammar.FunctionCall.RightParenthesis];
            rightParenthesis.GetContext(context);
        }

        /// <summary>
        /// Generates output for a join.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnJoin(MatchResult result, object context)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Join.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.Join.Wrapped.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult join = wrapped.Matches[SqlGrammar.Join.Wrapped.Join];
                join.GetContext(writer);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.Join.Wrapped.RightParenthesis];
                rightParenthesis.GetContext(writer);
                return;
            }
            MatchResult joined = result.Matches[SqlGrammar.Join.Joined.Name];
            if (joined.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult joinItem = joined.Matches[SqlGrammar.Join.Joined.JoinItem];
                joinItem.GetContext(writer);
                MatchResult joinPrime = joined.Matches[SqlGrammar.Join.Joined.JoinPrime];
                joinPrime.GetContext(writer);
                return;
            }
        }

        /// <summary>
        /// Generates output for the next join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnJoinPrime(MatchResult result, object context)
        {
            MatchResult filtered = result.Matches[SqlGrammar.JoinPrime.Filtered.Name];
            if (filtered.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                writer.Write(' ');
                MatchResult joinType = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinType];
                joinType.GetContext(writer);
                writer.Write(' ');
                MatchResult nextItem = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinItem];
                nextItem.GetContext(writer);
                MatchResult onClause = filtered.Matches[SqlGrammar.JoinPrime.Filtered.On.Name];
                if (onClause.IsMatch)
                {
                    writer.Write(' ');
                    MatchResult onKeyword = onClause.Matches[SqlGrammar.JoinPrime.Filtered.On.OnToken];
                    onKeyword.GetContext(writer);
                    writer.Write(' ');
                    MatchResult filterList = onClause.Matches[SqlGrammar.JoinPrime.Filtered.On.FilterList];
                    filterList.GetContext(writer);
                }
                MatchResult remaining = filtered.Matches[SqlGrammar.JoinPrime.Filtered.JoinPrime];
                remaining.GetContext(writer);
                return;
            }
            MatchResult cross = result.Matches[SqlGrammar.JoinPrime.Cross.Name];
            if (cross.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                writer.Write(' ');
                MatchResult joinType = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinType];
                joinType.GetContext(writer);
                writer.Write(' ');
                MatchResult nextItem = cross.Matches[SqlGrammar.JoinPrime.Cross.JoinItem];
                nextItem.GetContext(writer);
                return;
            }
        }

        /// <summary>
        /// Generates output for a filter list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnFilterList(MatchResult result, object context)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.FilterList.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult notKeyword = wrapped.Matches[SqlGrammar.FilterList.Wrapped.Not];
                if (notKeyword.IsMatch)
                {
                    notKeyword.GetContext(writer);
                    writer.Write(' ');
                }
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.FilterList.Wrapped.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult filterList = wrapped.Matches[SqlGrammar.FilterList.Wrapped.FilterList];
                filterList.GetContext(writer);
                MatchResult rightParethesis = wrapped.Matches[SqlGrammar.FilterList.Wrapped.RightParenthesis];
                rightParethesis.GetContext(writer);
                return;
            }
            MatchResult multiple = result.Matches[SqlGrammar.FilterList.Multiple.Name];
            if (multiple.IsMatch)
            {
                multiple.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a filter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnFilter(MatchResult result, object context)
        {
            MatchResult wrapped = result.Matches[SqlGrammar.Filter.Wrapped.Name];
            if (wrapped.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult leftParenthesis = wrapped.Matches[SqlGrammar.Filter.Wrapped.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult filter = wrapped.Matches[SqlGrammar.Filter.Wrapped.Filter];
                filter.GetContext(writer);
                MatchResult rightParenthesis = wrapped.Matches[SqlGrammar.Filter.Wrapped.RightParenthesis];
                rightParenthesis.GetContext(writer);
                return;
            }
            MatchResult not = result.Matches[SqlGrammar.Filter.Not.Name];
            if (not.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult notKeyword = not.Matches[SqlGrammar.Filter.Not.NotKeyword];
                notKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult filter = not.Matches[SqlGrammar.Filter.Not.Filter];
                filter.GetContext(writer);
                return;
            }
            MatchResult order = result.Matches[SqlGrammar.Filter.Order.Name];
            if (order.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult left = order.Matches[SqlGrammar.Filter.Order.Left];
                left.GetContext(writer);
                writer.Write(' ');
                MatchResult comparisonOperator = order.Matches[SqlGrammar.Filter.Order.ComparisonOperator];
                comparisonOperator.GetContext(writer);
                writer.Write(' ');
                MatchResult right = order.Matches[SqlGrammar.Filter.Order.Right];
                right.GetContext(writer);
                return;
            }
            MatchResult between = result.Matches[SqlGrammar.Filter.Between.Name];
            if (between.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult expression = between.Matches[SqlGrammar.Filter.Between.Expression];
                expression.GetContext(writer);
                writer.Write(' ');
                MatchResult notKeyword = between.Matches[SqlGrammar.Filter.Between.Not];
                if (notKeyword.IsMatch)
                {
                    notKeyword.GetContext(writer);
                    writer.Write(' ');
                }
                MatchResult betweenKeyword = between.Matches[SqlGrammar.Filter.Between.BetweenKeyword];
                betweenKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult lowerBound = between.Matches[SqlGrammar.Filter.Between.LowerBound];
                lowerBound.GetContext(writer);
                MatchResult andKeyword = between.Matches[SqlGrammar.Filter.Between.And];
                andKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult upperBound = between.Matches[SqlGrammar.Filter.Between.UpperBound];
                upperBound.GetContext(writer);
                return;
            }
            MatchResult like = result.Matches[SqlGrammar.Filter.Like.Name];
            if (like.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult expression = like.Matches[SqlGrammar.Filter.Like.Expression];
                expression.GetContext(writer);
                writer.Write(' ');
                MatchResult notKeyword = like.Matches[SqlGrammar.Filter.Like.Not];
                if (notKeyword.IsMatch)
                {
                    notKeyword.GetContext(writer);
                    writer.Write(' ');
                }
                MatchResult likeKeyword = like.Matches[SqlGrammar.Filter.Like.LikeKeyword];
                likeKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult value = like.Matches[SqlGrammar.Filter.Like.Value];
                value.GetContext(writer);
                return;
            }
            MatchResult isResult = result.Matches[SqlGrammar.Filter.Is.Name];
            if (isResult.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult expression = isResult.Matches[SqlGrammar.Filter.Is.Expression];
                expression.GetContext(writer);
                writer.Write(' ');
                MatchResult isKeyword = isResult.Matches[SqlGrammar.Filter.Is.IsKeyword];
                isKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult notKeyword = isResult.Matches[SqlGrammar.Filter.Is.Not];
                if (notKeyword.IsMatch)
                {
                    notKeyword.GetContext(writer);
                    writer.Write(' ');
                }
                MatchResult nullKeyword = isResult.Matches[SqlGrammar.Filter.Is.Null];
                nullKeyword.GetContext(writer);
                return;
            }
            MatchResult inResult = result.Matches[SqlGrammar.Filter.In.Name];
            if (inResult.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult expression = inResult.Matches[SqlGrammar.Filter.In.Name];
                expression.GetContext(writer);
                writer.Write(' ');
                MatchResult notKeyword = inResult.Matches[SqlGrammar.Filter.In.Not];
                if (notKeyword.IsMatch)
                {
                    notKeyword.GetContext(writer);
                    writer.Write(' ');
                }
                MatchResult inKeyword = inResult.Matches[SqlGrammar.Filter.In.InKeyword];
                inKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult leftParenthesis = inResult.Matches[SqlGrammar.Filter.In.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult selectExpression = inResult.Matches[SqlGrammar.Filter.In.SelectExpression];
                if (selectExpression.IsMatch)
                {
                    selectExpression.GetContext(writer);
                }
                MatchResult valueList = inResult.Matches[SqlGrammar.Filter.In.ValueList];
                if (valueList.IsMatch)
                {
                    valueList.GetContext(writer);
                }
                MatchResult rightParenthesis = inResult.Matches[SqlGrammar.Filter.In.RightParenthesis];
                rightParenthesis.GetContext(writer);
                return;
            }
        }

        /// <summary>
        /// Generates output for a value list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnValueList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ValueList.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.ValueList.Multiple.First];
                first.GetContext(writer);
                MatchResult comma = multiple.Matches[SqlGrammar.ValueList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ValueList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ValueList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a GROUP BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnGroupByList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.GroupByList.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult left = result.Matches[SqlGrammar.GroupByList.Multiple.First];
                left.GetContext(writer);
                MatchResult comma = result.Matches[SqlGrammar.GroupByList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = result.Matches[SqlGrammar.GroupByList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.GroupByList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a non-arithmetic item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnNonArithmeticItem(MatchResult result, object context)
        {
            MatchResult functionCall = result.Matches[SqlGrammar.NonArithmeticItem.FunctionCall];
            if (functionCall.IsMatch)
            {
                functionCall.GetContext(context);
                return;
            }
            MatchResult column = result.Matches[SqlGrammar.NonArithmeticItem.Column];
            if (column.IsMatch)
            {
                column.GetContext(context);
                return;
            }
            MatchResult select = result.Matches[SqlGrammar.NonArithmeticItem.SelectStatement];
            if (select.IsMatch)
            {
                select.GetContext(context);
                return;
            }
            MatchResult numberResult = result.Matches[SqlGrammar.NonArithmeticItem.Number];
            if (numberResult.IsMatch)
            {
                numberResult.GetContext(context);
                return;
            }
            MatchResult stringResult = result.Matches[SqlGrammar.NonArithmeticItem.String];
            if (stringResult.IsMatch)
            {
                stringResult.GetContext(context);
                return;
            }
            MatchResult nullResult = result.Matches[SqlGrammar.NonArithmeticItem.Null];
            if (nullResult.IsMatch)
            {
                nullResult.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for an item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnItem(MatchResult result, object context)
        {
            MatchResult functionCall = result.Matches[SqlGrammar.Item.FunctionCall];
            if (functionCall.IsMatch)
            {
                functionCall.GetContext(context);
                return;
            }
            MatchResult column = result.Matches[SqlGrammar.Item.Column];
            if (column.IsMatch)
            {
                column.GetContext(context);
                return;
            }
            MatchResult arithmetic = result.Matches[SqlGrammar.Item.ArithmeticExpression];
            if (arithmetic.IsMatch)
            {
                arithmetic.GetContext(context);
                return;
            }
            MatchResult select = result.Matches[SqlGrammar.Item.Select.Name];
            if (select.IsMatch)
            {
                MatchResult leftParenthesis = select.Matches[SqlGrammar.Item.Select.LeftParenthesis];
                leftParenthesis.GetContext(context);
                MatchResult selectStatement = select.Matches[SqlGrammar.Item.Select.SelectStatement];
                selectStatement.GetContext(context);
                MatchResult rightParenthesis = select.Matches[SqlGrammar.Item.Select.RightParenthesis];
                rightParenthesis.GetContext(context);
                return;
            }
            MatchResult numberResult = result.Matches[SqlGrammar.Item.Number];
            if (numberResult.IsMatch)
            {
                numberResult.GetContext(context);
                return;
            }
            MatchResult stringResult = result.Matches[SqlGrammar.Item.String];
            if (stringResult.IsMatch)
            {
                stringResult.GetContext(context);
                return;
            }
            MatchResult nullResult = result.Matches[SqlGrammar.Item.Null];
            if (nullResult.IsMatch)
            {
                nullResult.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for the INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnInsertStatement(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult insert = result.Matches[SqlGrammar.InsertStatement.InsertKeyword];
            insert.GetContext(writer);
            writer.Write(' ');
            MatchResult into = result.Matches[SqlGrammar.InsertStatement.IntoKeyword];
            if (into.IsMatch)
            {
                into.GetContext(writer);
                writer.Write(' ');
            }
            MatchResult table = result.Matches[SqlGrammar.InsertStatement.Table];
            table.GetContext(writer);
            writer.Write(' ');
            MatchResult columns = result.Matches[SqlGrammar.InsertStatement.Columns.Name];
            if (columns.IsMatch)
            {
                MatchResult leftParenthesis = columns.Matches[SqlGrammar.InsertStatement.Columns.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult columnList = columns.Matches[SqlGrammar.InsertStatement.Columns.ColumnList];
                columnList.GetContext(writer);
                MatchResult rightParenthesis = columns.Matches[SqlGrammar.InsertStatement.Columns.RightParenthesis];
                rightParenthesis.GetContext(writer);
                writer.Write(' ');
            }
            MatchResult values = result.Matches[SqlGrammar.InsertStatement.Values.Name];
            if (values.IsMatch)
            {
                MatchResult valuesKeyword = values.Matches[SqlGrammar.InsertStatement.Values.ValuesKeyword];
                valuesKeyword.GetContext(writer);
                MatchResult leftParenthesis = values.Matches[SqlGrammar.InsertStatement.Values.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult valuesList = values.Matches[SqlGrammar.InsertStatement.Values.ValueList];
                if (valuesList.IsMatch)
                {
                    valuesList.GetContext(writer);
                }
                MatchResult rightParenthesis = values.Matches[SqlGrammar.InsertStatement.Values.RightParenthesis];
                rightParenthesis.GetContext(writer);
            }
            MatchResult select = result.Matches[SqlGrammar.InsertStatement.Select.Name];
            if (select.IsMatch)
            {
                MatchResult leftParenthesis = select.Matches[SqlGrammar.InsertStatement.Select.LeftParenthesis];
                leftParenthesis.GetContext(writer);
                MatchResult selectExpression = select.Matches[SqlGrammar.InsertStatement.Select.SelectExpression];
                selectExpression.GetContext(writer);
                MatchResult rightParenthesis = select.Matches[SqlGrammar.InsertStatement.Select.RightParenthesis];
                rightParenthesis.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for the column list in an INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnColumnList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.ColumnList.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.ColumnList.Multiple.First];
                first.GetContext(writer);
                MatchResult comma = multiple.Matches[SqlGrammar.ColumnList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.ColumnList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.ColumnList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for the UDPATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnUpdateStatement(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult updateKeyword = result.Matches[SqlGrammar.UpdateStatement.UpdateKeyword];
            updateKeyword.GetContext(writer);
            writer.Write(' ');
            MatchResult table = result.Matches[SqlGrammar.UpdateStatement.Table];
            table.GetContext(writer);
            writer.Write(' ');
            MatchResult setKeyword = result.Matches[SqlGrammar.UpdateStatement.SetKeyword];
            setKeyword.GetContext(writer);
            writer.Write(' ');
            MatchResult setterList = result.Matches[SqlGrammar.UpdateStatement.SetterList];
            setterList.GetContext(writer);
            MatchResult where = result.Matches[SqlGrammar.UpdateStatement.Where.Name];
            if (where.IsMatch)
            {
                writer.Write(' ');
                MatchResult whereKeyword = where.Matches[SqlGrammar.UpdateStatement.Where.WhereKeyword];
                whereKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult filterList = where.Matches[SqlGrammar.UpdateStatement.Where.FilterList];
                filterList.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for the list of setters in an UPDATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnSetterList(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.SetterList.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.SetterList.Multiple.First];
                first.GetContext(writer);
                MatchResult comma = multiple.Matches[SqlGrammar.SetterList.Multiple.Comma];
                comma.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.SetterList.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.SetterList.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a setter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnSetter(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult column = result.Matches[SqlGrammar.Setter.Column];
            column.GetContext(writer);
            writer.Write(' ');
            MatchResult assignment = result.Matches[SqlGrammar.Setter.Assignment];
            assignment.GetContext(writer);
            writer.Write(' ');
            MatchResult item = result.Matches[SqlGrammar.Setter.Item];
            item.GetContext(writer);
        }

        /// <summary>
        /// Generates output for the DELETE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnDeleteStatement(MatchResult result, object context)
        {
            TextWriter writer = (TextWriter)context;
            MatchResult delete = result.Matches[SqlGrammar.DeleteStatement.DeleteKeyword];
            delete.GetContext(writer);
            writer.Write(' ');
            MatchResult from = result.Matches[SqlGrammar.DeleteStatement.FromKeyword];
            if (from.IsMatch)
            {
                from.GetContext(writer);
                writer.Write(' ');
            }
            MatchResult table = result.Matches[SqlGrammar.DeleteStatement.Table];
            table.GetContext(writer);
            MatchResult where = result.Matches[SqlGrammar.DeleteStatement.Where.Name];
            if (where.IsMatch)
            {
                writer.Write(' ');
                MatchResult whereKeyword = result.Matches[SqlGrammar.DeleteStatement.Where.WhereKeyword];
                whereKeyword.GetContext(writer);
                writer.Write(' ');
                MatchResult filterList = result.Matches[SqlGrammar.DeleteStatement.Where.FilterList];
                filterList.GetContext(writer);
            }
        }

        /// <summary>
        /// Generates output for a multipart identifier.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnMultipartIdentifier(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.MultipartIdentifier.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.First];
                first.GetContext(writer);
                MatchResult dot = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Dot];
                dot.GetContext(writer);
                MatchResult remaining = multiple.Matches[SqlGrammar.MultipartIdentifier.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.MultipartIdentifier.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a token.
        /// </summary>
        /// <param name="token">The token that was matched.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnToken(string token, object context)
        {
            TextWriter writer = (TextWriter)context;
            writer.Write(token);
        }

        /// <summary>
        /// Generates the output for a SELECT combiner.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnSelectCombiner(MatchResult result, object context)
        {
            MatchResult unionAll = result.Matches[SqlGrammar.SelectCombiner.UnionAll];
            if (unionAll.IsMatch)
            {
                unionAll.GetContext(context);
                return;
            }
            MatchResult union = result.Matches[SqlGrammar.SelectCombiner.Union];
            if (union.IsMatch)
            {
                union.GetContext(context);
                return;
            }
            MatchResult intersect = result.Matches[SqlGrammar.SelectCombiner.Intersect];
            if (intersect.IsMatch)
            {
                intersect.GetContext(context);
                return;
            }
            MatchResult except = result.Matches[SqlGrammar.SelectCombiner.Except];
            if (except.IsMatch)
            {
                except.GetContext(context);
                return;
            }
            MatchResult minus = result.Matches[SqlGrammar.SelectCombiner.Minus];
            if (minus.IsMatch)
            {
                minus.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for the distinct qualifier.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnDistinctQualifier(MatchResult result, object context)
        {
            MatchResult distinct = result.Matches[SqlGrammar.DistinctQualifier.Distinct];
            if (distinct.IsMatch)
            {
                distinct.GetContext(context);
                return;
            }
            MatchResult all = result.Matches[SqlGrammar.DistinctQualifier.All];
            if (all.IsMatch)
            {
                all.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for an order direction.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnOrderDirection(MatchResult result, object context)
        {
            MatchResult descending = result.Matches[SqlGrammar.OrderDirection.Descending];
            if (descending.IsMatch)
            {
                descending.GetContext(context);
                return;
            }
            MatchResult ascending = result.Matches[SqlGrammar.OrderDirection.Ascending];
            if (ascending.IsMatch)
            {
                ascending.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a null placement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnNullPlacement(MatchResult result, object context)
        {
            MatchResult nullsFirst = result.Matches[SqlGrammar.NullPlacement.NullsFirst];
            if (nullsFirst.IsMatch)
            {
                nullsFirst.GetContext(context);
                return;
            }
            MatchResult nullsLast = result.Matches[SqlGrammar.NullPlacement.NullsLast];
            if (nullsLast.IsMatch)
            {
                nullsLast.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates the output for a join type that is filtered.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnFilteredJoinType(MatchResult result, object context)
        {
            MatchResult inner = result.Matches[SqlGrammar.FilteredJoinType.InnerJoin];
            if (inner.IsMatch)
            {
                inner.GetContext(context);
                return;
            }
            MatchResult left = result.Matches[SqlGrammar.FilteredJoinType.LeftOuterJoin];
            if (left.IsMatch)
            {
                left.GetContext(context);
                return;
            }
            MatchResult right = result.Matches[SqlGrammar.FilteredJoinType.RightOuterJoin];
            if (right.IsMatch)
            {
                right.GetContext(context);
                return;
            }
            MatchResult full = result.Matches[SqlGrammar.FilteredJoinType.FullOuterJoin];
            if (full.IsMatch)
            {
                full.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for a comparison operator.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnComparisonOperator(MatchResult result, object context)
        {
            MatchResult equalTo = result.Matches[SqlGrammar.ComparisonOperator.EqualTo];
            if (equalTo.IsMatch)
            {
                equalTo.GetContext(context);
                return;
            }
            MatchResult notEqualTo = result.Matches[SqlGrammar.ComparisonOperator.NotEqualTo];
            if (notEqualTo.IsMatch)
            {
                notEqualTo.GetContext(context);
                return;
            }
            MatchResult lessThanEqualTo = result.Matches[SqlGrammar.ComparisonOperator.LessThanEqualTo];
            if (lessThanEqualTo.IsMatch)
            {
                lessThanEqualTo.GetContext(context);
                return;
            }
            MatchResult greaterThanEqualTo = result.Matches[SqlGrammar.ComparisonOperator.GreaterThanEqualTo];
            if (greaterThanEqualTo.IsMatch)
            {
                greaterThanEqualTo.GetContext(context);
                return;
            }
            MatchResult lessThan = result.Matches[SqlGrammar.ComparisonOperator.LessThan];
            if (lessThan.IsMatch)
            {
                lessThan.GetContext(context);
                return;
            }
            MatchResult greaterThan = result.Matches[SqlGrammar.ComparisonOperator.GreaterThan];
            if (greaterThan.IsMatch)
            {
                greaterThan.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for filters combined with an Or.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnOrConjunction(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.OrConjunction.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.OrConjunction.Multiple.First];
                first.GetContext(writer);
                writer.Write(' ');
                MatchResult or = multiple.Matches[SqlGrammar.OrConjunction.Multiple.Or];
                or.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.OrConjunction.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.OrConjunction.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }

        /// <summary>
        /// Generates output for filters combined with an AND.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected override void OnAndConjunction(MatchResult result, object context)
        {
            MatchResult multiple = result.Matches[SqlGrammar.AndConjunction.Multiple.Name];
            if (multiple.IsMatch)
            {
                TextWriter writer = (TextWriter)context;
                MatchResult first = multiple.Matches[SqlGrammar.AndConjunction.Multiple.First];
                first.GetContext(writer);
                writer.Write(' ');
                MatchResult or = multiple.Matches[SqlGrammar.AndConjunction.Multiple.And];
                or.GetContext(writer);
                writer.Write(' ');
                MatchResult remaining = multiple.Matches[SqlGrammar.AndConjunction.Multiple.Remaining];
                remaining.GetContext(writer);
                return;
            }
            MatchResult single = result.Matches[SqlGrammar.AndConjunction.Single];
            if (single.IsMatch)
            {
                single.GetContext(context);
                return;
            }
        }
    }
}
