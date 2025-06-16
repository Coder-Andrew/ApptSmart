using ApptSmartBackend.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApptSmartBackend_Tests
{
    internal class SlugifyTests
    {
        [TestCase("John's Pizza & Subs", "johns-pizza-subs")]
        [TestCase(" Clean Code Crafters ", "clean-code-crafters")]
        [TestCase("C# Developers 101!!!", "c-developers-101")]
        [TestCase("AI & Machine Learning - 2025", "ai-machine-learning-2025")]
        [TestCase("L'été en Provence", "lete-en-provence")]
        [TestCase("", "")]
        public void SlufifyHelper_Returns_Expected_Output(string input, string output)
        {
            // Arrange
            // Act
            string expected = SlugHelper.Slugify(input);
            // Assert

            Assert.That(expected, Is.EqualTo(output));
        }
    }
}
