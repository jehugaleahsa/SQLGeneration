using System;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Identifies tokens as SQL tokens.
    /// </summary>
    public class SqlTokenRegistry : TokenRegistry
    {
        /// <summary>
        /// Gets the identifier for alias indicators (AS).
        /// </summary>
        public const string AliasIndicator = "AliasIndicator";

        /// <summary>
        /// Gets the identifier for the addition operator.
        /// </summary>
        public const string PlusOperator = "PlusOperator";

        /// <summary>
        /// Gets the identifier for the subtraction operator.
        /// </summary>
        public const string MinusOperator = "MinusOperator";

        /// <summary>
        /// Gets the identifier for the multiplication operator.
        /// </summary>
        public const string MultiplicationOperator = "MultiplyOperator";

        /// <summary>
        /// Gets the identifier for the division operator.
        /// </summary>
        public const string DivisionOperator = "DivideOperator";

        /// <summary>
        /// Gets the identifier for the BETWEEN keyword.
        /// </summary>
        public const string Between = "Between";

        /// <summary>
        /// Gets the identifier for the comma separator.
        /// </summary>
        public const string Comma = "Comma";

        /// <summary>
        /// Gets the identifier for the AND keyword.
        /// </summary>
        public const string And = "And";

        /// <summary>
        /// Gets the identifier for the OR keyword.
        /// </summary>
        public const string Or = "Or";

        /// <summary>
        /// Gets the identifier for the DELETE keyword.
        /// </summary>
        public const string Delete = "Delete";

        /// <summary>
        /// Gets the identifier for the ALL keyword.
        /// </summary>
        public const string All = "All";

        /// <summary>
        /// Gets the identifier for the ANY keyword.
        /// </summary>
        public const string Any = "Any";

        /// <summary>
        /// Gets the identifier for the SOME keyword.
        /// </summary>
        public const string Some = "Some";

        /// <summary>
        /// Gets the identifier for the DISTINCT keyword.
        /// </summary>
        public const string Distinct = "Distinct";

        /// <summary>
        /// Gets the identifier for the dot separator.
        /// </summary>
        public const string Dot = "Dot";

        /// <summary>
        /// Gets the identifier for the FROM keyword.
        /// </summary>
        public const string From = "From";

        /// <summary>
        /// Gets the identifier for the GROUP BY keyword.
        /// </summary>
        public const string GroupBy = "GroupBy";

        /// <summary>
        /// Gets the identifier for the HAVING keyword.
        /// </summary>
        public const string Having = "Having";

        /// <summary>
        /// Gets the identifier for identifiers.
        /// </summary>
        public const string Identifier = "Identifier";

        /// <summary>
        /// Gets the identifier for the IN keyword.
        /// </summary>
        public const string In = "In";

        /// <summary>
        /// Gets the identifier for the INSERT keyword.
        /// </summary>
        public const string Insert = "Insert";

        /// <summary>
        /// Gets the indentifier for the INTO keyword.
        /// </summary>
        public const string Into = "Into";

        /// <summary>
        /// Gets the identifier for the IS keyword.
        /// </summary>
        public const string Is = "Is";

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

        /// <summary>
        /// Gets the identifier for the CROSS JOIN keyword.
        /// </summary>
        public const string CrossJoin = "CrossJoin";
        
        /// <summary>
        /// Gets the identifier for a left parenthesis.
        /// </summary>
        public const string LeftParenthesis = "LeftParenthesis";

        /// <summary>
        /// Gets the identifier for the LIKE keyword.
        /// </summary>
        public const string Like = "Like";

        /// <summary>
        /// Gets the identifier for the NOT keyword.
        /// </summary>
        public const string Not = "Not";

        /// <summary>
        /// Gets the identifier for the NULL keyword.
        /// </summary>
        public const string Null = "Null";

        /// <summary>
        /// Gets the identifier for the NULLS FIRST keyword.
        /// </summary>
        public const string NullsFirst = "NullsFirst";

        /// <summary>
        /// Gets the identifier for the NULLS LAST keyword.
        /// </summary>
        public const string NullsLast = "NullsLast";

        /// <summary>
        /// Gets the idenifier for numeric literals.
        /// </summary>
        public const string Number = "Number";

        /// <summary>
        /// Gets the identifier for the ON keyword.
        /// </summary>
        public const string On = "On";

        /// <summary>
        /// Gets the identifier for the ORDER BY keyword.
        /// </summary>
        public const string OrderBy = "OrderBy";

        /// <summary>
        /// Gets the identifier for the DESC keyword.
        /// </summary>
        public const string Descending = "Descending";

        /// <summary>
        /// Gets the identifier for the ASC keyword.
        /// </summary>
        public const string Ascending = "Ascending";

        /// <summary>
        /// Gets the identifier for the PERCENT keyword.
        /// </summary>
        public const string Percent = "Percent";

        /// <summary>
        /// Gets the identifier for the right parenthesis.
        /// </summary>
        public const string RightParenthesis = "RightParenthesis";

        /// <summary>
        /// Gets the identifier for the SELECT keyword.
        /// </summary>
        public const string Select = "Select";

        /// <summary>
        /// Gets the identifier for the UNION keyword.
        /// </summary>
        public const string Union = "Union";

        /// <summary>
        /// Gets the identfiier for the INTERSECT keyword.
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

        /// <summary>
        /// Gets the identifier for the SET keyword.
        /// </summary>
        public const string Set = "Set";
        
        /// <summary>
        /// Gets the identifier for a string literal.
        /// </summary>
        public const string String = "String";

        /// <summary>
        /// Gets the identifier for the TOP keyword.
        /// </summary>
        public const string Top = "Top";

        /// <summary>
        /// Gets the identifier for the UPDATE keyword.
        /// </summary>
        public const string Update = "Update";

        /// <summary>
        /// Gets the identifier for the VALUES keyword.
        /// </summary>
        public const string Values = "Values";

        /// <summary>
        /// Gets the identifier for the WHERE keyword.
        /// </summary>
        public const string Where = "Where";

        /// <summary>
        /// Gets the identifier for the WITH TIES keyword.
        /// </summary>
        public const string WithTies = "WithTies";

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

        /// <summary>
        /// Gets the identifier for the EXISTS keyword.
        /// </summary>
        public const string Exists = "exists";

        /// <summary>
        /// Gets the identifier for the OVER keyword.
        /// </summary>
        public const string Over = "over";

        /// <summary>
        /// Gets the identifier for the PARTITION BY keyword.
        /// </summary>
        public const string PartitionBy = "partition_by";

        /// <summary>
        /// Gets the identifier for the ROWS keyword.
        /// </summary>
        public const string Rows = "rows";

        /// <summary>
        /// Gets the identifier for the RANGE keyword.
        /// </summary>
        public const string Range = "range";

        /// <summary>
        /// Gets the identifier for the UNBOUNDED keyword.
        /// </summary>
        public const string Unbounded = "unbounded";

        /// <summary>
        /// Gets the identifier for the PRECEECING keyword.
        /// </summary>
        public const string Preceding = "preceding";

        /// <summary>
        /// Gets the identifier for the FOLLOWING keyword.
        /// </summary>
        public const string Following = "following";

        /// <summary>
        /// Gets the identifier for the CURRENT ROW keyword.
        /// </summary>
        public const string CurrentRow = "current_row";

        /// <summary>
        /// Initializes a new instance of a SqlTokenizer.
        /// </summary>
        public SqlTokenRegistry()
        {
            Define(Top, @"TOP\b", true);
            Define(Update, @"UPDATE\b", true);
            Define(Values, @"VALUES\b", true);
            Define(Where, @"WHERE\b", true);
            Define(WithTies, @"WITH\s+TIES\b", true);
            Define(Between, @"BETWEEN\b", true);
            Define(And, @"AND\b", true);
            Define(Or, @"OR\b", true);
            Define(Delete, @"DELETE\b", true);
            Define(All, @"ALL\b", true);
            Define(Any, @"ANY\b", true);
            Define(Some, @"SOME\b", true);
            Define(Distinct, @"DISTINCT\b", true);
            Define(From, @"FROM\b", true);
            Define(GroupBy, @"GROUP\s+BY\b", true);
            Define(Having, @"HAVING\b", true);
            Define(Insert, @"INSERT\b", true);
            Define(Into, @"INTO\b", true);
            Define(Is, @"IS\b", true);
            Define(FullOuterJoin, @"FULL\s+(OUTER\s+)?JOIN\b", true);
            Define(InnerJoin, @"(INNER\s+)?JOIN\b", true);
            Define(LeftOuterJoin, @"LEFT\s+(OUTER\s+)?JOIN\b", true);
            Define(RightOuterJoin, @"RIGHT\s+(OUTER\s+)?JOIN\b", true);
            Define(CrossJoin, @"CROSS\s+JOIN\b", true);
            Define(In, @"IN\b", true);
            Define(Like, @"LIKE\b", true);
            Define(Not, @"NOT\b", true);
            Define(NullsFirst, @"NULLS\s+FIRST\b", true);
            Define(NullsLast, @"NULLS\s+LAST\b", true);
            Define(Null, @"NULL\b", true);
            Define(OrderBy, @"ORDER\s+BY\b", true);
            Define(Ascending, @"ASC\b", true);
            Define(Descending, @"DESC\b", true);
            Define(Percent, @"PERCENT\b", true);
            Define(Select, @"SELECT\b", true);
            Define(Union, @"UNION\b", true);
            Define(Intersect, @"INTERSECT\b", true);
            Define(Except, @"EXCEPT\b", true);
            Define(Minus, @"MINUS\b", true);
            Define(Set, @"SET\b", true);
            Define(On, @"ON\b", true);
            Define(AliasIndicator, @"AS\b", true);
            Define(Exists, @"EXISTS\b", true);
            Define(Over, @"OVER\b", true);
            Define(PartitionBy, @"PARTITION\s+BY\b", true);
            Define(Rows, @"ROWS\b", true);
            Define(Range, @"RANGE\b", true);
            Define(Unbounded, @"UNBOUNDED\b", true);
            Define(Preceding, @"PRECEDING\b", true);
            Define(Following, @"FOLLOWING\b", true);
            Define(CurrentRow, @"CURRENT\s+ROW\b", true);

            Define(Identifier, @"([\p{L}:?@#_][\p{L}\p{N}@#$_]*)|(""(\.|"""")+"")|(\[[^\]]+\])");

            Define(PlusOperator, @"\+");
            Define(MinusOperator, @"-");
            Define(MultiplicationOperator, @"\*");
            Define(DivisionOperator, @"/");
            Define(Comma, @",");
            Define(EqualTo, @"=");
            Define(NotEqualTo, @"<>");
            Define(LessThanEqualTo, @"<=");
            Define(GreaterThanEqualTo, @">=");
            Define(LessThan, @"<");
            Define(GreaterThan, @">");
            Define(Dot, @"\.");
            Define(LeftParenthesis, @"\(");
            Define(Number, @"[-+]?\d*\.?\d+([eE][-+]?\d+)?");
            Define(RightParenthesis, @"\)");
            Define(String, @"'([^']|'')*'");
        }
    }
}
