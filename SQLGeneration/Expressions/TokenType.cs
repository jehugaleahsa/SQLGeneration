using System;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Specifies what the type a token is.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The token is an alias.
        /// </summary>
        Alias,
        /// <summary>
        /// The token indicates that an alias is being defined.
        /// </summary>
        AliasIndicator,
        /// <summary>
        /// The token is an arithmetic operator.
        /// </summary>
        ArithmeticOperator,
        /// <summary>
        /// The token is the assigning a value in the SET clause of an UPDATE command.
        /// </summary>
        Assignment,
        /// <summary>
        /// The token is a column name.
        /// </summary>
        ColumnName,
        /// <summary>
        /// The token is a comma.
        /// </summary>
        Comma,
        /// <summary>
        /// The token is a comparison operator.
        /// </summary>
        ComparisonOperator,
        /// <summary>
        /// The token is a logical conjunction.
        /// </summary>
        Conjunction,
        /// <summary>
        /// The token is a dot used to qualify a name.
        /// </summary>
        Dot,
        /// <summary>
        /// The token is the name of a function.
        /// </summary>
        FunctionName,
        /// <summary>
        /// The token specifies how to items are joined.
        /// </summary>
        JoinType,
        /// <summary>
        /// The token is a keyword.
        /// </summary>
        Keyword,
        /// <summary>
        /// The token is a left parenthesis.
        /// </summary>
        LeftParenthesis,
        /// <summary>
        /// The token is the NULL literal.
        /// </summary>
        Null,
        /// <summary>
        /// The token specifies where nulls appear when ordering.
        /// </summary>
        NullPlacement,
        /// <summary>
        /// The token is a numeric literal.
        /// </summary>
        Number,
        /// <summary>
        /// The token specifies the direction when ordering.
        /// </summary>
        OrderDirection,
        /// <summary>
        /// The token is a general-purpose placeholder.
        /// </summary>
        Placeholder,
        /// <summary>
        /// The token is a right parenthesis.
        /// </summary>
        RightParenthesis,
        /// <summary>
        /// The token is the name of a schema.
        /// </summary>
        SchemaName,
        /// <summary>
        /// The token combines two SELECT commands.
        /// </summary>
        SelectCombiner,
        /// <summary>
        /// The token is a string literal.
        /// </summary>
        String,
        /// <summary>
        /// The token is the name of a table.
        /// </summary>
        TableName,
    }
}
