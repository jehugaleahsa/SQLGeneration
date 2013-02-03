using System;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Identifies the expressions making up a SQL statement.
    /// </summary>
    public class SqlGrammar : Grammar
    {
        /// <summary>
        /// Initializes a new instance of a SqlGrammar.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to retrieve SQL tokens from.</param>
        public SqlGrammar(SqlTokenizer tokenizer)
            : base(tokenizer)
        {
            defineStart();
            defineSelectStatement();
            defineSelectExpression();
            defineSelectSpecification();
            defineOrderByList();
            defineOrderByItem();
            defineArithmeticExpression();
            defineProjectionList();
            defineProjectionItem();
            defineFromList();
            defineJoinItem();
            defineFunctionCall();
            defineJoin();
            defineJoinPrime();
            defineFilterList();
            defineFilter();
            defineValueList();
            defineGroupByList();
            defineExpression();
            defineInsertStatement();
            defineColumnList();
            defineUpdateStatement();
            defineSetterList();
            defineSetter();
            defineDeleteStatement();
            defineMultipartIdentifier();
        }

        /// <summary>
        /// Gets the top-level expression that defines the SQL grammar.
        /// </summary>
        public string StartExpression 
        {
            get { return "start"; }
        }

        private void defineStart()
        {
            Define("start")
                .Add("statement", true, Options()
                    .Add("SelectStatement", Expression("SelectStatement"))
                    .Add("InsertStatement", Expression("InsertStatement"))
                    .Add("UpdateStatement", Expression("UpdateStatement"))
                    .Add("DeleteStatement", Expression("DeleteStatement")));
        }

        private void defineSelectStatement()
        {
            Define("SelectStatement")
                .Add("SelectExpression", true, Expression("SelectExpression"))
                .Add("OrderBy", false, Define()
                    .Add("OrderBy", true, Token("OrderBy"))
                    .Add("OrderByList", true, Expression("OrderByList")));
        }

        private void defineSelectExpression()
        {
            Define("SelectExpression")
                .Add("First", true, Options()
                    .Add("Wrapped", Define()
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("SelectExpression", true, Expression("SelectExpression"))
                        .Add("RightParenthesis", true, Expression("RightParenthesis")))
                    .Add("SelectSpecification", Expression("SelectSpecification")))
                .Add("Remaining", false, Define()
                    .Add("Combiner", true, Token("SelectCombiner"))
                    .Add("SelectExpression", true, Expression("SelectExpression")));
        }

        private void defineSelectSpecification()
        {
            Define("SelectSpecification")
                .Add("Select", true, Token("Select"))
                .Add("DistinctQualifier", false, Token("DistinctQualifier"))
                .Add("Top", false, Define()
                    .Add("Top", true, Token("Top"))
                    .Add("Expression", true, Expression("ArithmeticExpression"))
                    .Add("Percent", false, Token("Percent"))
                    .Add("WithTies", false, Token("WithTies")))
                .Add("ProjectionList", true, Expression("ProjectionList"))
                .Add("From", false, Define()
                    .Add("From", true, Token("From"))
                    .Add("FromList", true, Expression("FromList")))
                .Add("Where", false, Define()
                    .Add("Where", true, Token("Where"))
                    .Add("FilterList", true, Expression("FilterList")))
                .Add("GroupBy", false, Define()
                    .Add("GroupBy", true, Token("GroupBy"))
                    .Add("GroupByList", true, Expression("GroupByList")))
                .Add("Having", false, Define()
                    .Add("Having", true, Token("Having"))
                    .Add("FilterList", true, Expression("FilterList")));
        }

        private void defineOrderByList()
        {
            Define("OrderByList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("OrderByItem"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("OrderByItem")))
                    .Add("Single", Expression("OrderByItem")));
        }

        private void defineOrderByItem()
        {
            Define("OrderByItem")
                .Add("Item", true, Options()
                    .Add("Expression", Expression("Expression"))
                    .Add("Alias", Token("Identifier")))
                .Add("OrderDirection", false, Token("OrderDirection"))
                .Add("NullPlacement", false, Token("NullPlacement"));
        }

        private void defineArithmeticExpression()
        {
            Define("ArithmeticExpression")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("Expression"))
                        .Add("ArithmeticOperator", true, Token("ArithmeticOperator"))
                        .Add("Right", true, Expression("ArithmeticExpression")))
                    .Add("Single", Expression("Expression")));
        }

        private void defineProjectionList()
        {
            Define("ProjectionList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("ProjectionItem"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("ProjectionList")))
                    .Add("Single", Expression("ProjectionItem")));
        }

        private void defineProjectionItem()
        {
            Define("ProjectionItem")
                .Add("Options", true, Options()
                    .Add("Star", Define()
                        .Add("Qualifier", false, Define()
                            .Add("ColumnSource", true, Options()
                                .Add("Table", Expression("MultipartIdentifier"))
                                .Add("Alias", Token("Identifier")))
                            .Add("Dot", true, Token("Dot")))
                        .Add("Star", true, Token("Star")))
                    .Add("Expression", Define()
                        .Add("Expression", true, Expression("Expression"))
                        .Add("Alias", false, Define()
                            .Add("AliasIndicator", false, Token("AliasIndicator"))
                            .Add("Alias", true, Token("Identifier")))));
        }

        private void defineFromList()
        {
            Define("FromList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("Join"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("FromList")))
                    .Add("Single", Expression("Join")));
        }

        private void defineJoinItem()
        {
            Define("JoinItem")
                .Add("Source", true, Options()
                    .Add("Table", Expression("MultipartIdentifier"))
                    .Add("FunctionCall", Expression("FunctionCall"))
                    .Add("SelectExpression", Expression("SelectExpression")))
                .Add("Alias", false, Define()
                    .Add("AliasIndicator", false, Expression("AS"))
                    .Add("Alias", true, Expression("Identifier")));
        }

        private void defineFunctionCall()
        {
            Define("FunctionCall")
                .Add("Name", true, Expression("MultipartIdentifier"))
                .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                .Add("Arguments", false, Expression("ValueList"))
                .Add("RightParenthesis", true, Token("RightParenthesis"));
        }

        private void defineJoin()
        {
            Define("Join")
                .Add("Options", true, Options()
                    .Add("Wrapped", Define()
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("Join", true, Expression("Join"))
                        .Add("RightParenthesis", true, Token("RightParenthesis")))
                    .Add("Joined", Define()
                        .Add("JoinItem", true, Expression("JoinItem"))
                        .Add("JoinPrime", true, Expression("JoinPrime"))));
        }

        private void defineJoinPrime()
        {
            Define("JoinPrime")
                .Add("Options", true, Options()
                    .Add("Joined", Define()
                        .Add("JoinType", true, Token("JoinType"))
                        .Add("JoinItem", true, Expression("JoinItem"))
                        .Add("On", false, Define()
                            .Add("On", true, Token("On"))
                            .Add("FilterList", true, Expression("FilterList")))
                        .Add("JoinPrime", true, Expression("JoinPrime")))
                    .Add("Empty", Define()));
        }

        private void defineFilterList()
        {
            Define("FilterList")
                .Add("Options", true, Options()
                    .Add("Wrapped", Define()
                        .Add("Not", false, Token("Not"))
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("FilterList", true, Expression("FilterList"))
                        .Add("RightParenthesis", true, Token("RightParenthesis")))
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("Filter"))
                        .Add("Conjunction", true, Token("Conjunction"))
                        .Add("Right", true, Expression("FilterList")))
                    .Add("Single", Expression("Filter")));
        }

        private void defineFilter()
        {
            Define("Filter")
                .Add("Options", true, Options()
                    .Add("Wrapped", Define()
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("Filter", true, Expression("Filter"))
                        .Add("RightParenthesis", true, Token("RightParenthesis")))
                    .Add("Not", Define()
                        .Add("Not", true, Token("Not"))
                        .Add("Filter", true, Expression("Filter")))
                    .Add("Binary", Define()
                        .Add("Left", true, Expression("Expression"))
                        .Add("ComparisonOperator", true, Token("ComparisonOperator"))
                        .Add("Right", true, Expression("Expression")))
                    .Add("Between", Define()
                        .Add("Expression", true, Expression("Expression"))
                        .Add("Not", false, Token("Not"))
                        .Add("Between", true, Token("Between"))
                        .Add("LowerBound", true, Expression("Expression"))
                        .Add("And", true, Expression("And"))
                        .Add("UpperBound", true, Expression("Expression")))
                    .Add("Like", Define()
                        .Add("Expression", true, Expression("Expression"))
                        .Add("Not", false, Token("Not"))
                        .Add("Like", true, Token("Like"))
                        .Add("Value", true, Token("String")))
                    .Add("Is", Define()
                        .Add("Expression", true, Expression("Expression"))
                        .Add("Is", true, Token("Is"))
                        .Add("Not", false, Token("Not"))
                        .Add("Null", true, Token("Null")))
                    .Add("In", Define()
                        .Add("Expression", true, Expression("Expression"))
                        .Add("Not", false, Token("In"))
                        .Add("In", true, Token("In"))
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("ValueSource", true, Options()
                            .Add("SelectExpression", Expression("SelectExpression"))
                            .Add("ValueList", Expression("ValueList")))
                        .Add("RightParenthesis", true, Token("RightParenthesis"))));
        }

        private void defineValueList()
        {
            Define("ValueList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("Expression"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("ValueList")))
                    .Add("Single", Expression("Expression")));
        }

        private void defineGroupByList()
        {
            Define("GroupByList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("Expression"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("GroupByList")))
                    .Add("Single", Expression("Expression")));
        }

        private void defineExpression()
        {
            Define("Expression")
                .Add("Options", true, Options()
                    .Add("Column", Expression("MultipartIdentifier"))
                    .Add("FunctionCall", Expression("FunctionCall"))
                    .Add("ArithmeticExpression", Expression("ArithmeticExpression"))
                    .Add("SelectExpression", Expression("SelectExpression"))
                    .Add("Number", Token("Number"))
                    .Add("String", Token("String"))
                    .Add("Null", Token("Null")));
        }

        private void defineInsertStatement()
        {
            Define("InsertStatement")
                .Add("Insert", true, Token("Insert"))
                .Add("Into", false, Token("Into"))
                .Add("Table", true, Expression("MultipartIdentifier"))
                .Add("ColumnList", false, Define()
                    .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                    .Add("ColumnList", true, Expression("ColumnList"))
                    .Add("RightParenthesis", true, Token("RightParenthesis")))
                .Add("ValueSource", true, Options()
                    .Add("ValueList", Define()
                        .Add("Values", true, Token("Values"))
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("ValueList", true, Expression("ValueList"))
                        .Add("RightParenthesis", true, Token("RightParenthesis")))
                    .Add("SelectExpression", Define()
                        .Add("LeftParenthesis", true, Token("LeftParenthesis"))
                        .Add("SelectExpression", true, Expression("SelectExpression"))
                        .Add("RightParenthesis", true, Token("RightParenthesis"))));

        }

        private void defineColumnList()
        {
            Define("ColumnList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("MultipartIdentifier"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("ColumnList")))
                    .Add("Single", Expression("MultipartIdentifier")));
        }

        private void defineUpdateStatement()
        {
            Define("UpdateStatement")
                .Add("Update", true, Token("Update"))
                .Add("Table", true, Expression("MultipartIdentifier"))
                .Add("Set", true, Token("Set"))
                .Add("SetterList", true, Expression("SetterList"))
                .Add("Where", false, Define()
                    .Add("Where", true, Token("Where"))
                    .Add("FilterList", true, Expression("FilterList")));
        }

        private void defineSetterList()
        {
            Define("SetterList")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Expression("Setter"))
                        .Add("Comma", true, Token("Comma"))
                        .Add("Right", true, Expression("SetterList")))
                    .Add("Single", Expression("Setter")));
        }

        private void defineSetter()
        {
            Define("Setter")
                .Add("Column", true, Expression("MultipartIdentifier"))
                .Add("Assignment", true, Token("Assignment"))
                .Add("Expression", true, Expression("Expression"));
        }

        private void defineDeleteStatement()
        {
            Define("DeleteStatement")
                .Add("Delete", true, Token("Delete"))
                .Add("From", false, Token("From"))
                .Add("Table", true, Expression("MultipartIdentifier"))
                .Add("Where", false, Define()
                    .Add("Where", true, Token("Where"))
                    .Add("FilterList", true, Expression("FilterList")));
        }

        private void defineMultipartIdentifier()
        {
            Define("MultipartIdentifier")
                .Add("Options", true, Options()
                    .Add("Multiple", Define()
                        .Add("Left", true, Token("Identifier"))
                        .Add("Dot", true, Token("Dot"))
                        .Add("Right", true, Expression("MultipartIdentifier")))
                    .Add("Single", Token("Identifier")));
        }
    }
}
