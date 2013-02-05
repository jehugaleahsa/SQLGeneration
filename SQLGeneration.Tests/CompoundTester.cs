using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGeneration.Parsing;
using System.Text;
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

            string output = String.Join(" ", builder.GetCommandTokens(new CommandOptions()));

            SimpleFormatter formatter = new SimpleFormatter();
            string result = formatter.GetCommandText(builder);
        }
    }
}
