using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the DivideExpression class.
    /// </summary>
    [TestClass]
    public class DivideExpressionTester
    {
        /// <summary>
        /// Two values should be separated by a '/' characted.
        /// </summary>
        [TestMethod]
        public void TestDivide_PrintsLHSSlashRHS()
        {
            IProjectionItem expression = new DivideExpression(new NumericLiteral(1), new NumericLiteral(2)) { WrapInParentheses = false };
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetDeclaration(expression);
            string expected = "1 / 2";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// The expression should be wrapped in parentheses if the options is set.
        /// </summary>
        [TestMethod]
        public void TestDivide_Parens_PrintsLHSSlashRHS()
        {
            IProjectionItem expression = new DivideExpression(new NumericLiteral(1), new NumericLiteral(2)) { WrapInParentheses = true };
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetDeclaration(expression);
            string expected = "(1 / 2)";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// The expression should be followed by its alias when it is listed in the projection.
        /// </summary>
        [TestMethod]
        public void TestDivide_Aliased_InProjection_PrintsLHSSlashRHSASAlias()
        {
            IProjectionItem expression = new DivideExpression(new NumericLiteral(1), new NumericLiteral(2));
            expression.Alias = "Alias";
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetDeclaration(expression);
            string expected = "(1 / 2) AS Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// The wrapped expression should be followed by its alias when it is listed in the projection with parentheses enabled.
        /// </summary>
        [TestMethod]
        public void TestDivide_Aliased_NoParens_InProjection_PrintsLHSSlashRHSASAlias()
        {
            IProjectionItem expression = new DivideExpression(new NumericLiteral(1), new NumericLiteral(2)) { WrapInParentheses = false };
            expression.Alias = "Alias";
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetDeclaration(expression);
            string expected = "1 / 2 AS Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// The alias should be ignored if the expression is in the WHERE, GROUP BY or HAVING sections.
        /// </summary>
        [TestMethod]
        public void TestDivide_Aliased_InWhere_PrintsLHSSlashRHS()
        {
            IProjectionItem expression = new DivideExpression(new NumericLiteral(1), new NumericLiteral(2));
            expression.Alias = "Alias";
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetUnaliasedReference(expression);
            string expected = "(1 / 2)";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// The alias should be printed if the expression is in the ORDER BY section.
        /// </summary>
        [TestMethod]
        public void TestDivide_Aliased_InOrderBy_PrintsAlias()
        {
            IProjectionItem expression = new DivideExpression(new NumericLiteral(1), new NumericLiteral(2));
            expression.Alias = "Alias";
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetAliasedReference(expression);
            string expected = "Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        // TODO - use global setting for wrapping arithmetic expression
    }
}
