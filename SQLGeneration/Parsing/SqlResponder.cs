using System;
using System.Collections.Generic;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Provides the methods that must be overridden in order to properly process SQL expressions.
    /// </summary>
    public abstract class SqlResponder
    {
        private readonly SqlGrammar grammar;

        /// <summary>
        /// Initializes a new instance of a SqlResponder.
        /// </summary>
        /// <param name="grammar">The grammar to use.</param>
        protected SqlResponder(SqlGrammar grammar)
        {
            if (grammar == null)
            {
                grammar = new SqlGrammar();
            }
            this.grammar = grammar;
        }

        /// <summary>
        /// Extracts expressions from the token stream and calls the corresponding handler.
        /// </summary>
        /// <param name="tokenStream">The sequence of SQL tokens.</param>
        /// <param name="context">The context to pass among the expressions.</param>
        protected void GetResult(IEnumerable<string> tokenStream, object context)
        {
            ITokenSource tokenSource = grammar.Tokenizer.CreateTokenSource(tokenStream);
            Parser parser = new Parser(grammar);
            registerHandlers(parser);
            parser.Parse(SqlGrammar.Start.Name, tokenSource, context);
        }

        private void registerHandlers(Parser parser)
        {
            parser.RegisterHandler(SqlGrammar.Start.Name, OnStart);
            parser.RegisterHandler(SqlGrammar.SelectStatement.Name, OnSelectStatement);
            parser.RegisterHandler(SqlGrammar.SelectExpression.Name, OnSelectExpression);
            parser.RegisterHandler(SqlGrammar.SelectCombiner.Name, OnSelectCombiner);
            parser.RegisterHandler(SqlGrammar.SelectSpecification.Name, OnSelectSpecification);
            parser.RegisterHandler(SqlGrammar.DistinctQualifier.Name, OnDistinctQualifier);
            parser.RegisterHandler(SqlGrammar.OrderByList.Name, OnOrderByList);
            parser.RegisterHandler(SqlGrammar.OrderByItem.Name, OnOrderByItem);
            parser.RegisterHandler(SqlGrammar.OrderDirection.Name, OnOrderDirection);
            parser.RegisterHandler(SqlGrammar.NullPlacement.Name, OnNullPlacement);
            parser.RegisterHandler(SqlGrammar.AdditiveExpression.Name, OnAdditiveEpression);
            parser.RegisterHandler(SqlGrammar.AdditiveOperator.Name, OnAdditiveOperator);
            parser.RegisterHandler(SqlGrammar.MultiplicitiveExpression.Name, OnMultiplicitiveExpression);
            parser.RegisterHandler(SqlGrammar.MultiplicitiveOperator.Name, OnMultiplicitiveOperator);
            parser.RegisterHandler(SqlGrammar.ProjectionList.Name, OnProjectionList);
            parser.RegisterHandler(SqlGrammar.ProjectionItem.Name, OnProjectionItem);
            parser.RegisterHandler(SqlGrammar.FromList.Name, OnFromList);
            parser.RegisterHandler(SqlGrammar.JoinItem.Name, OnJoinItem);
            parser.RegisterHandler(SqlGrammar.FunctionCall.Name, OnFunctionCall);
            parser.RegisterHandler(SqlGrammar.Join.Name, OnJoin);
            parser.RegisterHandler(SqlGrammar.JoinPrime.Name, OnJoinPrime);
            parser.RegisterHandler(SqlGrammar.FilteredJoinType.Name, OnFilteredJoinType);
            parser.RegisterHandler(SqlGrammar.FilterList.Name, OnFilterList);
            parser.RegisterHandler(SqlGrammar.Filter.Name, OnFilter);
            parser.RegisterHandler(SqlGrammar.ComparisonOperator.Name, OnComparisonOperator);
            parser.RegisterHandler(SqlGrammar.OrConjunction.Name, OnOrConjunction);
            parser.RegisterHandler(SqlGrammar.AndConjunction.Name, OnAndConjunction);
            parser.RegisterHandler(SqlGrammar.ValueList.Name, OnValueList);
            parser.RegisterHandler(SqlGrammar.GroupByList.Name, OnGroupByList);
            parser.RegisterHandler(SqlGrammar.NonArithmeticItem.Name, OnNonArithmeticItem);
            parser.RegisterHandler(SqlGrammar.Item.Name, OnItem);
            parser.RegisterHandler(SqlGrammar.InsertStatement.Name, OnInsertStatement);
            parser.RegisterHandler(SqlGrammar.ColumnList.Name, OnColumnList);
            parser.RegisterHandler(SqlGrammar.UpdateStatement.Name, OnUpdateStatement);
            parser.RegisterHandler(SqlGrammar.SetterList.Name, OnSetterList);
            parser.RegisterHandler(SqlGrammar.Setter.Name, OnSetter);
            parser.RegisterHandler(SqlGrammar.DeleteStatement.Name, OnDeleteStatement);
            parser.RegisterHandler(SqlGrammar.MultipartIdentifier.Name, OnMultipartIdentifier);
            parser.RegisterTokenHandle(OnToken);
        }

        /// <summary>
        /// Generates output for the Start statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnStart(MatchResult result, object context);

        /// <summary>
        /// Generates output for the SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnSelectStatement(MatchResult result, object context);

        /// <summary>
        /// Generates output for the SELECT expression.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnSelectExpression(MatchResult result, object context);

        /// <summary>
        /// Generates the output for a SELECT combiner.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnSelectCombiner(MatchResult result, object context);

        /// <summary>
        /// Generates output for the SELECT specification.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnSelectSpecification(MatchResult result, object context);

        /// <summary>
        /// Generates output for the distinct qualifier.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnDistinctQualifier(MatchResult result, object context);

        /// <summary>
        /// Generates output for the ORDER BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnOrderByList(MatchResult result, object context);

        /// <summary>
        /// Generates output for an ORDER BY item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnOrderByItem(MatchResult result, object context);

        /// <summary>
        /// Generates output for an order direction.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnOrderDirection(MatchResult result, object context);

        /// <summary>
        /// Generates output for a null placement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnNullPlacement(MatchResult result, object context);

        /// <summary>
        /// Generates output for an arithmetic expression adding or subtracting values.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnAdditiveEpression(MatchResult result, object context);

        /// <summary>
        /// Generates output for the plus or minus operator.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnAdditiveOperator(MatchResult result, object context);

        /// <summary>
        /// Generates output for an arithmetic expression multiplying or dividing values.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnMultiplicitiveExpression(MatchResult result, object context);

        /// <summary>
        /// Generates output for a multiply or divide operator.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnMultiplicitiveOperator(MatchResult result, object context);

        /// <summary>
        /// Generates output for the projection list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnProjectionList(MatchResult result, object context);

        /// <summary>
        /// Generates output for a projection item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnProjectionItem(MatchResult result, object context);

        /// <summary>
        /// Generates output for the FROM list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnFromList(MatchResult result, object context);

        /// <summary>
        /// Generates output for a join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnJoinItem(MatchResult result, object context);

        /// <summary>
        /// Generates output for a function call.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnFunctionCall(MatchResult result, object context);

        /// <summary>
        /// Generates output for a join.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnJoin(MatchResult result, object context);

        /// <summary>
        /// Generates output for the next join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnJoinPrime(MatchResult result, object context);

        /// <summary>
        /// Generates the output for a join type that is filtered.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnFilteredJoinType(MatchResult result, object context);

        /// <summary>
        /// Generates output for a filter list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnFilterList(MatchResult result, object context);

        /// <summary>
        /// Generates output for a filter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnFilter(MatchResult result, object context);

        /// <summary>
        /// Generates output for a comparison operator.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnComparisonOperator(MatchResult result, object context);

        /// <summary>
        /// Generates output for filters combined with an Or.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnOrConjunction(MatchResult result, object context);

        /// <summary>
        /// Generates output for filters combined with an AND.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnAndConjunction(MatchResult result, object context);

        /// <summary>
        /// Generates output for a value list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnValueList(MatchResult result, object context);

        /// <summary>
        /// Generates output for a GROUP BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnGroupByList(MatchResult result, object context);

        /// <summary>
        /// Generates output for an item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnNonArithmeticItem(MatchResult result, object context);

        /// <summary>
        /// Generates output for an item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnItem(MatchResult result, object context);

        /// <summary>
        /// Generates output for the INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnInsertStatement(MatchResult result, object context);

        /// <summary>
        /// Generates output for the column list in an INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnColumnList(MatchResult result, object context);

        /// <summary>
        /// Generates output for the UDPATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnUpdateStatement(MatchResult result, object context);

        /// <summary>
        /// Generates output for the list of setters in an UPDATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnSetterList(MatchResult result, object context);

        /// <summary>
        /// Generates output for a setter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnSetter(MatchResult result, object context);

        /// <summary>
        /// Generates output for the DELETE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnDeleteStatement(MatchResult result, object context);

        /// <summary>
        /// Generates output for a multipart identifier.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnMultipartIdentifier(MatchResult result, object context);

        /// <summary>
        /// Generates output for a token.
        /// </summary>
        /// <param name="token">The token returned by the parser.</param>
        /// <param name="context">Holds information that should be passed from the outer expression to the inner expression.</param>
        protected abstract void OnToken(string token, object context);
    }
}
