using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLGeneration.Tests
{
    /// <summary>
    /// Tests the StringLiteral class.
    /// </summary>
    [TestClass]
    public class StringLiteralTester
    {
        /// <summary>
        /// If a StringLiteral contains single quotes, they should be escaped.
        /// </summary>
        [TestMethod]
        public void TestStringLiteral_ContainsQuotes_EscapesQuotes()
        {
            ILiteral literal = new StringLiteral("It's a nice day isn't it?");
            string actual = literal.GetFullText(new BuilderContext());
            string expected = "'It''s a nice day isn''t it?'";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }

        /// <summary>
        /// If a StringLiteral contains single quotes and is aliased, the quotes should be escaped
        /// and the alias listed.
        /// </summary>
        [TestMethod]
        public void TestStringLiteral_Aliased_ContainsQuotes_EscapesQuotes()
        {
            ILiteral literal = new StringLiteral("It's a nice day isn't it?") { Alias = "Alias" };
            ProjectionItemFormatter formatter = new ProjectionItemFormatter(new BuilderContext());
            string actual = formatter.GetDeclaration(literal);
            string expected = "'It''s a nice day isn''t it?' AS Alias";
            Assert.AreEqual(expected, actual, "The wrong text was generated.");
        }
    }
}
