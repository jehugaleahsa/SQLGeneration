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
        /// <param name="registry">The token registry to retrieve SQL tokens from.</param>
        public SqlGrammar(SqlTokenRegistry registry = null)
            : base(registry == null ? new SqlTokenRegistry() : registry)
        {
            defineStart();
            defineSelectStatement();
            defineSelectExpression();
            defineSelectCombiner();
            defineSelectSpecification();
            defineDistinctQualifier();
            defineOrderByList();
            defineOrderByItem();
            defineOrderDirection();
            defineNullPlacement();
            defineAdditiveExpression();
            defineAdditiveOperator();
            defineMultiplicitiveExpression();
            defineMultiplicitiveOperator();
            defineProjectionList();
            defineProjectionItem();
            defineFromList();
            defineJoinItem();
            defineFunctionCall();
            defineJoin();
            defineJoinPrime();
            defineFilteredJoinType();
            defineFilterList();
            defineFilter();
            defineComparisonOperator();
            defineQuantifier();
            defineOrConjunction();
            defineAndConjunction();
            defineValueList();
            defineGroupByList();
            defineArithmeticItem();
            defineItem();
            defineInsertStatement();
            defineColumnList();
            defineUpdateStatement();
            defineSetterList();
            defineSetter();
            defineDeleteStatement();
            defineMultipartIdentifier();
        }

        #region Start

        /// <summary>
        /// Describes the structure of the top-level SQL grammar.
        /// </summary>
        public static class Start
        {
            /// <summary>
            /// Gets the identifier representing the start expression.
            /// </summary>
            public const string Name = "Start";

            /// <summary>
                /// Gets the name for the SELECT statement option.
                /// </summary>
            public const string SelectStatement = "select_statement";

            /// <summary>
            /// Gets the name for the INSERT statement option.
            /// </summary>
            public const string InsertStatement = "insert_statement";

            /// <summary>
            /// Gets the name for the UPDATE statement option.
            /// </summary>
            public const string UpdateStatement = "update_statement";

            /// <summary>
            /// Gets the name for the DELETE statement option.
            /// </summary>
            public const string DeleteStatement = "delete_statement";
        }

        private void defineStart()
        {
            Define(Start.Name)
                .Add(true, Options()
                    .Add(Start.SelectStatement, Expression(SelectStatement.Name))
                    .Add(Start.InsertStatement, Expression(InsertStatement.Name))
                    .Add(Start.UpdateStatement, Expression(UpdateStatement.Name))
                    .Add(Start.DeleteStatement, Expression(DeleteStatement.Name)));
        }

        #endregion

        #region SelectStatement

        /// <summary>
        /// Describes the structure of the SELECT statement.
        /// </summary>
        public static class SelectStatement
        {
            /// <summary>
            /// Gets the identifier representing the SELECT statement.
            /// </summary>
            public const string Name = "SelectStatement";

            /// <summary>
            /// Gets the name of the SELECT expression.
            /// </summary>
            public const string SelectExpression = "select_expression";

            /// <summary>
            /// Describes the structure of the optional ORDER BY clause.
            /// </summary>
            public static class OrderBy
            {
                /// <summary>
                /// Gets the identifier representing the ORDER BY clause.
                /// </summary>
                public const string Name = "OrderBy";

                /// <summary>
                /// Gets the name representing the ORDER BY keyword.
                /// </summary>
                public const string OrderByKeyword = "order_by";

                /// <summary>
                /// Gets the name representing the ORDER BY list.
                /// </summary>
                public const string OrderByList = "order_by_list";
            }
        }

        private void defineSelectStatement()
        {
            Define(SelectStatement.Name)
                .Add(SelectStatement.SelectExpression, true, Expression(SelectExpression.Name))
                .Add(SelectStatement.OrderBy.Name, false, Define()
                    .Add(SelectStatement.OrderBy.OrderByKeyword, true, Token(SqlTokenRegistry.OrderBy))
                    .Add(SelectStatement.OrderBy.OrderByList, true, Expression(OrderByList.Name)));
        }

        #endregion

        #region SelectExpression

        /// <summary>
        /// Describes the structure of the SELECT expression.
        /// </summary>
        public static class SelectExpression
        {
            /// <summary>
            /// Gets the name identifying the SELECT expression.
            /// </summary>
            public const string Name = "SelectExpression";

            /// <summary>
                /// Describes the structure of the leading SELECT expression when it is surrounded by parenthesis.
                /// </summary>
            public static class Wrapped
            {
                /// <summary>
                /// Gets the name identifying the leading SELECT expression when it is surrounded by parenthesis.
                /// </summary>
                public const string Name = "Wrapped";

                /// <summary>
                /// Gets the left parenthesis identifier.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the SELECT expression identifier.
                /// </summary>
                public const string SelectExpression = "select_expression";

                /// <summary>
                /// Gets the right parenthesis identifier.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Gets the SELECT specification identifier.
            /// </summary>
            public const string SelectSpecification = "select_specification";

            /// <summary>
            /// Describes the structure of a compound SELECT statement.
            /// </summary>
            public static class Remaining
            {
                /// <summary>
                /// Gets the identifier for a compound SELECT statement.
                /// </summary>
                public const string Name = "Remaining";

                /// <summary>
                /// Gets the SELECT statement combiner identifier.
                /// </summary>
                public const string Combiner = "combiner";

                /// <summary>
                /// Gets the identifier for the distinct qualifier.
                /// </summary>
                public const string DistinctQualifier = "distinct_qualifier";

                /// <summary>
                /// Gets the SELECT expression identifier.
                /// </summary>
                public const string SelectExpression = "select_expression";
            }
        }

        private void defineSelectExpression()
        {
            Define(SelectExpression.Name)
                .Add(true, Options()
                    .Add(SelectExpression.Wrapped.Name, Define()
                        .Add(SelectExpression.Wrapped.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(SelectExpression.Wrapped.SelectExpression, true, Expression(SelectExpression.Name))
                        .Add(SelectExpression.Wrapped.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                    .Add(SelectExpression.SelectSpecification, Expression(SelectSpecification.Name)))
                .Add(SelectExpression.Remaining.Name, false, Define()
                    .Add(SelectExpression.Remaining.Combiner, true, Expression(SelectCombiner.Name))
                    .Add(SelectExpression.Remaining.DistinctQualifier, false, Expression(DistinctQualifier.Name))
                    .Add(SelectExpression.Remaining.SelectExpression, true, Expression(SelectExpression.Name)));
        }

        #endregion

        #region SelectCombiner

        /// <summary>
        /// Describes the options for a SELECT combiner.
        /// </summary>
        public static class SelectCombiner
        {
            /// <summary>
            /// Gets the identifier indicating that the token is a SELECT combiner.
            /// </summary>
            public const string Name = "SelectCombiner";

            /// <summary>
            /// Gets the identifier for the UNION keyword.
            /// </summary>
            public const string Union = "Union";

            /// <summary>
            /// Gets the identifier for the INTERSECT keyword.
            /// </summary>
            public const string Intersect = "Intersect";

            /// <summary>
            /// Gets the identifier for the EXCEPT keyword.
            /// </summary>
            public const string Except = "Except";

            /// <summary>
            /// Gets the identifier for the MINUS keyword.
            /// </summary>
            public const string Minus = "Minus";
        }

        private void defineSelectCombiner()
        {
            Define(SelectCombiner.Name)
                .Add(true, Options()
                    .Add(SelectCombiner.Union, Token(SqlTokenRegistry.Union))
                    .Add(SelectCombiner.Intersect, Token(SqlTokenRegistry.Intersect))
                    .Add(SelectCombiner.Except, Token(SqlTokenRegistry.Except))
                    .Add(SelectCombiner.Minus, Token(SqlTokenRegistry.Minus)));
        }

        #endregion

        #region SelectSpecification

        /// <summary>
        /// Describes the structure of the SELECT specification.
        /// </summary>
        public static class SelectSpecification
        {
            /// <summary>
            /// Gets the name identifying the SELECT specification.
            /// </summary>
            public const string Name = "SelectSpecification";

            /// <summary>
            /// Gets the SELECT keyword identifier.
            /// </summary>
            public const string SelectKeyword = "select";

            /// <summary>
            /// Gets the distinct qualifier identifier.
            /// </summary>
            public const string DistinctQualifier = "distinct_qualifier";

            /// <summary>
            /// Describes the structure of the TOP clause.
            /// </summary>
            public static class Top
            {
                /// <summary>
                /// Gets the identifier for the TOP expression.
                /// </summary>
                public const string Name = "Top";

                /// <summary>
                /// Gets the TOP keyword identifier.
                /// </summary>
                public const string TopKeyword = "top";

                /// <summary>
                /// Gets the expression identifier.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the PERCENT keyword identifier.
                /// </summary>
                public const string PercentKeyword = "percent";

                /// <summary>
                /// Gets the WITH TIES keyword identifier.
                /// </summary>
                public const string WithTiesKeyword = "with_ties";
            }

            /// <summary>
            /// Get the projection list identifier.
            /// </summary>
            public const string ProjectionList = "projection_list";

            /// <summary>
            /// Describes the structure of the FROM clause.
            /// </summary>
            public static class From
            {
                /// <summary>
                /// Gets the identifier for the FROM expression.
                /// </summary>
                public const string Name = "From";

                /// <summary>
                /// Gets the FROM keyword identifier.
                /// </summary>
                public const string FromKeyword = "from";

                /// <summary>
                /// Gets the from list identifier.
                /// </summary>
                public const string FromList = "from_list";
            }

            /// <summary>
            /// Describes the structure of the WHERE clause.
            /// </summary>
            public static class Where
            {
                /// <summary>
                /// Get the identifier for the WHERE clause.
                /// </summary>
                public const string Name = "Where";

                /// <summary>
                /// Gets the WHERE keyword identifier.
                /// </summary>
                public const string WhereKeyword = "where";

                /// <summary>
                /// Gets the filter list identifier.
                /// </summary>
                public const string FilterList = "filter_list";
            }

            /// <summary>
            /// Describes the structure of the GROUP BY clause.
            /// </summary>
            public static class GroupBy
            {
                /// <summary>
                /// Gets the identifier for the GROUP BY clause.
                /// </summary>
                public const string Name = "GroupBy";

                /// <summary>
                /// Gets the GROUP BY keyword identifier.
                /// </summary>
                public const string GroupByKeyword = "group_by";

                /// <summary>
                /// Gets the group by list identifier.
                /// </summary>
                public const string GroupByList = "group_by_list";
            }

            /// <summary>
            /// Describes the structure of the HAVING clause.
            /// </summary>
            public static class Having
            {
                /// <summary>
                /// Gets the identifier for the HAVING clause.
                /// </summary>
                public const string Name = "Having";

                /// <summary>
                /// Gets the HAVING keyword identifier.
                /// </summary>
                public const string HavingKeyword = "having";

                /// <summary>
                /// Gets the filter list identifier.
                /// </summary>
                public const string FilterList = "filter_list";
            }
        }

        private void defineSelectSpecification()
        {
            Define(SelectSpecification.Name)
                .Add(SelectSpecification.SelectKeyword, true, Token(SqlTokenRegistry.Select))
                .Add(SelectSpecification.DistinctQualifier, false, Expression(DistinctQualifier.Name))
                .Add(SelectSpecification.Top.Name, false, Define()
                    .Add(SelectSpecification.Top.TopKeyword, true, Token(SqlTokenRegistry.Top))
                    .Add(SelectSpecification.Top.Expression, true, Expression(AdditiveExpression.Name))
                    .Add(SelectSpecification.Top.PercentKeyword, false, Token(SqlTokenRegistry.Percent))
                    .Add(SelectSpecification.Top.WithTiesKeyword, false, Token(SqlTokenRegistry.WithTies)))
                .Add(SelectSpecification.ProjectionList, true, Expression(ProjectionList.Name))
                .Add(SelectSpecification.From.Name, false, Define()
                    .Add(SelectSpecification.From.FromKeyword, true, Token(SqlTokenRegistry.From))
                    .Add(SelectSpecification.From.FromList, true, Expression(FromList.Name)))
                .Add(SelectSpecification.Where.Name, false, Define()
                    .Add(SelectSpecification.Where.WhereKeyword, true, Token(SqlTokenRegistry.Where))
                    .Add(SelectSpecification.Where.FilterList, true, Expression(FilterList.Name)))
                .Add(SelectSpecification.GroupBy.Name, false, Define()
                    .Add(SelectSpecification.GroupBy.GroupByKeyword, true, Token(SqlTokenRegistry.GroupBy))
                    .Add(SelectSpecification.GroupBy.GroupByList, true, Expression(GroupByList.Name)))
                .Add(SelectSpecification.Having.Name, false, Define()
                    .Add(SelectSpecification.Having.HavingKeyword, true, Token(SqlTokenRegistry.Having))
                    .Add(SelectSpecification.Having.FilterList, true, Expression(FilterList.Name)));
        }

        #endregion

        #region DistinctQualifier

        /// <summary>
        /// Describes the options for a distinct qualifier.
        /// </summary>
        public static class DistinctQualifier
        {
            /// <summary>
            /// Gets the identifier indicating that the token is a distinct qualifier.
            /// </summary>
            public const string Name = "DistinctQualifier";

            /// <summary>
            /// Gets the identifier for the DISTINCT keyword.
            /// </summary>
            public const string Distinct = "distinct";

            /// <summary>
            /// Gets the identifier for the ALL keyword.
            /// </summary>
            public const string All = "all";
        }

        private void defineDistinctQualifier()
        {
            Define(DistinctQualifier.Name)
                .Add(true, Options()
                    .Add(DistinctQualifier.Distinct, Token(SqlTokenRegistry.Distinct))
                    .Add(DistinctQualifier.All, Token(SqlTokenRegistry.All)));
        }

        #endregion

        #region OrderByList

        /// <summary>
        /// Describes the structure of the ORDER BY list.
        /// </summary>
        public static class OrderByList
        {
            /// <summary>
            /// Gets the name identifying the ORDER BY list.
            /// </summary>
            public const string Name = "OrderByList";

            /// <summary>
            /// Describes the structure of an order by list containing multiple items.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier for the multiple option.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the first order by item identifier.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";

                /// <summary>
                /// Gets the identifier for the rest of the order by list.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier for a single order by item.
            /// </summary>
            public const string Single = "single";
        }

        private void defineOrderByList()
        {
            Define(OrderByList.Name)
                .Add(true, Options()
                    .Add(OrderByList.Multiple.Name, Define()
                        .Add(OrderByList.Multiple.First, true, Expression(OrderByItem.Name))
                        .Add(OrderByList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(OrderByList.Multiple.Remaining, true, Expression(OrderByList.Name)))
                    .Add(OrderByList.Single, Expression(OrderByItem.Name)));
        }

        #endregion

        #region OrderByItem

        /// <summary>
        /// Describes the structure of the ORDER BY item.
        /// </summary>
        public static class OrderByItem
        {
            /// <summary>
            /// Gets the name identifying the ORDER BY item.
            /// </summary>
            public const string Name = "OrderByItem";

            /// <summary>
            /// Gets the identifier for the item being ordered.
            /// </summary>
            public const string Expression = "expression";

            /// <summary>
            /// Gets the identifier for the direction specifier.
            /// </summary>
            public const string OrderDirection = "order_direction";

            /// <summary>
            /// Gets the identifier for the null placement specifier.
            /// </summary>
            public const string NullPlacement = "null_placement";
        }

        private void defineOrderByItem()
        {
            Define(OrderByItem.Name)
                .Add(OrderByItem.Expression, true, Expression(ArithmeticItem.Name))
                .Add(OrderByItem.OrderDirection, false, Expression(OrderDirection.Name))
                .Add(OrderByItem.NullPlacement, false, Expression(NullPlacement.Name));
        }

        #endregion

        #region OrderDirection

        /// <summary>
        /// Describes the options for an ORDER BY direction.
        /// </summary>
        public static class OrderDirection
        {
            /// <summary>
            /// Gets the identifier indicating that the token is an ORDER direction.
            /// </summary>
            public const string Name = "OrderDirection";

            /// <summary>
            /// Gets the identifier for the DESC keyword.
            /// </summary>
            public const string Descending = "Descending";

            /// <summary>
            /// Gets the identifier for the ASC keyword.
            /// </summary>
            public const string Ascending = "Ascending";
        }

        private void defineOrderDirection()
        {
            Define(OrderDirection.Name)
                .Add(true, Options()
                    .Add(OrderDirection.Descending, Token(SqlTokenRegistry.Descending))
                    .Add(OrderDirection.Ascending, Token(SqlTokenRegistry.Ascending)));
        }

        #endregion

        #region NullPlacement

        /// <summary>
        /// Describes the options for null placement.
        /// </summary>
        public static class NullPlacement
        {
            /// <summary>
            /// Gets the identifier indicating that the token is a null placement keyword.
            /// </summary>
            public const string Name = "NullPlacement";

            /// <summary>
            /// Gets the identifier for the NULLS FIRST keyword.
            /// </summary>
            public const string NullsFirst = "NullsFirst";

            /// <summary>
            /// Gets the identifier for the NULLS LAST keyword.
            /// </summary>
            public const string NullsLast = "NullsLast";
        }

        private void defineNullPlacement()
        {
            Define(NullPlacement.Name)
                .Add(true, Options()
                    .Add(NullPlacement.NullsFirst, Token(SqlTokenRegistry.NullsFirst))
                    .Add(NullPlacement.NullsLast, Token(SqlTokenRegistry.NullsLast)));
        }

        #endregion

        #region AdditiveExpression

        /// <summary>
        /// Describes the structure of an arithmetic expression adding or substracting two values.
        /// </summary>
        public static class AdditiveExpression
        {
            /// <summary>
            /// Gets the identifier indicating that the expression adds or substracts two values.
            /// </summary>
            public const string Name = "AdditiveExpression";

            /// <summary>
            /// Describes the structure of an arithmetic expression wrapped in parentheses.
            /// </summary>
            public static class Wrapped
            {
                /// <summary>
                /// Gets the identifier indicating that an arithmetic expression is wrapped in parentheses.
                /// </summary>
                public const string Name = "Wrapped";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parethesis";

                /// <summary>
                /// Gets the identifier for the wrapped arithmetic expression.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Describes the structure of a additive expression adding or substracting multiple values.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that the expression is adding or subtracting multiple values.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first operand.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identfier for the operator.
                /// </summary>
                public const string Operator = "operator";

                /// <summary>
                /// Gets the identifier for the rest of the arithmetic expression.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier indicating that the expression is a single value, a multiplication or division.
            /// </summary>
            public const string Single = "single";
        }

        private void defineAdditiveExpression()
        {
            Define(AdditiveExpression.Name)
                .Add(true, Options()
                    .Add(AdditiveExpression.Wrapped.Name, Define()
                        .Add(AdditiveExpression.Wrapped.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(AdditiveExpression.Wrapped.Expression, true, Expression(SqlGrammar.AdditiveExpression.Name))
                        .Add(AdditiveExpression.Wrapped.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                    .Add(AdditiveExpression.Multiple.Name, Define()
                        .Add(AdditiveExpression.Multiple.First, true, Expression(MultiplicitiveExpression.Name))
                        .Add(AdditiveExpression.Multiple.Operator, true, Expression(AdditiveOperator.Name))
                        .Add(AdditiveExpression.Multiple.Remaining, true, Expression(AdditiveExpression.Name)))
                    .Add(AdditiveExpression.Single, Expression(MultiplicitiveExpression.Name)));
        }

        #endregion

        #region AdditiveOperator

        /// <summary>
        /// Describes the structure of an arithmetic operator doing addition or subtraction.
        /// </summary>
        public static class AdditiveOperator
        {
            /// <summary>
            /// Gets the identifier indicating that the operator is addition or subtraction.
            /// </summary>
            public const string Name = "AdditiveOperator";

            /// <summary>
            /// Gets the identifier for the addition operator.
            /// </summary>
            public const string PlusOperator = "plus_operator";

            /// <summary>
            /// Gets the identifier for the subtraction operator.
            /// </summary>
            public const string MinusOperator = "minus_operator";
        }

        private void defineAdditiveOperator()
        {
            Define(AdditiveOperator.Name)
                .Add(true, Options()
                    .Add(AdditiveOperator.PlusOperator, Token(SqlTokenRegistry.PlusOperator))
                    .Add(AdditiveOperator.MinusOperator, Token(SqlTokenRegistry.MinusOperator)));
        }

        #endregion

        #region MultiplicitiveExpression

        /// <summary>
        /// Describes the structure of an arithmetic expression multiplying or dividing values.
        /// </summary>
        public static class MultiplicitiveExpression
        {
            /// <summary>
            /// Gets the identifier indicating that there are values being multiplied or divided.
            /// </summary>
            public const string Name = "MultiplicitiveExpression";

            /// <summary>
            /// Describes the structure of an expression when multiple values are being multiplied and divided together.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that multiple values are being multiplied or divided together.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first operand.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the operator.
                /// </summary>
                public const string Operator = "operator";

                /// <summary>
                /// Gets the identifier for the rest of the arithmetic expression.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier for a single item.
            /// </summary>
            public const string Single = "single";
        }

        private void defineMultiplicitiveExpression()
        {
            Define(MultiplicitiveExpression.Name)
                .Add(true, Options()
                    .Add(MultiplicitiveExpression.Multiple.Name, Define()
                        .Add(MultiplicitiveExpression.Multiple.First, true, Expression(Item.Name))
                        .Add(MultiplicitiveExpression.Multiple.Operator, true, Expression(MultiplicitiveOperator.Name))
                        .Add(MultiplicitiveExpression.Multiple.Remaining, true, Expression(MultiplicitiveExpression.Name)))
                    .Add(MultiplicitiveExpression.Single, Expression(Item.Name)));
        }

        #endregion

        #region MultiplicitiveOperator

        /// <summary>
        /// Describes the structure of a multiplicitive operator.
        /// </summary>
        public static class MultiplicitiveOperator
        {
            /// <summary>
            /// Gets the identifier indicating that the operator is multiplication or division.
            /// </summary>
            public const string Name = "MultiplicitiveOperator";

            /// <summary>
            /// Gets the identifier indicating that the operation is multiplication.
            /// </summary>
            public const string Multiply = "multiply";

            /// <summary>
            /// Gets the identifier indicating that the operation is division.
            /// </summary>
            public const string Divide = "divide";
        }

        private void defineMultiplicitiveOperator()
        {
            Define(MultiplicitiveOperator.Name)
                .Add(true, Options()
                    .Add(MultiplicitiveOperator.Multiply, Token(SqlTokenRegistry.MultiplicationOperator))
                    .Add(MultiplicitiveOperator.Divide, Token(SqlTokenRegistry.DivisionOperator)));
        }

        #endregion

        #region ProjectionList

        /// <summary>
        /// Describes the structure of the projection list.
        /// </summary>
        public static class ProjectionList
        {
            /// <summary>
            /// Gets the name identifying the projection list.
            /// </summary>
            public const string Name = "ProjectionList";

            /// <summary>
            /// Describes the structure of a projection list containing multiple items.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that the projection list contains multiple items.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first projection item.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";
                
                /// <summary>
                /// Gets the identifier for the rest of the projection list.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier indicating that there is one item in the projection list.
            /// </summary>
            public const string Single = "single";
        }

        private void defineProjectionList()
        {
            Define(ProjectionList.Name)
                .Add(true, Options()
                    .Add(ProjectionList.Multiple.Name, Define()
                        .Add(ProjectionList.Multiple.First, true, Expression(ProjectionItem.Name))
                        .Add(ProjectionList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(ProjectionList.Multiple.Remaining, true, Expression(ProjectionList.Name)))
                    .Add(ProjectionList.Single, Expression(ProjectionItem.Name)));
        }

        #endregion

        #region ProjectionItem

        /// <summary>
        /// Describes the structure of the projection item.
        /// </summary>
        public static class ProjectionItem
        {
            /// <summary>
            /// Gets the name identifying the projection item.
            /// </summary>
            public const string Name = "ProjectionItem";

            /// <summary>
            /// Describes the structure of the projection item when it is a star (*).
            /// </summary>
            public static class Star
            {
                /// <summary>
                /// Gets the identifier indicating that the projection item is a star (*).
                /// </summary>
                public const string Name = "Star";

                /// <summary>
                /// Describes the structure of a star projection item (*) that is qualified.
                /// </summary>
                public static class Qualifier
                {
                    /// <summary>
                    /// Gets the identifier indicating that the star projection item (*) is qualified.
                    /// </summary>
                    public const string Name = "Qualifier";

                    /// <summary>
                    /// Gets the identifier for the qualifier.
                    /// </summary>
                    public const string ColumnSource = "column_source";

                    /// <summary>
                    /// Gets the identifier for the dot separator.
                    /// </summary>
                    public const string Dot = "dot";
                }

                /// <summary>
                /// Gets the identifier for the star (*) token.
                /// </summary>
                public const string StarToken = "star";
            }

            /// <summary>
            /// Describes the structure of a projection item when it is a column, function call, SELECT statement or arithmetic expression.
            /// </summary>
            public static class Expression
            {
                /// <summary>
                /// Gets the identifier indicating that the expression is a column, function call, SELECT statement of arithmetic expression.
                /// </summary>
                public const string Name = "Expression";

                /// <summary>
                /// Gets the identifier for the item.
                /// </summary>
                public const string Item = "item";

                /// <summary>
                /// Describes the structure of an alias of a projection item.
                /// </summary>
                public static class AliasExpression
                {
                    /// <summary>
                    /// Gets the identifier indicating that the projection item has an alias.
                    /// </summary>
                    public const string Name = "Alias";

                    /// <summary>
                    /// Gets the identifier for the AS token.
                    /// </summary>
                    public const string AliasIndicator = "alias_indicator";

                    /// <summary>
                    /// Gets the identifier for the alias.
                    /// </summary>
                    public const string Alias = "alias";
                }
            }
        }

        private void defineProjectionItem()
        {
            Define(ProjectionItem.Name)
                .Add(true, Options()
                    .Add(ProjectionItem.Star.Name, Define()
                        .Add(ProjectionItem.Star.Qualifier.Name, false, Define()
                            .Add(ProjectionItem.Star.Qualifier.ColumnSource, true, Expression(MultipartIdentifier.Name))
                            .Add(ProjectionItem.Star.Qualifier.Dot, true, Token(SqlTokenRegistry.Dot)))
                        .Add(ProjectionItem.Star.StarToken, true, Token(SqlTokenRegistry.MultiplicationOperator)))
                    .Add(ProjectionItem.Expression.Name, Define()
                        .Add(ProjectionItem.Expression.Item, true, Expression(ArithmeticItem.Name))
                        .Add(ProjectionItem.Expression.AliasExpression.Name, false, Define()
                            .Add(ProjectionItem.Expression.AliasExpression.AliasIndicator, false, Token(SqlTokenRegistry.AliasIndicator))
                            .Add(ProjectionItem.Expression.AliasExpression.Alias, true, Token(SqlTokenRegistry.Identifier)))));
        }

        #endregion

        #region FromList

        /// <summary>
        /// Describes the structure of the FROM list.
        /// </summary>
        public static class FromList
        {
            /// <summary>
            /// Gets the name identifying the FROM list.
            /// </summary>
            public const string Name = "FromList";

            /// <summary>
            /// Describes the structure of multiple sources in a FROM clause.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that the list contains multiple sources.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first source.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";

                /// <summary>
                /// Gets the identifier for the rest of the sources.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier indicating that the list only has one source.
            /// </summary>
            public const string Single = "single";
        }

        private void defineFromList()
        {
            Define(FromList.Name)
                .Add(true, Options()
                    .Add(FromList.Multiple.Name, Define()
                        .Add(FromList.Multiple.First, true, Expression(Join.Name))
                        .Add(FromList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(FromList.Multiple.Remaining, true, Expression(FromList.Name)))
                    .Add(FromList.Single, Expression(Join.Name)));
        }

        #endregion

        #region JoinItem

        /// <summary>
        /// Describes the structure of the join item.
        /// </summary>
        public static class JoinItem
        {
            /// <summary>
            /// Gets the name identifying the join item.
            /// </summary>
            public const string Name = "JoinItem";

            /// <summary>
            /// Gets the identifier indicating that the join item is a table.
            /// </summary>
            public const string Table = "table";

            /// <summary>
            /// Gets the identifier indicating that the join item is a function call.
            /// </summary>
            public const string FunctionCall = "function_call";

            /// <summary>
            /// Gets the identifier indicating that the join item is a SELECT expression.
            /// </summary>
            public const string SelectExpression = "select_expression";

            /// <summary>
            /// Describes the structure of the join item alias.
            /// </summary>
            public static class AliasExpression
            {
                /// <summary>
                /// Gets the identifier indicating that the join item is aliased.
                /// </summary>
                public const string Name = "Alias";

                /// <summary>
                /// Gets the identifier for the alias indicator (AS).
                /// </summary>
                public const string AliasIndicator = "alias_indicator";

                /// <summary>
                /// Gets the identifier for the alias.
                /// </summary>
                public const string Alias = "alias";
            }
        }

        private void defineJoinItem()
        {
            Define(JoinItem.Name)
                .Add(true, Options()
                    .Add(JoinItem.FunctionCall, Expression(FunctionCall.Name))
                    .Add(JoinItem.Table, Expression(MultipartIdentifier.Name))
                    .Add(JoinItem.SelectExpression, Expression(SelectExpression.Name)))
                .Add(JoinItem.AliasExpression.Name, false, Define()
                    .Add(JoinItem.AliasExpression.AliasIndicator, false, Token(SqlTokenRegistry.AliasIndicator))
                    .Add(JoinItem.AliasExpression.Alias, true, Token(SqlTokenRegistry.Identifier)));
        }

        #endregion

        #region FunctionCall

        /// <summary>
        /// Describes the structure of the function call.
        /// </summary>
        public static class FunctionCall
        {
            /// <summary>
            /// Gets the name identifying the function call.
            /// </summary>
            public const string Name = "FunctionCall";

            /// <summary>
            /// Gets the identifier for the function name.
            /// </summary>
            public const string FunctionName = "function_name";

            /// <summary>
            /// Gets the identifier for the left parenthesis.
            /// </summary>
            public const string LeftParethesis = "left_parenthesis";

            /// <summary>
            /// Gets the identifier for the function arguments.
            /// </summary>
            public const string Arguments = "arguments";

            /// <summary>
            /// Gets the identifier for the right parenthesis.
            /// </summary>
            public const string RightParenthesis = "right_parenthesis";
        }

        private void defineFunctionCall()
        {
            Define(FunctionCall.Name)
                .Add(FunctionCall.FunctionName, true, Expression(MultipartIdentifier.Name))
                .Add(FunctionCall.LeftParethesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                .Add(FunctionCall.Arguments, false, Expression(ValueList.Name))
                .Add(FunctionCall.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis));
        }

        #endregion

        #region Join

        /// <summary>
        /// Describes the structure of the join.
        /// </summary>
        public static class Join
        {
            /// <summary>
            /// Gets the name identifying the join.
            /// </summary>
            public const string Name = "Join";

            /// <summary>
            /// Describes the structure of a join surrounded in parenthesis.
            /// </summary>
            public static class Wrapped
            {
                /// <summary>
                /// Gets the identifier indicating that the join is wrapped in parenthesis.
                /// </summary>
                public const string Name = "Wrapped";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the wrapped join.
                /// </summary>
                public const string Join = "join";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Describes the structure of a join item potentially joined to another item.
            /// </summary>
            public static class Joined
            {
                /// <summary>
                /// Gets the identifier indicating that we have a join item and potentially a join.
                /// </summary>
                public const string Name = "Joined";

                /// <summary>
                /// Gets the identifier for the item being joined.
                /// </summary>
                public const string JoinItem = "join_item";

                /// <summary>
                /// Gets the identifier for the rest of the join statement.
                /// </summary>
                public const string JoinPrime = "join_prime";
            }
        }

        private void defineJoin()
        {
            Define(Join.Name)
                .Add(true, Options()
                    .Add(Join.Wrapped.Name, Define()
                        .Add(Join.Wrapped.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(Join.Wrapped.Join, true, Expression(Join.Name))
                        .Add(Join.Wrapped.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                    .Add(Join.Joined.Name, Define()
                        .Add(Join.Joined.JoinItem, true, Expression(JoinItem.Name))
                        .Add(Join.Joined.JoinPrime, true, Expression(JoinPrime.Name))));
        }

        #endregion

        #region JoinPrime

        /// <summary>
        /// Describes the structure of the join prime expression.
        /// </summary>
        public static class JoinPrime
        {
            /// <summary>
            /// Gets the name identifying the join prime expression.
            /// </summary>
            public const string Name = "JoinPrime";

            /// <summary>
            /// Describes the structure of a join with a filter.
            /// </summary>
            public static class Filtered
            {
                /// <summary>
                /// Gets the identifier indicating that there is another join item.
                /// </summary>
                public const string Name = "Joined";

                /// <summary>
                /// Gets the identifier for the next join type.
                /// </summary>
                public const string JoinType = "join_type";

                /// <summary>
                /// Gets the identifier for the next join item.
                /// </summary>
                public const string JoinItem = "join_item";

                /// <summary>
                /// Describes the structure of the join filter.
                /// </summary>
                public static class On
                {
                    /// <summary>
                    /// Gets the identifier indicating that there is a ON clause.
                    /// </summary>
                    public const string Name = "On";

                    /// <summary>
                    /// Gets the identifier for the ON keyword.
                    /// </summary>
                    public const string OnKeyword = "on";

                    /// <summary>
                    /// Gets the identifier for the filter list.
                    /// </summary>
                    public const string FilterList = "filter_list";
                }

                /// <summary>
                /// Gets the identifier for the next join in the series.
                /// </summary>
                public const string JoinPrime = "join_prime";
            }

            /// <summary>
            /// Describes the structure of a cross join.
            /// </summary>
            public static class Cross
            {
                /// <summary>
                /// Gets the identifier indicating that the join is a cross join.
                /// </summary>
                public const string Name = "Cross";

                /// <summary>
                /// Gets the identifier for the next join type.
                /// </summary>
                public const string JoinType = "join_type";

                /// <summary>
                /// Gets the identifier for the next join item.
                /// </summary>
                public const string JoinItem = "join_item";

                /// <summary>
                /// Gets the identifier for the next join in the series.
                /// </summary>
                public const string JoinPrime = "join_prime";
            }

            /// <summary>
            /// Gets the identifier indicating that there are no more joins.
            /// </summary>
            public const string Empty = "empty";
        }

        private void defineJoinPrime()
        {
            Define(JoinPrime.Name)
                .Add(true, Options()
                    .Add(JoinPrime.Filtered.Name, Define()
                        .Add(JoinPrime.Filtered.JoinType, true, Expression(FilteredJoinType.Name))
                        .Add(JoinPrime.Filtered.JoinItem, true, Expression(JoinItem.Name))
                        .Add(JoinPrime.Filtered.On.Name, false, Define()
                            .Add(JoinPrime.Filtered.On.OnKeyword, true, Token(SqlTokenRegistry.On))
                            .Add(JoinPrime.Filtered.On.FilterList, true, Expression(FilterList.Name)))
                        .Add(JoinPrime.Filtered.JoinPrime, true, Expression(JoinPrime.Name)))
                    .Add(JoinPrime.Cross.Name, Define()
                        .Add(JoinPrime.Cross.JoinType, true, Token(SqlTokenRegistry.CrossJoin))
                        .Add(JoinPrime.Cross.JoinItem, true, Expression(JoinItem.Name))
                        .Add(JoinPrime.Cross.JoinPrime, true, Expression(JoinPrime.Name)))
                    .Add(JoinPrime.Empty, Define()));
        }

        #endregion

        #region FilteredJoinType

        /// <summary>
        /// Describes the options for a join type.
        /// </summary>
        public static class FilteredJoinType
        {
            /// <summary>
            /// Gets the identifier indicating that the token is a join type.
            /// </summary>
            public const string Name = "FilteredJoinType";

            /// <summary>
            /// Gets the identifier for the INNER JOIN keyword.
            /// </summary>
            public const string InnerJoin = "InnerJoin";

            /// <summary>
            /// Gets the identifier for the LEFT OUTER JOIN keyword.
            /// </summary>
            public const string LeftOuterJoin = "LeftOuterJoin";

            /// <summary>
            /// Gets the identifier for the RIGHT OUTER JOIN keyword.
            /// </summary>
            public const string RightOuterJoin = "RightOuterJoin";

            /// <summary>
            /// Gets the identifier for the FULL OUTER JOIN keyword.
            /// </summary>
            public const string FullOuterJoin = "FullOuterJoin";
        }

        private void defineFilteredJoinType()
        {
            Define(FilteredJoinType.Name)
                .Add(true, Options()
                    .Add(FilteredJoinType.InnerJoin, Token(SqlTokenRegistry.InnerJoin))
                    .Add(FilteredJoinType.LeftOuterJoin, Token(SqlTokenRegistry.LeftOuterJoin))
                    .Add(FilteredJoinType.RightOuterJoin, Token(SqlTokenRegistry.RightOuterJoin))
                    .Add(FilteredJoinType.FullOuterJoin, Token(SqlTokenRegistry.FullOuterJoin)));
        }

        #endregion

        #region FilterList

        /// <summary>
        /// Describes the structure of the filter list.
        /// </summary>
        public static class FilterList
        {
            /// <summary>
            /// Gets the name identifying the filter list.
            /// </summary>
            public const string Name = "FilterList";

            /// <summary>
            /// Describes the structure of a filter list wrapped in parentheses.
            /// </summary>
            public static class Wrapped
            {
                /// <summary>
                /// Gets the identifier indicating that the filter list is wrapped in parentheses.
                /// </summary>
                public const string Name = "Wrapped";

                /// <summary>
                /// Gets the identifier for the NOT keyword.
                /// </summary>
                public const string Not = "not";

                /// <summary>
                /// Gets the identifier for a left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the filter list.
                /// </summary>
                public const string FilterList = "filter_list";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Gets the identifier indicating that there are multiple filters in the list.
            /// </summary>
            public const string Multiple = "Multiple";
        }

        private void defineFilterList()
        {
            Define(FilterList.Name)
                .Add(true, Options()
                    .Add(FilterList.Multiple, Expression(OrFilter.Name))
                    .Add(FilterList.Wrapped.Name, Define()
                        .Add(FilterList.Wrapped.Not, false, Token(SqlTokenRegistry.Not))
                        .Add(FilterList.Wrapped.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(FilterList.Wrapped.FilterList, true, Expression(FilterList.Name))
                        .Add(FilterList.Wrapped.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis))));
        }

        #endregion

        #region Filter

        /// <summary>
        /// Describes the structure of the filter.
        /// </summary>
        public static class Filter
        {
            /// <summary>
            /// Gets the name identifying the filter.
            /// </summary>
            public const string Name = "Filter";

            /// <summary>
            /// Describes the structure of a filter wrapped in parenthesis.
            /// </summary>
            public static class Wrapped
            {
                /// <summary>
                /// Gets the indentifier indicating that the filter is wrapped in parenthesis.
                /// </summary>
                public const string Name = "Wrapped";

                /// <summary>
                /// Gets the identifier for the left parenthesis token.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the wrapped filter.
                /// </summary>
                public const string Filter = "filter";

                /// <summary>
                /// Gets the identifier for the right parenthesis token.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Describes the structure of a filter that is negated.
            /// </summary>
            public static class Not
            {
                /// <summary>
                /// Gets the identifier indicating that a filter is negated.
                /// </summary>
                public const string Name = "Not";

                /// <summary>
                /// Gets the identifier for the NOT keyword.
                /// </summary>
                public const string NotKeyword = "not";

                /// <summary>
                /// Gets the identifier for the negated filter.
                /// </summary>
                public const string Filter = "filter";
            }

            /// <summary>
            /// Describes the structure of filter that compares the order of two items.
            /// </summary>
            public static class Order
            {
                /// <summary>
                /// Gets the identifier indicating that the filter is an order filter.
                /// </summary>
                public const string Name = "Order";

                /// <summary>
                /// Gets the identifier for the value on the left.
                /// </summary>
                public const string Left = "left";

                /// <summary>
                /// Gets the identifier for the comparison operator.
                /// </summary>
                public const string ComparisonOperator = "comparison_operator";

                /// <summary>
                /// Gets the identifier for the value on the right.
                /// </summary>
                public const string Right = "right";
            }

            /// <summary>
            /// Describes the structure of a filter checking that a value falls within a range.
            /// </summary>
            public static class Between
            {
                /// <summary>
                /// The identifier indicating that the filter is a BETWEEN filter.
                /// </summary>
                public const string Name = "Between";

                /// <summary>
                /// Gets the identifier for the value being checked.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the identifier for whether or not to negate the filter.
                /// </summary>
                public const string Not = "not";

                /// <summary>
                /// Gets the identifier for the BETWEEN keyword.
                /// </summary>
                public const string BetweenKeyword = "between";

                /// <summary>
                /// Gets the identifier for the lower bound value.
                /// </summary>
                public const string LowerBound = "lower_bound";

                /// <summary>
                /// Gets the identifier for the AND keyword.
                /// </summary>
                public const string And = "between_and";

                /// <summary>
                /// Gets the identifier for the upper bound value.
                /// </summary>
                public const string UpperBound = "upper_bound";
            }

            /// <summary>
            /// Describes the structure of a filter doing a string comparison.
            /// </summary>
            public static class Like
            {
                /// <summary>
                /// Gets the identifier indicating whether the filter is doing a string comparison.
                /// </summary>
                public const string Name = "Like";
                
                /// <summary>
                /// Gets the identifier for the expression being compared.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the identifier indicating whether to negate the results of the comparison.
                /// </summary>
                public const string NotKeyword = "not";

                /// <summary>
                /// Gets the identifier for the LIKE keyword.
                /// </summary>
                public const string LikeKeyword = "like";

                /// <summary>
                /// Gets the identifier for string literal being compared to.
                /// </summary>
                public const string Value = "value";
            }

            /// <summary>
            /// Describes the structure of a filter checking whether a value is null.
            /// </summary>
            public static class Is
            {
                /// <summary>
                /// Gets the identifier indicating that the filter is checking whether a value is null or not.
                /// </summary>
                public const string Name = "Is";

                /// <summary>
                /// Gets the identifier for value being compared.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the identifier for the IS keyword.
                /// </summary>
                public const string IsKeyword = "is";

                /// <summary>
                /// Gets the identifier for the NOT keyword.
                /// </summary>
                public const string NotKeyword = "not";

                /// <summary>
                /// Gets the identifier for the NULL keyword.
                /// </summary>
                public const string NullKeyword = "null";
            }

            /// <summary>
            /// Describes the structure of a filter checking whether a value exists in a list of values.
            /// </summary>
            public static class In
            {
                /// <summary>
                /// Gets the identifier indicating that the filter is checking whether a value exists in a list of values.
                /// </summary>
                public const string Name = "In";

                /// <summary>
                /// Gets the identifier for the value being compared.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the identifier for the NOT keyword.
                /// </summary>
                public const string NotKeyword = "not";

                /// <summary>
                /// Gets the identifier for the IN keyword.
                /// </summary>
                public const string InKeyword = "in";

                /// <summary>
                /// Describes the structure of a values list.
                /// </summary>
                public static class Values
                {
                    /// <summary>
                    /// Gets the identifier indicating that the source is a value list.
                    /// </summary>
                    public const string Name = "Values";

                    /// <summary>
                    /// Gets the identifier for the left parenthesis token.
                    /// </summary>
                    public const string LeftParenthesis = "left_parenthesis";

                    /// <summary>
                    /// Gets the identifier indicating that the values come from a list of values.
                    /// </summary>
                    public const string ValueList = "value_list";

                    /// <summary>
                    /// Gets the identifier for the right parenthesis.
                    /// </summary>
                    public const string RightParenthesis = "right_parenthesis";
                }

                /// <summary>
                /// Describes the structure a select source.
                /// </summary>
                public static class Select
                {
                    /// <summary>
                    /// Gets the identifier indicating that the source is a SELECT expression.
                    /// </summary>
                    public const string Name = "Select";

                    /// <summary>
                    /// Gets the identifier for the left parenthesis token.
                    /// </summary>
                    public const string LeftParenthesis = "left_parenthesis";

                    /// <summary>
                    /// Gets the identifier indicating that the values come from a SELECT expression.
                    /// </summary>
                    public const string SelectExpression = "select_expression";

                    /// <summary>
                    /// Gets the identifier for the right parenthesis.
                    /// </summary>
                    public const string RightParenthesis = "right_parenthesis";
                }

                /// <summary>
                /// Gets the identifier indicating that the values come from a function call.
                /// </summary>
                public const string FunctionCall = "function_call";
            }

            /// <summary>
            /// Describes the structure of an Exists filter.
            /// </summary>
            public static class Exists
            {
                /// <summary>
                /// Gets the identifier indicating that the filter is an exists filter.
                /// </summary>
                public const string Name = "Exists";

                /// <summary>
                /// Gets the identifier for the EXISTS keyword.
                /// </summary>
                public const string ExistsKeyword = "exists";

                /// <summary>
                /// Gets the identfier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the SELECT expression.
                /// </summary>
                public const string SelectExpression = "select_expression";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Describes the structure of a existential or universal quantifier.
            /// </summary>
            public static class Quantify
            {
                /// <summary>
                /// Gets the identifier indicating that the filter is a quantifier.
                /// </summary>
                public const string Name = "Quantify";

                /// <summary>
                /// Gets the identifier for the value being compared.
                /// </summary>
                public const string Expression = "expression";

                /// <summary>
                /// Gets the identifier for the comparison operator.
                /// </summary>
                public const string ComparisonOperator = "comparison_operator";

                /// <summary>
                /// Gets the identifier for the existential or universal quantifier.
                /// </summary>
                public const string Quantifier = "quantifier";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the select expression.
                /// </summary>
                public const string SelectExpression = "select_expression";

                /// <summary>
                /// Gets the identifier for the value list.
                /// </summary>
                public const string ValueList = "value_list";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }
        }

        private void defineFilter()
        {
            Define(Filter.Name)
                .Add(true, Options()
                    .Add(Filter.Wrapped.Name, Define()
                        .Add(Filter.Wrapped.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(Filter.Wrapped.Filter, true, Expression(Filter.Name))
                        .Add(Filter.Wrapped.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                    .Add(Filter.Not.Name, Define()
                        .Add(Filter.Not.NotKeyword, true, Token(SqlTokenRegistry.Not))
                        .Add(Filter.Not.Filter, true, Expression(Filter.Name)))
                    .Add(Filter.Quantify.Name, Define()
                        .Add(Filter.Quantify.Expression, true, Expression(SqlGrammar.ArithmeticItem.Name))
                        .Add(Filter.Quantify.ComparisonOperator, true, Expression(SqlGrammar.ComparisonOperator.Name))
                        .Add(Filter.Quantify.Quantifier, true, Expression(SqlGrammar.Quantifier.Name))
                        .Add(Filter.Quantify.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(true, Options()
                            .Add(Filter.Quantify.SelectExpression, Expression(SelectExpression.Name))
                            .Add(Filter.Quantify.ValueList, Expression(ValueList.Name)))
                        .Add(Filter.Quantify.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                    .Add(Filter.Order.Name, Define()
                        .Add(Filter.Order.Left, true, Expression(ArithmeticItem.Name))
                        .Add(Filter.Order.ComparisonOperator, true, Expression(ComparisonOperator.Name))
                        .Add(Filter.Order.Right, true, Expression(ArithmeticItem.Name)))
                    .Add(Filter.Between.Name, Define()
                        .Add(Filter.Between.Expression, true, Expression(ArithmeticItem.Name))
                        .Add(Filter.Between.Not, false, Token(SqlTokenRegistry.Not))
                        .Add(Filter.Between.BetweenKeyword, true, Token(SqlTokenRegistry.Between))
                        .Add(Filter.Between.LowerBound, true, Expression(ArithmeticItem.Name))
                        .Add(Filter.Between.And, true, Token(SqlTokenRegistry.And))
                        .Add(Filter.Between.UpperBound, true, Expression(ArithmeticItem.Name)))
                    .Add(Filter.Like.Name, Define()
                        .Add(Filter.Like.Expression, true, Expression(ArithmeticItem.Name))
                        .Add(Filter.Like.NotKeyword, false, Token(SqlTokenRegistry.Not))
                        .Add(Filter.Like.LikeKeyword, true, Token(SqlTokenRegistry.Like))
                        .Add(Filter.Like.Value, true, Token(SqlTokenRegistry.String)))
                    .Add(Filter.Is.Name, Define()
                        .Add(Filter.Is.Expression, true, Expression(ArithmeticItem.Name))
                        .Add(Filter.Is.IsKeyword, true, Token(SqlTokenRegistry.Is))
                        .Add(Filter.Is.NotKeyword, false, Token(SqlTokenRegistry.Not))
                        .Add(Filter.Is.NullKeyword, true, Token(SqlTokenRegistry.Null)))
                    .Add(Filter.In.Name, Define()
                        .Add(Filter.In.Expression, true, Expression(ArithmeticItem.Name))
                        .Add(Filter.In.NotKeyword, false, Token(SqlTokenRegistry.Not))
                        .Add(Filter.In.InKeyword, true, Token(SqlTokenRegistry.In))
                        .Add(true, Options()
                            .Add(Filter.In.Values.Name, Define()
                                .Add(Filter.In.Values.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                                .Add(Filter.In.Values.ValueList, false, Expression(ValueList.Name))
                                .Add(Filter.In.Values.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                            .Add(Filter.In.Select.Name, Define()
                                .Add(Filter.In.Select.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                                .Add(Filter.In.Select.SelectExpression, true, Expression(SelectExpression.Name))
                                .Add(Filter.In.Select.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                            .Add(Filter.In.FunctionCall, Expression(FunctionCall.Name))))
                    .Add(Filter.Exists.Name, Define()
                        .Add(Filter.Exists.ExistsKeyword, true, Token(SqlTokenRegistry.Exists))
                        .Add(Filter.Exists.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(Filter.Exists.SelectExpression, true, Expression(SqlGrammar.SelectExpression.Name))
                        .Add(Filter.Exists.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis))));
        }

        #endregion

        #region ComparisonOperator

        /// <summary>
        /// Describes the structure of a comparison operator.
        /// </summary>
        public static class ComparisonOperator
        {
            /// <summary>
            /// Gets the identifier indicating that the token is a comparison operator.
            /// </summary>
            public const string Name = "ComparisonOperator";

            /// <summary>
            /// Gets the identifier for the equality operator.
            /// </summary>
            public const string EqualTo = "equal_to";

            /// <summary>
            /// Gets the identifier for the inequality operator.
            /// </summary>
            public const string NotEqualTo = "not_equal_to";

            /// <summary>
            /// Gets the identifier for the less than or equal to operator.
            /// </summary>
            public const string LessThanEqualTo = "less_than_equal_to";

            /// <summary>
            /// Gets the identifier for the greater than or equal to operator.
            /// </summary>
            public const string GreaterThanEqualTo = "greater_than_equal_to";

            /// <summary>
            /// Gets the identifier for the less than operator.
            /// </summary>
            public const string LessThan = "less_than";

            /// <summary>
            /// Gets the identifier for the greater than operator.
            /// </summary>
            public const string GreaterThan = "greater_than";
        }

        private void defineComparisonOperator()
        {
            Define(ComparisonOperator.Name)
                .Add(true, Options()
                    .Add(ComparisonOperator.EqualTo, Token(SqlTokenRegistry.EqualTo))
                    .Add(ComparisonOperator.NotEqualTo, Token(SqlTokenRegistry.NotEqualTo))
                    .Add(ComparisonOperator.LessThanEqualTo, Token(SqlTokenRegistry.LessThanEqualTo))
                    .Add(ComparisonOperator.GreaterThanEqualTo, Token(SqlTokenRegistry.GreaterThanEqualTo))
                    .Add(ComparisonOperator.LessThan, Token(SqlTokenRegistry.LessThan))
                    .Add(ComparisonOperator.GreaterThan, Token(SqlTokenRegistry.GreaterThan)));
        }

        #endregion

        #region Quantifier

        /// <summary>
        /// Describes the structure of a quantifier.
        /// </summary>
        public static class Quantifier
        {
            /// <summary>
            /// Gets the identifier indicating that the current token is a quantifier.
            /// </summary>
            public const string Name = "Quantifier";

            /// <summary>
            /// Gets the identifier for the ALL quantifier.
            /// </summary>
            public const string All = "all";

            /// <summary>
            /// Gets the identifier for the ANY quantifier.
            /// </summary>
            public const string Any = "any";

            /// <summary>
            /// Gets the identifier for the SOME quantifier.
            /// </summary>
            public const string Some = "some";
        }

        private void defineQuantifier()
        {
            Define(Quantifier.Name)
                .Add(true, Options()
                    .Add(Quantifier.All, Token(SqlTokenRegistry.All))
                    .Add(Quantifier.Any, Token(SqlTokenRegistry.Any))
                    .Add(Quantifier.Some, Token(SqlTokenRegistry.Some)));
        }

        #endregion

        #region OrFilter

        /// <summary>
        /// Describes the structure of two filters OR'd together.
        /// </summary>
        public static class OrFilter
        {
            /// <summary>
            /// Gets the identifier indicating that two filters are OR'd together.
            /// </summary>
            public const string Name = "OrFilter";

            /// <summary>
            /// Gets the identifier indicating that two filter are OR'd together.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that filters are OR'd together.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first filter.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the OR keyword.
                /// </summary>
                public const string Or = "or";

                /// <summary>
                /// Gets the identifier for the rest of the filters.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier for a single filter.
            /// </summary>
            public const string Single = "single";
        }

        private void defineOrConjunction()
        {
            Define(OrFilter.Name)
                .Add(true, Options()
                    .Add(OrFilter.Multiple.Name, Define()
                        .Add(OrFilter.Multiple.First, true, Expression(AndFilter.Name))
                        .Add(OrFilter.Multiple.Or, true, Token(SqlTokenRegistry.Or))
                        .Add(OrFilter.Multiple.Remaining, true, Expression(OrFilter.Name)))
                    .Add(OrFilter.Single, Expression(AndFilter.Name)));
        }

        #endregion

        #region AndFilter

        /// <summary>
        /// Describes the structure of two filters AND'd together.
        /// </summary>
        public static class AndFilter
        {
            /// <summary>
            /// Gets the identifier indicating that two filters are AND'd together.
            /// </summary>
            public const string Name = "AndFilter";

            /// <summary>
            /// Gets the identifier indicating that two filter are AND'd together.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that filters are AND'd together.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first filter.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the AND keyword.
                /// </summary>
                public const string And = "and";

                /// <summary>
                /// Gets the identifier for the rest of the filters.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier for a single filter.
            /// </summary>
            public const string Single = "single";
        }

        private void defineAndConjunction()
        {
            Define(AndFilter.Name)
                .Add(true, Options()
                    .Add(AndFilter.Multiple.Name, Define()
                        .Add(AndFilter.Multiple.First, true, Expression(Filter.Name))
                        .Add(AndFilter.Multiple.And, true, Token(SqlTokenRegistry.And))
                        .Add(AndFilter.Multiple.Remaining, true, Expression(AndFilter.Name)))
                    .Add(AndFilter.Single, Expression(Filter.Name)));
        }

        #endregion

        #region ValueList

        /// <summary>
        /// Describes the structure of the value list.
        /// </summary>
        public static class ValueList
        {
            /// <summary>
            /// Gets the name identifying the value list.
            /// </summary>
            public const string Name = "ValueList";

            /// <summary>
            /// Describes the structure of a value list containing multiple items.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that the value list has more than one item.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first value.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";

                /// <summary>
                /// Gets the identifier for the rest of the values.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier indicating that there is a single value.
            /// </summary>
            public const string Single = "single";
        }

        private void defineValueList()
        {
            Define(ValueList.Name)
                .Add(true, Options()
                    .Add(ValueList.Multiple.Name, Define()
                        .Add(ValueList.Multiple.First, true, Expression(ArithmeticItem.Name))
                        .Add(ValueList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(ValueList.Multiple.Remaining, true, Expression(ValueList.Name)))
                    .Add(ValueList.Single, Expression(ArithmeticItem.Name)));
        }

        #endregion

        #region GroupByList

        /// <summary>
        /// Describes the structure of the GROUP BY list.
        /// </summary>
        public static class GroupByList
        {
            /// <summary>
            /// Gets the name identifying the GROUP BY list.
            /// </summary>
            public const string Name = "GroupByList";

            /// <summary>
            /// Describes the structure of multiple GROUP BY items.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that there are multiple GROUP BY items.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first GROUP BY item.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";

                /// <summary>
                /// Gets the identifier for the rest of the GROUP BY items.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier for a single GROUP BY item.
            /// </summary>
            public const string Single = "single";
        }

        private void defineGroupByList()
        {
            Define(GroupByList.Name)
                .Add(true, Options()
                    .Add(SqlGrammar.GroupByList.Multiple.Name, Define()
                        .Add(SqlGrammar.GroupByList.Multiple.First, true, Expression(ArithmeticItem.Name))
                        .Add(SqlGrammar.GroupByList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(SqlGrammar.GroupByList.Multiple.Remaining, true, Expression(GroupByList.Name)))
                    .Add(SqlGrammar.GroupByList.Single, Expression(ArithmeticItem.Name)));
        }

        #endregion

        #region ArithmeticItem

        /// <summary>
        /// Describes the structure of an item that can be an arithmetic expression.
        /// </summary>
        public static class ArithmeticItem
        {
            /// <summary>
            /// The identifier indicating that the item can be an arithmetic expression.
            /// </summary>
            public const string Name = "ArithmeticItem";

            /// <summary>
            /// Gets the identifier indicating that the item is an arithmetic expression.
            /// </summary>
            public const string ArithmeticExpression = "arithmetic_expression";
        }

        private void defineArithmeticItem()
        {
            Define(ArithmeticItem.Name)
                .Add(ArithmeticItem.ArithmeticExpression, true, Expression(AdditiveExpression.Name));
        }

        #endregion

        #region Item

        /// <summary>
        /// Describes the structure of the item.
        /// </summary>
        public static class Item
        {
            /// <summary>
            /// Gets the name identifying the item.
            /// </summary>
            public const string Name = "Item";

            /// <summary>
            /// Gets the identifier indicating that the item is a column.
            /// </summary>
            public const string Column = "column";

            /// <summary>
            /// Gets the identifier indicating that the item is a function call.
            /// </summary>
            public const string FunctionCall = "function_call";

            /// <summary>
            /// Gets the identifier indicating that the item is a select statement.
            /// </summary>
            public static class Select
            {
                /// <summary>
                /// Gets the identifier indicating that the item is a SELECT statement.
                /// </summary>
                public const string Name = "SelectStatement";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the SELECT statement.
                /// </summary>
                public const string SelectStatement = "select_statement";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Gets the identifier indicating that the item is a number.
            /// </summary>
            public const string Number = "number";

            /// <summary>
            /// Gets the identifier indicating that the item is a string.
            /// </summary>
            public const string String = "string";

            /// <summary>
            /// Gets the identifier indicating that the item is a null.
            /// </summary>
            public const string Null = "null";
        }

        private void defineItem()
        {
            Define(Item.Name)
                .Add(true, Options()
                    .Add(Item.Number, Token(SqlTokenRegistry.Number))
                    .Add(Item.String, Token(SqlTokenRegistry.String))
                    .Add(Item.Null, Token(SqlTokenRegistry.Null))
                    .Add(Item.FunctionCall, Expression(FunctionCall.Name))
                    .Add(Item.Column, Expression(MultipartIdentifier.Name))
                    .Add(Item.Select.Name, Define()
                        .Add(Item.Select.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(Item.Select.SelectStatement, true, Expression(SelectStatement.Name))
                        .Add(Item.Select.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis))));
        }

        #endregion

        #region InsertStatement

        /// <summary>
        /// Describes the structure of the INSERT statement.
        /// </summary>
        public static class InsertStatement
        {
            /// <summary>
            /// Gets the name identifying the INSERT statement.
            /// </summary>
            public const string Name = "InsertStatement";

            /// <summary>
            /// Gets the identifier for the INSERT keyword.
            /// </summary>
            public const string InsertKeyword = "insert";

            /// <summary>
            /// Gets the identifier for the INTO keyword.
            /// </summary>
            public const string IntoKeyword = "into";

            /// <summary>
            /// Gets the identifier for the table name.
            /// </summary>
            public const string Table = "table";

            /// <summary>
            /// Describes the structure of the table alias.
            /// </summary>
            public static class AliasExpression
            {
                /// <summary>
                /// Gets the identifier indicating that the table has an alias.
                /// </summary>
                public const string Name = "AliasExpression";

                /// <summary>
                /// Gets the identifier for the AS keyword.
                /// </summary>
                public const string AliasIndicator = "alias_indicator";

                /// <summary>
                /// Gets the identiifier for the alias.
                /// </summary>
                public const string Alias = "alias";
            }

            /// <summary>
            /// Describes the structure of the columns list.
            /// </summary>
            public static class Columns
            {
                /// <summary>
                /// Gets the identifier indicating that there is a column list.
                /// </summary>
                public const string Name = "ColumnList";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the column list.
                /// </summary>
                public const string ColumnList = "column_list";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Describes the structure of the values list.
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// Gets the identifier indicating that a value list is used.
                /// </summary>
                public const string Name = "Values";

                /// <summary>
                /// Gets the identifier for the VALUES keyword.
                /// </summary>
                public const string ValuesKeyword = "values";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the value list.
                /// </summary>
                public const string ValueList = "value_list";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }

            /// <summary>
            /// Describes the structure of the SELECT expression generating the values.
            /// </summary>
            public static class Select
            {
                /// <summary>
                /// Gets the identifier indicating that the values come from a SELECT statement.
                /// </summary>
                public const string Name = "SelectExpression";

                /// <summary>
                /// Gets the identifier for the left parenthesis.
                /// </summary>
                public const string LeftParenthesis = "left_parenthesis";

                /// <summary>
                /// Gets the identifier for the select expression.
                /// </summary>
                public const string SelectExpression = "select_expression";

                /// <summary>
                /// Gets the identifier for the right parenthesis.
                /// </summary>
                public const string RightParenthesis = "right_parenthesis";
            }
        }

        private void defineInsertStatement()
        {
            Define(InsertStatement.Name)
                .Add(SqlGrammar.InsertStatement.InsertKeyword, true, Token(SqlTokenRegistry.Insert))
                .Add(SqlGrammar.InsertStatement.IntoKeyword, false, Token(SqlTokenRegistry.Into))
                .Add(SqlGrammar.InsertStatement.Table, true, Expression(MultipartIdentifier.Name))
                .Add(SqlGrammar.InsertStatement.AliasExpression.Name, false, Define()
                    .Add(SqlGrammar.InsertStatement.AliasExpression.AliasIndicator, false, Token(SqlTokenRegistry.AliasIndicator))
                    .Add(SqlGrammar.InsertStatement.AliasExpression.Alias, true, Token(SqlTokenRegistry.Identifier)))
                .Add(SqlGrammar.InsertStatement.Columns.Name, false, Define()
                    .Add(SqlGrammar.InsertStatement.Columns.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                    .Add(SqlGrammar.InsertStatement.Columns.ColumnList, true, Expression(ColumnList.Name))
                    .Add(SqlGrammar.InsertStatement.Columns.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                .Add(true, Options()
                    .Add(SqlGrammar.InsertStatement.Values.Name, Define()
                        .Add(SqlGrammar.InsertStatement.Values.ValuesKeyword, true, Token(SqlTokenRegistry.Values))
                        .Add(SqlGrammar.InsertStatement.Values.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(SqlGrammar.InsertStatement.Values.ValueList, false, Expression(ValueList.Name))
                        .Add(SqlGrammar.InsertStatement.Values.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis)))
                    .Add(SqlGrammar.InsertStatement.Select.Name, Define()
                        .Add(SqlGrammar.InsertStatement.Select.LeftParenthesis, true, Token(SqlTokenRegistry.LeftParenthesis))
                        .Add(SqlGrammar.InsertStatement.Select.SelectExpression, true, Expression(SelectExpression.Name))
                        .Add(SqlGrammar.InsertStatement.Select.RightParenthesis, true, Token(SqlTokenRegistry.RightParenthesis))));

        }

        #endregion

        #region ColumnList

        /// <summary>
        /// Describes the structure of the column list.
        /// </summary>
        public static class ColumnList
        {
            /// <summary>
            /// Gets the name identifying the column list.
            /// </summary>
            public const string Name = "ColumnList";

            /// <summary>
            /// Describes the structure of a column list with multiple columns.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier used to indicate that multiple columns exist.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first column.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";

                /// <summary>
                /// Gets the identifier for the remaining columns.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier used to indicate that a single column exists.
            /// </summary>
            public const string Single = "single";
        }

        private void defineColumnList()
        {
            Define(ColumnList.Name)
                .Add(true, Options()
                    .Add(ColumnList.Multiple.Name, Define()
                        .Add(ColumnList.Multiple.First, true, Expression(MultipartIdentifier.Name))
                        .Add(ColumnList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(ColumnList.Multiple.Remaining, true, Expression(ColumnList.Name)))
                    .Add(ColumnList.Single, Expression(MultipartIdentifier.Name)));
        }

        #endregion

        #region UpdateStatement

        /// <summary>
        /// Describes the structure of the UPDATE statement.
        /// </summary>
        public static class UpdateStatement
        {
            /// <summary>
            /// Gets the name identifying the UPDATE statement.
            /// </summary>
            public const string Name = "UpdateStatement";

            /// <summary>
            /// Gets the identifier for the UPDATE keyword.
            /// </summary>
            public const string UpdateKeyword = "update";

            /// <summary>
            /// Gets the identifier for the table.
            /// </summary>
            public const string Table = "table";

            /// <summary>
            /// Describes the structure of the table alias.
            /// </summary>
            public static class AliasExpression
            {
                /// <summary>
                /// Gets the identifier indicating that the table is aliased.
                /// </summary>
                public const string Name = "AliasExpression";

                /// <summary>
                /// Gets the identifier for the AS keyword.
                /// </summary>
                public const string AliasIndicator = "alias_indicator";

                /// <summary>
                /// Gets the identifier for the alias.
                /// </summary>
                public const string Alias = "alias";
            }

            /// <summary>
            /// Gets the identifier for the SET keyword.
            /// </summary>
            public const string SetKeyword = "set";

            /// <summary>
            /// Gets the identifier for the setter list.
            /// </summary>
            public const string SetterList = "setter_list";

            /// <summary>
            /// Describes the structure of the WHERE clause.
            /// </summary>
            public static class Where
            {
                /// <summary>
                /// Gets the identifier indicating that there is a WHERE clause.
                /// </summary>
                public const string Name = "Where";

                /// <summary>
                /// Gets the identifier for the WHERE keyword.
                /// </summary>
                public const string WhereKeyword = "where";

                /// <summary>
                /// Gets the identifier for the filter list.
                /// </summary>
                public const string FilterList = "filter_list";
            }
        }

        private void defineUpdateStatement()
        {
            Define(UpdateStatement.Name)
                .Add(UpdateStatement.UpdateKeyword, true, Token(SqlTokenRegistry.Update))
                .Add(UpdateStatement.Table, true, Expression(MultipartIdentifier.Name))
                .Add(UpdateStatement.AliasExpression.Name, false, Define()
                    .Add(UpdateStatement.AliasExpression.AliasIndicator, false, Token(SqlTokenRegistry.AliasIndicator))
                    .Add(UpdateStatement.AliasExpression.Alias, true, Token(SqlTokenRegistry.Identifier)))
                .Add(UpdateStatement.SetKeyword, true, Token(SqlTokenRegistry.Set))
                .Add(UpdateStatement.SetterList, true, Expression(SetterList.Name))
                .Add(UpdateStatement.Where.Name, false, Define()
                    .Add(UpdateStatement.Where.WhereKeyword, true, Token(SqlTokenRegistry.Where))
                    .Add(UpdateStatement.Where.FilterList, true, Expression(FilterList.Name)));
        }

        #endregion

        #region SetterList

        /// <summary>
        /// Describes the structure of the setter list.
        /// </summary>
        public static class SetterList
        {
            /// <summary>
            /// Gets the name identifying the setter list.
            /// </summary>
            public const string Name = "SetterList";

            /// <summary>
            /// Describes the structure of a setter list when there is more than one item.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that there are multiple setters in the list.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first setter.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the comma separator.
                /// </summary>
                public const string Comma = "comma";

                /// <summary>
                /// Gets the identifier for the rest of the setters in the list.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier indicating that the list only has one setter.
            /// </summary>
            public const string Single = "single";
        }

        private void defineSetterList()
        {
            Define(SetterList.Name)
                .Add(true, Options()
                    .Add(SetterList.Multiple.Name, Define()
                        .Add(SetterList.Multiple.First, true, Expression(Setter.Name))
                        .Add(SetterList.Multiple.Comma, true, Token(SqlTokenRegistry.Comma))
                        .Add(SetterList.Multiple.Remaining, true, Expression(SetterList.Name)))
                    .Add(SetterList.Single, Expression(Setter.Name)));
        }

        #endregion

        #region Setter

        /// <summary>
        /// Describes the structure of a setter.
        /// </summary>
        public static class Setter
        {
            /// <summary>
            /// Gets the name identifying the setter.
            /// </summary>
            public const string Name = "Setter";

            /// <summary>
            /// Gets the identifier for the column being assigned.
            /// </summary>
            public const string Column = "column";

            /// <summary>
            /// Gets the identifier for the assignment operator.
            /// </summary>
            public const string Assignment = "assignment";

            /// <summary>
            /// Gets the identifier for the value the column is being assigned to.
            /// </summary>
            public const string Value = "item";
        }

        private void defineSetter()
        {
            Define(Setter.Name)
                .Add(Setter.Column, true, Expression(MultipartIdentifier.Name))
                .Add(Setter.Assignment, true, Token(SqlTokenRegistry.EqualTo))
                .Add(Setter.Value, true, Expression(ArithmeticItem.Name));
        }

        #endregion

        #region DeleteStatement

        /// <summary>
        /// Describes the structure of the DELETE statement.
        /// </summary>
        public static class DeleteStatement
        {
            /// <summary>
            /// Gets the name identifying the DELETE statement.
            /// </summary>
            public const string Name = "DeleteStatement";

            /// <summary>
            /// Gets the identifier for the DELETE keyword.
            /// </summary>
            public const string DeleteKeyword = "delete";

            /// <summary>
            /// Gets the identifier for the FROM keyword.
            /// </summary>
            public const string FromKeyword = "from";

            /// <summary>
            /// Gets the identifeir for the table name.
            /// </summary>
            public const string Table = "table";

            /// <summary>
            /// Describes the structure of the alias for the table.
            /// </summary>
            public static class AliasExpression
            {
                /// <summary>
                /// Gets the identifier indicating that the table is aliased.
                /// </summary>
                public const string Name = "AliasExpression";

                /// <summary>
                /// Gets the indentifier for the AS keyword.
                /// </summary>
                public const string AliasIndicator = "alias_indicator";

                /// <summary>
                /// Gets the identifier for the alias.
                /// </summary>
                public const string Alias = "alias";
            }

            /// <summary>
            /// Describes the structure of the WHERE clause.
            /// </summary>
            public static class Where
            {
                /// <summary>
                /// Gets the indentifier that indicates whether the WHERE clause is present.
                /// </summary>
                public const string Name = "Where";

                /// <summary>
                /// Gets the indentifier for the WHERE keyword.
                /// </summary>
                public const string WhereKeyword = "where";

                /// <summary>
                /// Gets the identifier for the filter list.
                /// </summary>
                public const string FilterList = "filter_list";
            }
        }

        private void defineDeleteStatement()
        {
            Define(DeleteStatement.Name)
                .Add(DeleteStatement.DeleteKeyword, true, Token(SqlTokenRegistry.Delete))
                .Add(DeleteStatement.FromKeyword, false, Token(SqlTokenRegistry.From))
                .Add(DeleteStatement.Table, true, Expression(MultipartIdentifier.Name))
                .Add(DeleteStatement.AliasExpression.Name, false, Define()
                    .Add(DeleteStatement.AliasExpression.AliasIndicator, false, Token(SqlTokenRegistry.AliasIndicator))
                    .Add(DeleteStatement.AliasExpression.Alias, true, Token(SqlTokenRegistry.Identifier)))
                .Add(DeleteStatement.Where.Name, false, Define()
                    .Add(DeleteStatement.Where.WhereKeyword, true, Token(SqlTokenRegistry.Where))
                    .Add(DeleteStatement.Where.FilterList, true, Expression(FilterList.Name)));
        }

        #endregion

        #region MultipartIdentifier

        /// <summary>
        /// Describes the structure of a multi-part identifier.
        /// </summary>
        public static class MultipartIdentifier
        {
            /// <summary>
            /// Gets the name identifying the multi-part identifier.
            /// </summary>
            public const string Name = "MultipartIdentifier";

            /// <summary>
            /// Describes the structure of an identifier with multiple parts.
            /// </summary>
            public static class Multiple
            {
                /// <summary>
                /// Gets the identifier indicating that there are multiple parts.
                /// </summary>
                public const string Name = "Multiple";

                /// <summary>
                /// Gets the identifier for the first identifier.
                /// </summary>
                public const string First = "first";

                /// <summary>
                /// Gets the identifier for the dot separator.
                /// </summary>
                public const string Dot = "dot";
                
                /// <summary>
                /// Gets the identifier for the rest of the identifiers.
                /// </summary>
                public const string Remaining = "remaining";
            }

            /// <summary>
            /// Gets the identifier indicating that there is a single identifier.
            /// </summary>
            public const string Single = "single";
        }

        private void defineMultipartIdentifier()
        {
            Define(MultipartIdentifier.Name)
                .Add(true, Options()
                    .Add(MultipartIdentifier.Multiple.Name, Define()
                        .Add(MultipartIdentifier.Multiple.First, true, Token(SqlTokenRegistry.Identifier))
                        .Add(MultipartIdentifier.Multiple.Dot, true, Token(SqlTokenRegistry.Dot))
                        .Add(MultipartIdentifier.Multiple.Remaining, true, Expression(MultipartIdentifier.Name)))
                    .Add(MultipartIdentifier.Single, Token(SqlTokenRegistry.Identifier)));
        }

        #endregion
    }
}
