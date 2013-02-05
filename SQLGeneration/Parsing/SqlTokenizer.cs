using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SQLGeneration.Parsing
{
    /// <summary>
    /// Identifies tokens as SQL tokens.
    /// </summary>
    public class SqlTokenizer : Tokenizer
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
        /// Gets the identifier for the UNION ALL keyword.
        /// </summary>
        public const string UnionAll = "UnionAll";

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
        /// Initializes a new instance of a SqlTokenizer.
        /// </summary>
        public SqlTokenizer()
        {
            Define(Top, @"TOP", true);
            Define(Update, @"UPDATE", true);
            Define(Values, @"VALUES", true);
            Define(Where, @"WHERE", true);
            Define(WithTies, @"WITH\s+TIES", true);
            Define(Between, @"BETWEEN", true);
            Define(And, @"AND", true);
            Define(Or, @"OR", true);
            Define(Delete, @"DELETE", true);
            Define(All, @"ALL", true);
            Define(Distinct, @"DISTINCT", true);
            Define(From, @"FROM", true);
            Define(GroupBy, @"GROUP\s+BY", true);
            Define(Having, @"HAVING", true);
            Define(Insert, @"INSERT", true);
            Define(Into, @"INTO", true);
            Define(Is, @"IS", true);
            Define(FullOuterJoin, @"FULL\s+(OUTER\s+)?JOIN", true);
            Define(InnerJoin, @"(INNER\s+)?JOIN", true);
            Define(LeftOuterJoin, @"LEFT\s+(OUTER\s+)?JOIN", true);
            Define(RightOuterJoin, @"RIGHT\s+(OUTER\s+)?JOIN", true);
            Define(CrossJoin, @"CROSS\s+JOIN", true);
            Define(In, @"IN", true);
            Define(Like, @"LIKE", true);
            Define(Not, @"NOT", true);
            Define(NullsFirst, @"NULLS\s+FIRST", true);
            Define(NullsLast, @"NULLS\s+LAST", true);
            Define(Null, @"NULL", true);
            Define(OrderBy, @"ORDER\s+BY", true);
            Define(Ascending, @"ASC", true);
            Define(Descending, @"DESC", true);
            Define(Percent, @"PERCENT", true);
            Define(Select, @"SELECT", true);
            Define(UnionAll, @"UNION ALL", true);
            Define(Union, @"UNION", true);
            Define(Intersect, @"INTERSECT", true);
            Define(Except, @"EXCEPT", true);
            Define(Minus, @"MINUS", true);
            Define(Set, @"SET", true);
            Define(On, @"ON", true);
            Define(AliasIndicator, @"AS", true);

            Define(Identifier, @"(\w(\w|\d)*)|(""(\.|"""")+"")");

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
