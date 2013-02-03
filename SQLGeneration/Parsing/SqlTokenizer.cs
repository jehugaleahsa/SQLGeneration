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
        /// Initializes a new instance of a SqlTokenizer.
        /// </summary>
        /// <param name="tokenStream">The token stream to inspect.</param>
        public SqlTokenizer(IEnumerable<string> tokenStream)
            : base(tokenStream)
        {
            Define("AliasIndicator", @"AS", RegexOptions.IgnoreCase);
            Define("ArithmeticOperator", @"\+");
            Define("ArithmeticOperator", @"-");
            Define("ArithmeticOperator", @"\*");
            Define("ArithmeticOperator", @"/");
            Define("Assignment", @"=");
            Define("Between", @"BETWEEN", RegexOptions.IgnoreCase);
            Define("BetweenAnd", @"AND", RegexOptions.IgnoreCase);
            Define("Comma", @",");
            Define("ComparisonOperator", @"<=");
            Define("ComparisonOperator", @">=");
            Define("ComparisonOperator", @"<>");
            Define("ComparisonOperator", @"=");
            Define("ComparisonOperator", @"<");
            Define("ComparisonOperator", @">");
            Define("Conjunction", @"AND", RegexOptions.IgnoreCase);
            Define("Conjunction", @"OR", RegexOptions.IgnoreCase);
            Define("Delete", @"DELETE", RegexOptions.IgnoreCase);
            Define("DistinctQualifier", @"ALL", RegexOptions.IgnoreCase);
            Define("DistinctQualifier", @"DISTINCT", RegexOptions.IgnoreCase);
            Define("Dot", @"\.");
            Define("From", @"FROM", RegexOptions.IgnoreCase);
            Define("GroupBy", @"GROUP\s+BY", RegexOptions.IgnoreCase);
            Define("Having", @"HAVING", RegexOptions.IgnoreCase);
            Define("Identifier", @"\w(\w|\d)*");
            Define("Identifier", @"""(\.|"""")+""");
            Define("Identifier", @"\[[^\]]+\]");
            Define("In", @"IN", RegexOptions.IgnoreCase);
            Define("Insert", @"INSERT", RegexOptions.IgnoreCase);
            Define("Into", @"INTO", RegexOptions.IgnoreCase);
            Define("Is", @"IS", RegexOptions.IgnoreCase);
            Define("JoinType", @"CROSS\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"FULL\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"FULL\s+OUTER\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"INNER\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"LEFT\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"LEFT\s+OUTER\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"RIGHT\s+JOIN", RegexOptions.IgnoreCase);
            Define("JoinType", @"RIGHT\s+OUTER\s+JOIN", RegexOptions.IgnoreCase);
            Define("LeftParenthesis", @"\(");
            Define("Like", @"LIKE", RegexOptions.IgnoreCase);
            Define("Not", @"NOT", RegexOptions.IgnoreCase);
            Define("Null", @"NULL", RegexOptions.IgnoreCase);
            Define("NullPlacement", @"NULLS\s+FIRST", RegexOptions.IgnoreCase);
            Define("NullPlacement", @"NULLS\s+LAST", RegexOptions.IgnoreCase);
            Define("Number", @"[-+]?\d*\.?\d+([eE][-+]?\d+)?");
            Define("On", @"ON", RegexOptions.IgnoreCase);
            Define("OrderBy", @"ORDER\s+BY", RegexOptions.IgnoreCase);
            Define("OrderDirection", @"ASC", RegexOptions.IgnoreCase);
            Define("OrderDirection", @"DESC", RegexOptions.IgnoreCase);
            Define("Percent", @"PERCENT", RegexOptions.IgnoreCase);
            Define("RightParenthesis", @"\)");
            Define("Select", @"SELECT", RegexOptions.IgnoreCase);
            Define("SelectCombiner", @"UNION", RegexOptions.IgnoreCase);
            Define("SelectCombiner", @"UNION ALL", RegexOptions.IgnoreCase);
            Define("SelectCombiner", @"INTERSECT", RegexOptions.IgnoreCase);
            Define("SelectCombiner", @"EXCEPT", RegexOptions.IgnoreCase);
            Define("SelectCombiner", @"MINUS", RegexOptions.IgnoreCase);
            Define("Set", @"SET", RegexOptions.IgnoreCase);
            Define("Star", @"\*");
            Define("String", @"'([^']|'')*'");
            Define("Top", @"TOP", RegexOptions.IgnoreCase);
            Define("Update", @"UPDATE", RegexOptions.IgnoreCase);
            Define("Values", @"VALUES", RegexOptions.IgnoreCase);
            Define("Where", @"WHERE", RegexOptions.IgnoreCase);
            Define("WithTies", @"WITH\s+TIES", RegexOptions.IgnoreCase);
        }
    }
}
