//using System;
//using SQLGeneration.Parsing;
//using System.Collections.Generic;

//namespace SQLGeneration.Formatting
//{
//    /// <summary>
//    /// Generates simple SQL text from a token source.
//    /// </summary>
//    public sealed class SimpleFormatter : SqlResponder
//    {
//        public SimpleFormatter(SqlTokenizer tokenizer, SqlGrammar grammar)
//        {
//        }

//        /// <summary>
//        /// Gets the command text.
//        /// </summary>
//        /// <returns>The command text.</returns>
//        public string GetCommandText()
//        {
//            object result = GetResult();
//            return (string)result;
//        }

//        /// <summary>
//        /// Generates output for the top-level statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnStart(MatchResult result);

//        /// <summary>
//        /// Generates output for the SELECT statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnSelectStatement(MatchResult result);

//        /// <summary>
//        /// Generates output for the SELECT expression.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnSelectExpression(MatchResult result);

//        /// <summary>
//        /// Generates output for the SELECT specification.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnSelectSpecification(MatchResult result);

//        /// <summary>
//        /// Generates output for the ORDER BY list.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnOrderByList(MatchResult result);

//        /// <summary>
//        /// Generates output for an ORDER BY item.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnOrderByItem(MatchResult result);

//        /// <summary>
//        /// Generates output for an arithmetic expression.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnArithmeticExpression(MatchResult result);

//        /// <summary>
//        /// Generates output for the projection list of a SELECT statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnProjectionList(MatchResult result);

//        /// <summary>
//        /// Generates output for a projection item.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnProjectionItem(MatchResult result);

//        /// <summary>
//        /// Generates output for the FROM list of a SELECT statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnFromList(MatchResult result);

//        /// <summary>
//        /// Generates output for a join item.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnJoinItem(MatchResult result);

//        /// <summary>
//        /// Generates output for a function call.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnFunctionCall(MatchResult result);

//        /// <summary>
//        /// Generates output for a join.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnJoin(MatchResult result);

//        /// <summary>
//        /// Generates output for the next join item.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnJoinPrime(MatchResult result);

//        /// <summary>
//        /// Generates output for a filter list.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnFilterList(MatchResult result);

//        /// <summary>
//        /// Generates output for a filter.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnFilter(MatchResult result);

//        /// <summary>
//        /// Generates output for a value list.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnValueList(MatchResult result);

//        /// <summary>
//        /// Generates output for a GROUP BY list.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnGroupByList(MatchResult result);

//        /// <summary>
//        /// Generates output for an item.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnItem(MatchResult result);

//        /// <summary>
//        /// Generates output for the INSERT statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnInsertStatement(MatchResult result);

//        /// <summary>
//        /// Generates output for the column list in an INSERT statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnColumnList(MatchResult result);

//        /// <summary>
//        /// Generates output for the UDPATE statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnUpdateStatement(MatchResult result);

//        /// <summary>
//        /// Generates output for the list of setters in an UPDATE statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnSetterList(MatchResult result);

//        /// <summary>
//        /// Generates output for a setter.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnSetter(MatchResult result);

//        /// <summary>
//        /// Generates output for the DELETE statement.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnDeleteStatement(MatchResult result);

//        /// <summary>
//        /// Generates output for a multipart identifier.
//        /// </summary>
//        /// <param name="result">The results returned by the parser.</param>
//        /// <returns>The generated output.</returns>
//        protected override object OnMultipartIdentifier(MatchResult result);
//    }
//}
