using NUnit.Framework;
using Common.Structure.NamingStructures;
using Common.UI.Converters;

namespace Common.UI.Tests.Converters
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
            Name name = new Name(surname, forename);

            NameToStringConverter converter = new NameToStringConverter();

            object converted = converter.Convert(name, null, null, null);
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
            Name name = new Name(surname, forename);

            NameToStringConverter converter = new NameToStringConverter();

            //This test is broken because it required a specific ToSTring that has changed
            // when moving into this solution. Need to rewrite test to fix.
            object converted = converter.ConvertBack(name.ToString(), null, null, null);
            Assertions.NamesEqual(name, (Name)converted);
        }
    }
}
