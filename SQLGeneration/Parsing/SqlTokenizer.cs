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
        /// Gets the identifier for arithmetic operators.
        /// </summary>
        public const string ArithmeticOperator = "ArithmeticOperator";

        /// <summary>
        /// Gets the identifier for the setter assignment operator.
        /// </summary>
        public const string Assignment = "Assignment";

        /// <summary>
        /// Gets the identifier for the BETWEEN keyword.
        /// </summary>
        public const string Between = "Between";

        /// <summary>
        /// Gets the identifier for the AND keyword in a BETWEEN filter.
        /// </summary>
        public const string BetweenAnd = "BetweenAnd";

        /// <summary>
        /// Gets the identifier for the comma separator.
        /// </summary>
        public const string Comma = "Comma";

        /// <summary>
        /// Gets the identifier for a comparison operator.
        /// </summary>
        public const string ComparisonOperator = "ComparisonOperator";

        /// <summary>
        /// Gets the identifier for a filter conjunction (AND or OR).
        /// </summary>
        public const string Conjunction = "Conjunction";

        /// <summary>
        /// Gets the identifier for the DELETE keyword.
        /// </summary>
        public const string Delete = "Delete";

        /// <summary>
        /// Gets the identifier for the distinct qualifiers (DISTINCT or ALL).
        /// </summary>
        public const string DistinctQualifier = "DistinctQualifier";

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
        /// Gets the identifier for join types.
        /// </summary>
        public const string JoinType = "JoinType";
        
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
        /// Gets the identifier for a null placement indicator (NULLS FIRST or NULLS LAST).
        /// </summary>
        public const string NullPlacement = "NullPlacement";

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
        /// Gets the identifier for ORDER BY directions (ASC or DESC).
        /// </summary>
        public const string OrderDirection = "OrderDirection";

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
        /// Gets the identifier for SELECT combiners (UNION, UNION ALL, etc.)
        /// </summary>
        public const string SelectCombiner = "SelectCombiner";

        /// <summary>
        /// Gets the identifier for the SET keyword.
        /// </summary>
        public const string Set = "Set";

        /// <summary>
        /// Gets the identifier for the star (*) symbol.
        /// </summary>
        public const string Star = "Star";
        
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
        /// Initializes a new instance of a SqlTokenizer.
        /// </summary>
        public SqlTokenizer()
        {
            Define(AliasIndicator, @"AS", RegexOptions.IgnoreCase);
            Define(ArithmeticOperator, @"\+");
            Define(ArithmeticOperator, @"-");
            Define(ArithmeticOperator, @"\*");
            Define(ArithmeticOperator, @"/");
            Define(Assignment, @"=");
            Define(Between, @"BETWEEN", RegexOptions.IgnoreCase);
            Define(BetweenAnd, @"AND", RegexOptions.IgnoreCase);
            Define(Comma, @",");
            Define(ComparisonOperator, @"<=");
            Define(ComparisonOperator, @">=");
            Define(ComparisonOperator, @"<>");
            Define(ComparisonOperator, @"=");
            Define(ComparisonOperator, @"<");
            Define(ComparisonOperator, @">");
            Define(Conjunction, @"AND", RegexOptions.IgnoreCase);
            Define(Conjunction, @"OR", RegexOptions.IgnoreCase);
            Define(Delete, @"DELETE", RegexOptions.IgnoreCase);
            Define(DistinctQualifier, @"ALL", RegexOptions.IgnoreCase);
            Define(DistinctQualifier, @"DISTINCT", RegexOptions.IgnoreCase);
            Define(Dot, @"\.");
            Define(From, @"FROM", RegexOptions.IgnoreCase);
            Define(GroupBy, @"GROUP\s+BY", RegexOptions.IgnoreCase);
            Define(Having, @"HAVING", RegexOptions.IgnoreCase);
            Define(Identifier, @"\w(\w|\d)*");
            Define(Identifier, @"""(\.|"""")+""");
            Define(Identifier, @"\[[^\]]+\]");
            Define(In, @"IN", RegexOptions.IgnoreCase);
            Define(Insert, @"INSERT", RegexOptions.IgnoreCase);
            Define(Into, @"INTO", RegexOptions.IgnoreCase);
            Define(Is, @"IS", RegexOptions.IgnoreCase);
            Define(JoinType, @"CROSS\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"FULL\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"FULL\s+OUTER\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"INNER\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"LEFT\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"LEFT\s+OUTER\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"RIGHT\s+JOIN", RegexOptions.IgnoreCase);
            Define(JoinType, @"RIGHT\s+OUTER\s+JOIN", RegexOptions.IgnoreCase);
            Define(LeftParenthesis, @"\(");
            Define(Like, @"LIKE", RegexOptions.IgnoreCase);
            Define(Not, @"NOT", RegexOptions.IgnoreCase);
            Define(Null, @"NULL", RegexOptions.IgnoreCase);
            Define(NullPlacement, @"NULLS\s+FIRST", RegexOptions.IgnoreCase);
            Define(NullPlacement, @"NULLS\s+LAST", RegexOptions.IgnoreCase);
            Define(Number, @"[-+]?\d*\.?\d+([eE][-+]?\d+)?");
            Define(On, @"ON", RegexOptions.IgnoreCase);
            Define(OrderBy, @"ORDER\s+BY", RegexOptions.IgnoreCase);
            Define(OrderDirection, @"ASC", RegexOptions.IgnoreCase);
            Define(OrderDirection, @"DESC", RegexOptions.IgnoreCase);
            Define(Percent, @"PERCENT", RegexOptions.IgnoreCase);
            Define(RightParenthesis, @"\)");
            Define(Select, @"SELECT", RegexOptions.IgnoreCase);
            Define(SelectCombiner, @"UNION", RegexOptions.IgnoreCase);
            Define(SelectCombiner, @"UNION ALL", RegexOptions.IgnoreCase);
            Define(SelectCombiner, @"INTERSECT", RegexOptions.IgnoreCase);
            Define(SelectCombiner, @"EXCEPT", RegexOptions.IgnoreCase);
            Define(SelectCombiner, @"MINUS", RegexOptions.IgnoreCase);
            Define(Set, @"SET", RegexOptions.IgnoreCase);
            Define(Star, @"\*");
            Define(String, @"'([^']|'')*'");
            Define(Top, @"TOP", RegexOptions.IgnoreCase);
            Define(Update, @"UPDATE", RegexOptions.IgnoreCase);
            Define(Values, @"VALUES", RegexOptions.IgnoreCase);
            Define(Where, @"WHERE", RegexOptions.IgnoreCase);
            Define(WithTies, @"WITH\s+TIES", RegexOptions.IgnoreCase);
        }
    }
}
