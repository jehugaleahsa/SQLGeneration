using System;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Provides the methods that must be overridden in order to properly process SQL expressions.
    /// </summary>
    public abstract class SqlResponder
    {
        /// <summary>
        /// Initializes a new instance of a SqlResponder.
        /// </summary>
        /// <param name="tokenizer">The SQL tokenizer.</param>
        /// <param name="grammar">The grammar to use.</param>
        protected SqlResponder(SqlTokenizer tokenizer, SqlGrammar grammar)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the result of the operations.
        /// </summary>
        /// <returns>The result.</returns>
        protected object GetResult()
        {
            throw new NotImplementedException();
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
