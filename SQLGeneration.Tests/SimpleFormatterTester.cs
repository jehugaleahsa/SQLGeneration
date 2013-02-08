using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGeneration.Builders;
using SQLGeneration.Generators;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the SimpleFormatter class.
    /// </summary>
    [TestClass]
    public class SimpleFormatterTester
    {
        #region AllColumns

        /// <summary>
        /// We can get all of the columns from a source by using the star symbol.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_Unqualified()
        {
            SelectBuilder builder = new SelectBuilder();
            builder.AddProjection(new AllColumns());
            builder.AddTable(new Table("Table"));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT * FROM Table";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We can get all of the columns from a specific source by using the star symbol
        /// and passing it the source.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_Qualified_NotAliased()
        {
            SelectBuilder builder = new SelectBuilder();
            AliasedSource source = builder.AddTable(new Table("Table"));
            builder.AddProjection(new AllColumns(source));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.* FROM Table";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We can get all of the columns from a specific source by using the star symbol
        /// and passing it the source. If that source is aliased, the alias will be used to
        /// qualify the star.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_Qualified_Aliased()
        {
            SelectBuilder builder = new SelectBuilder();
            AliasedSource source = builder.AddTable(new Table("Table"), "t");
            builder.AddProjection(new AllColumns(source));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT t.* FROM Table t";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We can get all of the columns from a specific source by using the star symbol
        /// and passing it the source. If that source has a namespace, it should appear as
        /// well.
        /// </summary>
        [TestMethod]
        public void TestAllColumns_Qualified_Namespaced()
        {
            SelectBuilder builder = new SelectBuilder();
            Namespace schema = new Namespace("LocalServer", "Owner", "Database");
            Table table = new Table(schema, "Table");
            AliasedSource source = builder.AddTable(table);
            builder.AddProjection(new AllColumns(source));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT LocalServer.Owner.Database.Table.* FROM LocalServer.Owner.Database.Table";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        #endregion

        #region Column

        #region SelectBuilder

        /// <summary>
        /// If we project a column in a SELECT statement without specifying an alias,
        /// it should appear qualified with the table name.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsProjection_NoAlias_Qualified()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table);
            Column column = source.Column("Column");
            builder.AddProjection(column);
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.Column FROM Table";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we project a column in a SELECT statement and specify an alias,
        /// it should appear qualified by the table name and with the alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsProjection_WithAlias_QualifiedAnedAliased()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table);
            Column column = source.Column("Column");
            builder.AddProjection(column, "c");
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.Column AS c FROM Table";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we project a column in a SELECT statement whose table is aliased,
        /// it should appear qualified by the table alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsProjection_TableAliased_QualifiedWithTableAlias()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table, "t");
            Column column = source.Column("Column");
            builder.AddProjection(column);
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT t.Column FROM Table t";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we project an aliased column in a SELECT statement whose table is aliased,
        /// it should appear qualified by the table alias along with its alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsProjection_ColumnAndTableAliased_QualifiedWithTableAliasAndAliased()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table, "t");
            Column column = source.Column("Column");
            builder.AddProjection(column, "c");
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT t.Column AS c FROM Table t";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We should be able to ORDER BY a column without using its alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsOrderItem_ReferenceWithoutAlias()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table);
            Column column = source.Column("Column");
            builder.AddProjection(column, "c");
            builder.AddOrderBy(new OrderBy(column));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.Column AS c FROM Table ORDER BY Table.Column";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We should be able to ORDER BY a column using its alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsOrderItem_ReferenceWithAlias()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table);
            Column column = source.Column("Column");
            AliasedProjection aliased = builder.AddProjection(column, "c");
            builder.AddOrderBy(new OrderBy(aliased));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.Column AS c FROM Table ORDER BY c";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We should be able to filter using a column. It isn't possible to
        /// refer to an alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_WhereItem_NoAlias()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table);
            Column column = source.Column("Column");
            builder.AddProjection(column);
            builder.AddWhere(new EqualToFilter(column, new NumericLiteral(1)));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.Column FROM Table WHERE Table.Column = 1";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We should be able to filter using a column. It isn't possible to
        /// refer to an alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_GroupByItem_HavingItem_NoAlias()
        {
            SelectBuilder builder = new SelectBuilder();
            Table table = new Table("Table");
            AliasedSource source = builder.AddTable(table);
            Column column = source.Column("Column");
            builder.AddProjection(column);
            builder.AddGroupBy(column);
            builder.AddHaving(new EqualToFilter(column, new NumericLiteral(1)));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT Table.Column FROM Table GROUP BY Table.Column HAVING Table.Column = 1";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// We should be able to join two tables using columns in the ON filter.
        /// </summary>
        [TestMethod]
        public void TestColumn_AsJoinFilter_NoAlias()
        {
            SelectBuilder builder = new SelectBuilder();
            FilteredJoin join = Join.From(new Table("Table1"), "t1")
                .InnerJoin(new Table("Table2"), "t2");
            builder.AddJoin(join);
            AliasedSource table1Source = builder.Sources["t1"];
            AliasedSource table2Source = builder.Sources["t2"];
            Column column1 = table1Source.Column("Column");
            Column column2 = table2Source.Column("Column");
            join.On(j => new EqualToFilter(column1, column2));
            builder.AddProjection(column1);
            builder.AddProjection(column2);
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "SELECT t1.Column, t2.Column FROM Table1 t1 INNER JOIN Table2 t2 ON t1.Column = t2.Column";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we have a sub-select, it should be able to reference columns
        /// from the outer select statement (maybe).
        /// </summary>
        [TestMethod]
        public void TestColumn_NestedTable_ReferenceOuterColumn()
        {
            SelectBuilder outer = new SelectBuilder();
            AliasedSource outerSource = outer.AddTable(new Table("Customer"), "o");
            Column outerColumn = outerSource.Column("CustomerId");
            outer.AddProjection(outerColumn);
            SelectBuilder inner = new SelectBuilder();
            AliasedSource innerSource = inner.AddTable(new Table("Customer"), "i");
            Column innerColumn = innerSource.Column("CustomerId");
            inner.AddProjection(innerColumn);
            inner.AddWhere(new EqualToFilter(outerColumn, innerColumn));
            outer.AddWhere(new InFilter(outerColumn, inner));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(outer);
            string expected = "SELECT o.CustomerId FROM Customer o WHERE o.CustomerId IN (SELECT i.CustomerId FROM Customer i WHERE o.CustomerId = i.CustomerId)";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        #endregion

        #region InsertBuilder

        /// <summary>
        /// If we don't provide and alias for the table, the
        /// column should appear in the column list.
        /// </summary>
        [TestMethod]
        public void TestColumn_ColumnList_NoAlias()
        {
            Table table = new Table("Table");
            ValueList values = new ValueList();
            InsertBuilder builder = new InsertBuilder(table, values);
            AliasedSource tableSource = builder.Table;
            Column column = tableSource.Column("Column");
            builder.AddColumn(column);
            values.AddValue(new NumericLiteral(1));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "INSERT INTO Table (Column) VALUES(1)";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we don't provide and alias for the table and we say to qualify the column, the
        /// column should appear fully-qualified in the column list.
        /// </summary>
        [TestMethod]
        public void TestColumn_ColumnList_NoAlias_Qualified()
        {
            Table table = new Table("Table");
            ValueList values = new ValueList();
            InsertBuilder builder = new InsertBuilder(table, values);
            AliasedSource tableSource = builder.Table;
            Column column = tableSource.Column("Column");
            column.Qualify = true;
            builder.AddColumn(column);
            values.AddValue(new NumericLiteral(1));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "INSERT INTO Table (Table.Column) VALUES(1)";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we specify an alias for the table, the alias should be displayed.
        /// </summary>
        [TestMethod]
        public void TestColumn_ColumnList_TableAliased()
        {
            Table table = new Table("Table");
            ValueList values = new ValueList();
            InsertBuilder builder = new InsertBuilder(table, values, "t");
            AliasedSource tableSource = builder.Table;
            Column column = tableSource.Column("Column");
            builder.AddColumn(column);
            values.AddValue(new NumericLiteral(1));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "INSERT INTO Table t (Column) VALUES(1)";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we specify an alias for the table, and say we want to qualify the column,
        /// the column should be qualified using the alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_ColumnList_TableAliased_Qualified()
        {
            Table table = new Table("Table");
            ValueList values = new ValueList();
            InsertBuilder builder = new InsertBuilder(table, values, "t");
            AliasedSource tableSource = builder.Table;
            Column column = tableSource.Column("Column");
            column.Qualify = true;
            builder.AddColumn(column);
            values.AddValue(new NumericLiteral(1));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "INSERT INTO Table t (t.Column) VALUES(1)";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        #endregion

        #region UpdateBuilder

        /// <summary>
        /// By default, columns are not qualified in update statements.
        /// </summary>
        [TestMethod]
        public void TestColumn_InSetter_NotQualifiedByDefault()
        {
            Table table = new Table("Table");
            UpdateBuilder builder = new UpdateBuilder(table);
            Column column = builder.Table.Column("Column");
            builder.AddSetter(new Setter(column, new StringLiteral("hello")));
            builder.AddWhere(new EqualToFilter(column, new StringLiteral("goodbye")));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "UPDATE Table SET Column = 'hello' WHERE Column = 'goodbye'";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we ask that columns be qualified, they should be preceded by their table names.
        /// </summary>
        [TestMethod]
        public void TestColumn_InSetter_Qualified_IncludeTableName()
        {
            Table table = new Table("Table");
            UpdateBuilder builder = new UpdateBuilder(table);
            Column column = builder.Table.Column("Column");
            column.Qualify = true;
            builder.AddSetter(new Setter(column, new StringLiteral("hello")));
            builder.AddWhere(new EqualToFilter(column, new StringLiteral("goodbye")));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "UPDATE Table SET Table.Column = 'hello' WHERE Table.Column = 'goodbye'";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we ask that columns be qualified and the table is aliased, they should be preceded by the table alias.
        /// </summary>
        [TestMethod]
        public void TestColumn_InSetter_Qualified_TableAliased_IncludeAlias()
        {
            Table table = new Table("Table");
            UpdateBuilder builder = new UpdateBuilder(table, "t");
            Column column = builder.Table.Column("Column");
            column.Qualify = true;
            builder.AddSetter(new Setter(column, new StringLiteral("hello")));
            builder.AddWhere(new EqualToFilter(column, new StringLiteral("goodbye")));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "UPDATE Table t SET t.Column = 'hello' WHERE t.Column = 'goodbye'";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        #endregion

        #region DeleteBuilder

        /// <summary>
        /// By default, columns are not qualified in DELETE statements.
        /// </summary>
        [TestMethod]
        public void TestColumn_InWhere_NotQualifiedByDefault()
        {
            Table table = new Table("Table");
            DeleteBuilder builder = new DeleteBuilder(table);
            Column column = builder.Table.Column("Column");
            builder.AddWhere(new NullFilter(column));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "DELETE FROM Table WHERE Column IS NULL";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we ask for the column to be qualified, it will be qualified
        /// with the table name.
        /// </summary>
        [TestMethod]
        public void TestColumn_InWhere_Qualified_QualifiedWithTableName()
        {
            Table table = new Table("Table");
            DeleteBuilder builder = new DeleteBuilder(table);
            Column column = builder.Table.Column("Column");
            column.Qualify = true;
            builder.AddWhere(new NullFilter(column));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "DELETE FROM Table WHERE Table.Column IS NULL";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        /// <summary>
        /// If we ask for the column to be qualified and the table has an alias, 
        /// it will be qualified with the table name.
        /// </summary>
        [TestMethod]
        public void TestColumn_InWhere_Qualified_TableAliased_QualifiedWithAlias()
        {
            Table table = new Table("Table");
            DeleteBuilder builder = new DeleteBuilder(table, "t");
            Column column = builder.Table.Column("Column");
            column.Qualify = true;
            builder.AddWhere(new NullFilter(column));
            SimpleFormatter formatter = new SimpleFormatter();
            string actual = formatter.GetCommandText(builder);
            string expected = "DELETE FROM Table t WHERE t.Column IS NULL";
            Assert.AreEqual(expected, actual, "The wrong SQL was generated.");
        }

        #endregion

        #endregion
    }
}
