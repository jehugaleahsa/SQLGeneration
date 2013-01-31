using System;
using System.Collections.Generic;
using SQLGeneration.Expressions;
using System.Linq;
using System.Text;
using System.IO;

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

        private void defineColumn()
        {
            registry.Define(ExpressionItemType.Column)
                .OptionalAny(options =>
                {
                    options.Define()
                        .Require(ExpressionItemType.TableReference, ExpressionItemType.SelectCombiner, ExpressionItemType.SelectCommand)
                        .Require(".");
                })
                .Require(ExpressionItemType.Token);
        }

        private void defineJoin()
        {
            registry.Define(ExpressionItemType.Join)
                .RequireAny(options =>
                {
                    options.Define()
                        .Require("(")
                        .Require(ExpressionItemType.Join)
                        .Require(")");
                    options.Define()
                        .Require(ExpressionItemType.Join, ExpressionItemType.SelectCombiner, ExpressionItemType.SelectCommand, ExpressionItemType.TableDeclaration)
                        .RequireAny(inner =>
                        {
                            inner.Define().Require("JOIN");
                            inner.Define().Require("INNER JOIN");
                            inner.Define().Require("LEFT JOIN");
                            inner.Define().Require("LEFT OUTER JOIN");
                            inner.Define().Require("RIGHT JOIN");
                            inner.Define().Require("RIGHT OUTER JOIN");
                            inner.Define().Require("FULL JOIN");
                            inner.Define().Require("FULL OUTER JOIN");
                            inner.Define().Require("CROSS JOIN");
                        })
                        .Require(ExpressionItemType.SelectCombiner, ExpressionItemType.SelectCommand, ExpressionItemType.TableDeclaration)
                        .OptionalAny(inner =>
                        {
                            inner.Define()
                                .Require("ON")
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
                        .Require("AS")
                        .Require(ExpressionItemType.Token);
                    options.Define()
                        .Require(ExpressionItemType.Token);
                });
        }

        private void defineProjectionItemList()
        {
            registry.Define(ExpressionItemType.ProjectionItemList)
                .Require(ExpressionItemType.ProjectionDeclaration)
                .Require(",")
                .RequireAny(options =>
                {
                    options.Define().Require(ExpressionItemType.ProjectionItemList);
                    options.Define().Require(ExpressionItemType.ProjectionDeclaration);
                });
        }

        private void defineSelectCommand()
        {
            registry.Define(ExpressionItemType.SelectCommand)
                .Require("SELECT")
                .Optional("DISTINCT")
                .Optional(ExpressionItemType.Top)
                .Require(ExpressionItemType.ProjectionDeclaration, ExpressionItemType.ProjectionItemList)
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require("FROM")
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
                        .Require("WHERE")
                        .Require(ExpressionItemType.Filter);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require("GROUP BY")
                        .Require(ExpressionItemType.GroupByList);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require("HAVING")
                        .Require(ExpressionItemType.Filter);
                })
                .OptionalAny(option =>
                {
                    option.Define()
                        .Require("ORDER BY")
                        .Require(ExpressionItemType.OrderByList);
                });
        }

        private void defineTableReference()
        {
            registry.Define(ExpressionItemType.TableReference)
                .OptionalAny(options =>
                {
                    options.Define()
                        .Require(ExpressionItemType.Token)
                        .Require(".");
                })
                .Require(ExpressionItemType.Token);
        }

        private void defineTop()
        {
            registry.Define(ExpressionItemType.Top)
                .Require("TOP")
                .Require(ExpressionItemType.Arithmetic, ExpressionItemType.Token)
                .Optional("PERCENT")
                .Optional("WITH TIES");
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
