using System;

namespace SQLGeneration.Expressions
{
    /// <summary>
    /// Specifies the type of an expression.
    /// </summary>
    public enum ExpressionItemType
    {
        /// <summary>
        /// The expression is arithmetic.
        /// </summary>
        Arithmetic,
        /// <summary>
        /// The expression is a BETWEEN filter.
        /// </summary>
        BetweenFilter,
        /// <summary>
        /// The expression is a filter that compares two values using an operator.
        /// </summary>
        BinaryFilter,
        /// <summary>
        /// The expression is a column list within an INSERT command.
        /// </summary>
        ColumnList,
        /// <summary>
        /// The expression is a DELETE command.
        /// </summary>
        DeleteCommand,
        /// <summary>
        /// The expression is a filter.
        /// </summary>
        Filter,
        /// <summary>
        /// The expression is a list of join items in a SELECT statement.
        /// </summary>
        FromList,
        /// <summary>
        /// The expression is a function call.
        /// </summary>
        Function,
        /// <summary>
        /// The expression is a list of projection items in the GROUP BY clause of a SELECT statement.
        /// </summary>
        GroupByList,
        /// <summary>
        /// The expression is an INSERT command.
        /// </summary>
        InsertCommand,
        /// <summary>
        /// The expression is a JOIN within a SELECT command.
        /// </summary>
        Join,
        /// <summary>
        /// The expression is empty and should be ignored.
        /// </summary>
        None,
        /// <summary>
        /// The expression is a filter comparing a value to null.
        /// </summary>
        NullFilter,
        /// <summary>
        /// The expression is an order item in the ORDER BY clause of a SELECT command.
        /// </summary>
        OrderBy,
        /// <summary>
        /// The expression is a list of the order items in the ORDER BY clause of a SELECT command.
        /// </summary>
        OrderByList,
        /// <summary>
        /// The expression is declaring a projection item.
        /// </summary>
        ProjectionDeclaration,
        /// <summary>
        /// The expression is a projection with a SELECT command.
        /// </summary>
        ProjectionItemList,
        /// <summary>
        /// The expression is referencing a projection item.
        /// </summary>
        ProjectionReference,
        /// <summary>
        /// The expression is combining two SELECT commands.
        /// </summary>
        SelectCombiner,
        /// <summary>
        /// The expression isa SELECT command.
        /// </summary>
        SelectCommand,
        /// <summary>
        /// The expression is a setter within an UPDATE command.
        /// </summary>
        Setter,
        /// <summary>
        /// The expression is declaring a table.
        /// </summary>
        TableDeclaration,
        /// <summary>
        /// The expression is referencing a table.
        /// </summary>
        TableReference,
        /// <summary>
        /// The expression is a token.
        /// </summary>
        Token,
        /// <summary>
        /// The expression is a TOP clause within a SELECT command.
        /// </summary>
        Top,
        /// <summary>
        /// The expression is an UPDATE command.
        /// </summary>
        UpdateCommand,
        /// <summary>
        /// The expression is a value list.
        /// </summary>
        ValueList,
    }
}
