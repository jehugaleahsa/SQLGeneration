using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the Column class.
    /// </summary>
    [TestClass]
    public class ColumnTester
    {
        /// <summary>
        /// A column name should always appear with its join item name.
        /// </summary>
        [TestMethod]
        public void TestColumn_IProjectionItem_PrintsJoinItemDotName()
        {
            Table table = new Table("Table");
            IProjectionItem column = table.CreateColumn("Column");
            string actual = column.GetFullText(new BuilderContext());
            string expected = "Table.Column";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// A column with an alias should be fully-qualified followed by its alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_IProjectionItem_Aliased_PrintsJoinItemDotNameAsAlias()
        {
            Table table = new Table("Table");
            IProjectionItem column = table.CreateColumn("Column", "Alias");
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetDeclaration(column);
            string expected = "Table.Column AS Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// The optional keyword AS should be excluded if the options say to skip it.
        /// </summary>
        [TestMethod]
        public void TestColumn_IProjectionItem_Aliased_NoAsOption_PrintsJoinItemDotNameAlias()
        {
            Table table = new Table("Table");
            IProjectionItem column = table.CreateColumn("Column", "Alias");
            BuilderContext context = new BuilderContext();
            context.Options.AliasProjectionsUsingAs = false;
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(context);
            string actual = formatter.GetDeclaration(column);
            string expected = "Table.Column Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// When an aliased column is referenced in the ORDER BY clause, it's alias should be printed.
        /// </summary>
        [TestMethod]
        public void TestColumn_IProjectionItem_Aliased_PrintsAlias()
        {
            Table table = new Table("Table");
            IProjectionItem column = table.CreateColumn("Column", "Alias");
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetAliasedReference(column);
            string expected = "Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// When an aliased column is referenced in a WHERE, GROUP BY or HAvING clause the alias should be ignored.
        /// </summary>
        [TestMethod]
        public void TestColumn_IProjectionItem_Aliased_PrintsFullyQualified()
        {
            Table table = new Table("Table");
            IProjectionItem column = table.CreateColumn("Column", "Alias");
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetUnaliasedReference(column);
            string expected = "Table.Column";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }
    }
}
