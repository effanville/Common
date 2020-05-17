using NUnit.Framework;
using StructureCommon.NamingStructures;
using UICommon.Converters;

namespace UICommon.Tests.Converters
{
    [TestFixture]
    public sealed class PlayerNameToStringTests
    {
        [TestCase("Bloggs", "Joe")]
        [TestCase("Bloggs", "")]
        [TestCase("", "Joe")]
        [TestCase("Bloggs", null)]
        [TestCase(null, "Joe")]
        [TestCase(null, null)]
        public void ConvertCorrectly(string surname, string forename)
        {
            var name = new Name(surname, forename);

            var converter = new NameToStringConverter();

            var converted = converter.Convert(name, null, null, null);
            Assert.AreEqual(name.ToString(), converted);
        }

        [TestCase("Bloggs", "Joe")]
        [TestCase("Bloggs", "")]
        [TestCase("", "Joe")]
        [TestCase("Bloggs", null)]
        [TestCase(null, "Joe")]
        [TestCase(null, null)]
        public void ConvertBackCorrectly(string surname, string forename)
        {
            var name = new Name(surname, forename);

            var converter = new NameToStringConverter();

            //This test is broken because it required a specific ToSTring that has changed
            // when moving into this solution. Need to rewrite test to fix.
            var converted = converter.ConvertBack(name.ToString(), null, null, null);
            Assertions.NamesEqual(name, (Name)converted);
        }
    }
}
