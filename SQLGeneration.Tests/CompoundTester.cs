using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGeneration.Generators;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests that builder constructs work in tandem.
    /// </summary>
    [TestClass]
    public class CompoundTester
    {
        /// <summary>
        /// We should verify that the right tokens are being generated for a complex SELECT statement.
        /// </summary>
        [TestMethod]
        public void TestSelectBuilder()
        {
            Table table1 = new Table("Table1");
            Table table2 = new Table("Table2");

            SelectBuilder builder = new SelectBuilder();
            builder.AddJoin(
                Join.From(table1, "t1")
                    .InnerJoin(table2, "t2").On(j => j.Sources["t1"].Column("c1").EqualTo(j.Sources["t2"].Column("c1"))));

            builder.AddProjection(builder.Sources["t1"].Column("c1"));
            builder.AddProjection(builder.Sources["t2"].Column("c1"), "t2c1");
            builder.AddProjection(new PlusExpression(new NumericLiteral(1), new NumericLiteral(1)));
            builder.AddProjection(new NumericLiteral(1.1m));
            builder.AddProjection(builder.Sources["t2"].Column("c2"));

            SelectBuilder inner = new SelectBuilder();
            inner.AddProjection(new StringLiteral("hello"));
            builder.AddProjection(inner, "inner");

            builder.AddProjection(new Function("MAX", new NumericLiteral(1), new NumericLiteral(2)), "Max");
            builder.AddProjection(new NullLiteral());

            IFilter filter = new NumericLiteral(1).EqualTo(new NumericLiteral(1));
            filter.WrapInParentheses = true;
            builder.AddWhere(filter, Conjunction.And);

            string output = String.Join(" ", builder.GetCommandTokens(new CommandOptions()));

            SimpleFormatter formatter = new SimpleFormatter();
            string result = formatter.GetCommandText(builder);
        }
    }
}
