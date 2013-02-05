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
        /// Gets the result of the operations.
        /// </summary>
        /// <returns>The result.</returns>
        protected object GetResult(IEnumerable<string> tokenStream)
        {
            Parser parser = new Parser(grammar);
            registerHandlers(parser);
            ITokenSource tokenSource = grammar.Tokenizer.CreateTokenSource(tokenStream);
            return parser.Parse(SqlGrammar.Start.Name, tokenSource);
        }

        private void registerHandlers(Parser parser)
        {
            parser.RegisterHandler(SqlGrammar.Start.Name, OnStart);
            parser.RegisterHandler(SqlGrammar.SelectStatement.Name, OnSelectStatement);
            parser.RegisterHandler(SqlGrammar.SelectExpression.Name, OnSelectExpression);
            parser.RegisterHandler(SqlGrammar.SelectSpecification.Name, OnSelectSpecification);
            parser.RegisterHandler(SqlGrammar.OrderByList.Name, OnOrderByList);
            parser.RegisterHandler(SqlGrammar.OrderByItem.Name, OnOrderByItem);
            parser.RegisterHandler(SqlGrammar.ArithmeticExpression.Name, OnArithmeticExpression);
            parser.RegisterHandler(SqlGrammar.ProjectionList.Name, OnProjectionList);
            parser.RegisterHandler(SqlGrammar.ProjectionItem.Name, OnProjectionItem);
            parser.RegisterHandler(SqlGrammar.FromList.Name, OnFromList);
            parser.RegisterHandler(SqlGrammar.JoinItem.Name, OnJoinItem);
            parser.RegisterHandler(SqlGrammar.FunctionCall.Name, OnFunctionCall);
            parser.RegisterHandler(SqlGrammar.Join.Name, OnJoin);
            parser.RegisterHandler(SqlGrammar.JoinPrime.Name, OnJoinPrime);
            parser.RegisterHandler(SqlGrammar.FilterList.Name, OnFilterList);
            parser.RegisterHandler(SqlGrammar.Filter.Name, OnFilter);
            parser.RegisterHandler(SqlGrammar.ValueList.Name, OnValueList);
            parser.RegisterHandler(SqlGrammar.GroupByList.Name, OnGroupByList);
            parser.RegisterHandler(SqlGrammar.Item.Name, OnItem);
            parser.RegisterHandler(SqlGrammar.InsertStatement.Name, OnInsertStatement);
            parser.RegisterHandler(SqlGrammar.ColumnList.Name, OnColumnList);
            parser.RegisterHandler(SqlGrammar.UpdateStatement.Name, OnUpdateStatement);
            parser.RegisterHandler(SqlGrammar.SetterList.Name, OnSetterList);
            parser.RegisterHandler(SqlGrammar.Setter.Name, OnSetter);
            parser.RegisterHandler(SqlGrammar.DeleteStatement.Name, OnDeleteStatement);
            parser.RegisterHandler(SqlGrammar.MultipartIdentifier.Name, OnMultipartIdentifier);
        }

        /// <summary>
        /// Generates output for the Start statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnStart(MatchResult result);

        /// <summary>
        /// Generates output for the SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnSelectStatement(MatchResult result);

        /// <summary>
        /// Generates output for the SELECT expression.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnSelectExpression(MatchResult result);

        /// <summary>
        /// Generates output for the SELECT specification.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnSelectSpecification(MatchResult result);

        /// <summary>
        /// Generates output for the ORDER BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnOrderByList(MatchResult result);

        /// <summary>
        /// Generates output for an ORDER BY item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnOrderByItem(MatchResult result);

        /// <summary>
        /// Generates output for an arithmetic expression.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnArithmeticExpression(MatchResult result);

        /// <summary>
        /// Generates output for the projection list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnProjectionList(MatchResult result);

        /// <summary>
        /// Generates output for a projection item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnProjectionItem(MatchResult result);

        /// <summary>
        /// Generates output for the FROM list of a SELECT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnFromList(MatchResult result);

        /// <summary>
        /// Generates output for a join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnJoinItem(MatchResult result);

        /// <summary>
        /// Generates output for a function call.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnFunctionCall(MatchResult result);

        /// <summary>
        /// Generates output for a join.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnJoin(MatchResult result);

        /// <summary>
        /// Generates output for the next join item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnJoinPrime(MatchResult result);

        /// <summary>
        /// Generates output for a filter list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnFilterList(MatchResult result);

        /// <summary>
        /// Generates output for a filter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnFilter(MatchResult result);

        /// <summary>
        /// Generates output for a value list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnValueList(MatchResult result);

        /// <summary>
        /// Generates output for a GROUP BY list.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnGroupByList(MatchResult result);

        /// <summary>
        /// Generates output for an item.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnItem(MatchResult result);

        /// <summary>
        /// Generates output for the INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnInsertStatement(MatchResult result);

        /// <summary>
        /// Generates output for the column list in an INSERT statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnColumnList(MatchResult result);

        /// <summary>
        /// Generates output for the UDPATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnUpdateStatement(MatchResult result);

        /// <summary>
        /// Generates output for the list of setters in an UPDATE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnSetterList(MatchResult result);

        /// <summary>
        /// Generates output for a setter.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnSetter(MatchResult result);

        /// <summary>
        /// Generates output for the DELETE statement.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnDeleteStatement(MatchResult result);

        /// <summary>
        /// Generates output for a multipart identifier.
        /// </summary>
        /// <param name="result">The results returned by the parser.</param>
        /// <returns>The generated output.</returns>
        protected abstract object OnMultipartIdentifier(MatchResult result);
    }
}
