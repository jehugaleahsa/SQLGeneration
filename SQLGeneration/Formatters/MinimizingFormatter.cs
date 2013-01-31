using System;
using System.IO;
using System.Text;
using SQLGeneration.Expressions;

namespace SQLGeneration.Formatters
{
    /// <summary>
    /// Formats the command expressions given to it by generating the minimal amount of text.
    /// </summary>
    public class MinimizingFormatter
    {
        private readonly ExpressionRegistry registry;

        /// <summary>
        /// Initializes a new instance of a MinimizingFormatter.
        /// </summary>
        public MinimizingFormatter()
        {
            this.registry = new ExpressionRegistry();

            defineAll();
            defineToken();

            defineArithmetic();
            defineBetweenFilter();
            defineColumn();
            defineJoin();
            defineProjectionDeclaration();
            defineProjectionItemList();
            defineSelectCommand();
            defineTableReference();
            defineTop();
        }

        private void defineAll()
        {
            registry.Define(ExpressionItemType.None)
                .RequireAny(options =>
                {
                    options.Define().Require(ExpressionItemType.SelectCommand);
                    options.Define().Require(ExpressionItemType.SelectCombiner);
                    options.Define().Require(ExpressionItemType.InsertCommand);
                    options.Define().Require(ExpressionItemType.UpdateCommand);
                    options.Define().Require(ExpressionItemType.DeleteCommand);
                });
        }

        private void defineToken()
        {
            registry.Define(ExpressionItemType.Token);
        }

        private void defineArithmetic()
        {
            registry.Define(ExpressionItemType.Arithmetic)
                .RequireAny(options =>
                {
                    options.Define()
                        .Require(TokenType.LeftParenthesis)
                        .Require(ExpressionItemType.Arithmetic)
                        .Require(TokenType.RightParenthesis);
                    options.Define()
                        .Require(ExpressionItemType.ProjectionReference)
                        .Require(TokenType.ArithmeticOperator)
                        .Require(ExpressionItemType.ProjectionReference);
                    options.Define()
                        .Require(TokenType.Number);
                });
        }

        private void defineBetweenFilter()
        {
            registry.Define(ExpressionItemType.BetweenFilter)
                .Require(ExpressionItemType.ProjectionReference)
                .Optional(TokenType.Keyword, "NOT")
                .Require(TokenType.Keyword, "BETWEEN")
                .Require(ExpressionItemType.ProjectionReference)
                .Require(TokenType.Keyword, "AND")
                .Require(ExpressionItemType.ProjectionReference);
        }

        private void defineColumn()
        {
            registry.Define(ExpressionItemType.Column)
                .OptionalAny(options =>
                {
                    options.Define()
                        .Require(ExpressionItemType.TableReference, ExpressionItemType.SelectCombiner, ExpressionItemType.SelectCommand)
                        .Require(TokenType.Dot);
                })
                .Require(ExpressionItemType.Token);
        }

        private void defineJoin()
        {
            registry.Define(ExpressionItemType.Join)
                .RequireAny(options =>
                {
                    options.Define()
                        .Require(TokenType.LeftParenthesis)
                        .Require(ExpressionItemType.Join)
                        .Require(TokenType.RightParenthesis);
                    options.Define()
                        .Require(ExpressionItemType.Join, ExpressionItemType.SelectCombiner, ExpressionItemType.SelectCommand, ExpressionItemType.TableDeclaration)
                        .Require(TokenType.JoinType)
                        .Require(ExpressionItemType.SelectCombiner, ExpressionItemType.SelectCommand, ExpressionItemType.TableDeclaration)
                        .OptionalAny(inner =>
                        {
                            inner.Define()
                                .Require(TokenType.Keyword, "ON")
                                .Require(ExpressionItemType.Filter);
                        });
                });
        }

        private void defineProjectionDeclaration()
        {
            registry.Define(ExpressionItemType.ProjectionDeclaration)
                .Require(ExpressionItemType.Arithmetic, ExpressionItemType.Column, ExpressionItemType.Function, ExpressionItemType.SelectCommand, ExpressionItemType.Token)
                .OptionalAny(options =>
                {
                    options.Define()
                        .Optional(TokenType.AliasIndicator)
                        .Require(TokenType.Alias);
                });
        }

        private void defineProjectionItemList()
        {
            registry.Define(ExpressionItemType.ProjectionItemList)
                .Require(ExpressionItemType.ProjectionDeclaration)
                .Require(TokenType.Comma)
                .RequireAny(options =>
                {
                    options.Define().Require(ExpressionItemType.ProjectionItemList);
                    options.Define().Require(ExpressionItemType.ProjectionDeclaration);
                });
        }

        private void defineSelectCommand()
        {
            registry.Define(ExpressionItemType.SelectCommand)
                .Require(TokenType.Keyword, "SELECT")
                .Optional(TokenType.Keyword, "DISTINCT")
                .Optional(ExpressionItemType.Top)
                .Require(ExpressionItemType.ProjectionDeclaration, ExpressionItemType.ProjectionItemList)
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require(TokenType.Keyword, "FROM")
                        .Require(
                            ExpressionItemType.FromList,
                            ExpressionItemType.Join,
                            ExpressionItemType.SelectCombiner,
                            ExpressionItemType.SelectCommand,
                            ExpressionItemType.TableDeclaration);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require(TokenType.Keyword, "WHERE")
                        .Require(ExpressionItemType.Filter);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require(TokenType.Keyword, "GROUP BY")
                        .Require(ExpressionItemType.GroupByList);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require(TokenType.Keyword, "HAVING")
                        .Require(ExpressionItemType.Filter);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require(TokenType.Keyword, "ORDER BY")
                        .Require(ExpressionItemType.OrderByList);
                });
        }

        private void defineTableReference()
        {
            registry.Define(ExpressionItemType.TableReference)
                .OptionalAny(options =>
                {
                    options.Define()
                        .Require(TokenType.SchemaName)
                        .Require(TokenType.Dot);
                })
                .Require(TokenType.TableName);
        }

        private void defineTop()
        {
            registry.Define(ExpressionItemType.Top)
                .Require(TokenType.Keyword, "TOP")
                .Require(ExpressionItemType.Arithmetic)
                .Optional(TokenType.Keyword, "PERCENT")
                .Optional(TokenType.Keyword, "WITH TIES");
        }

        /// <summary>
        /// Gets the command text for the given command using the given options. If
        /// the options are null, the default options will be used.
        /// </summary>
        /// <param name="command">The command to format.</param>
        /// <param name="options">The configuration settings for the command builder.</param>
        /// <returns>The formatted command text.</returns>
        public string GetCommandText(ICommand command, CommandOptions options = null)
        {
            if (options == null)
            {
                options = new CommandOptions();
            }
            IExpressionItem commandExpression = command.GetCommandExpression(options);
            StringBuilder builder = new StringBuilder();
            using (TextWriter writer = new StringWriter(builder))
            {
                ExpressionDefinition top = registry.Find(ExpressionItemType.None);
                Expression topExpression = new Expression(ExpressionItemType.None);
                topExpression.AddItem(commandExpression);
                Parser parser = new Parser(topExpression);
                bool isMatch = top.IsMatch(parser);
            }
            return builder.ToString();
        }
    }
}
