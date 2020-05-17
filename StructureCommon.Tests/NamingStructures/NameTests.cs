using NUnit.Framework;
using System.Collections.Generic;
using StructureCommon.Validation;
using StructureCommon.NamingStructures;

namespace StructureCommon.Tests.NamingStructures
{
    [TestFixture]
    public sealed class NameTests
    {
        [Test]
        public void CanCreate()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var name = new Name(surname, forename);

            Assert.AreEqual(forename, name.SecondaryName);
            Assert.AreEqual(surname, name.PrimaryName);
        }

        [Test]
        public void CanEdit()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var name = new Name(surname, forename);

            Assert.AreEqual(forename, name.SecondaryName);
            Assert.AreEqual(surname, name.PrimaryName);

            name.EditName("Smith", "Steve");
            Assert.AreEqual("Steve", name.SecondaryName);
            Assert.AreEqual("Smith", name.PrimaryName);
        }

        [TestCase("Bloggs", "Joe", "Bloggs", "Joe", true)]
        [TestCase("Bloggs", "Joe", "Bloggs", "Mark", false)]
        [TestCase("Bloggs", "Joe", "Simon", "Joe", false)]
        [TestCase("Bloggs", "Joe", "Simth", "Alan", false)]
        [TestCase("Bloggs", "Joe", "Bloggs", null, false)]
        [TestCase("Bloggs", "Joe", null, "Joe", false)]
        [TestCase("Bloggs", "Joe", null, null, false)]
        [TestCase("Bloggs", null, "Bloggs", "Joe", false)]
        [TestCase(null, "Joe", "Bloggs", "Joe", false)]
        [TestCase(null, null, "Bloggs", "Joe", false)]
        [TestCase("Bloggs", null, "Bloggs", null, true)]
        [TestCase(null, "Joe", null, "Joe", true)]
        [TestCase(null, null, null, null, true)]
        public void EqualityCorrect(string surname, string forename, string testingSurname, string testingForename, bool expected)
        {
            var player = new Name(surname, forename);
            Assert.AreEqual(expected, player.Equals(new Name(testingSurname, testingForename)));
        }

        [Test]
        public void ToStringCorrect()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var name = new Name(surname, forename);

            Assert.AreEqual(forename, name.SecondaryName);
            Assert.AreEqual(surname, name.PrimaryName);
            Assert.AreEqual(forename + " " + surname, name.ToString());
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Bloggs", "Joe", true)]
        [TestCase("Bloggs", "", false)]
        [TestCase("", "Joe", false)]
        [TestCase("Bloggs", null, false)]
        [TestCase(null, "Joe", false)]
        [TestCase(null, null, false)]
        public void TestValidity(string surname, string forename, bool isValid)
        {
            var name = new Name(surname, forename);
            var valid = name.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase("Smith", "Steve", true, new string[] { })]
        [TestCase("Bloggs", "Joe", true, new string[] { })]
        [TestCase("Bloggs", "", false, new string[] { "SecondaryName cannot be empty or null." })]
        [TestCase("", "Joe", false, new string[] { "PrimaryName cannot be empty or null." })]
        [TestCase("Bloggs", null, false, new string[] { "SecondaryName cannot be empty or null." })]
        [TestCase(null, "Joe", false, new string[] { "PrimaryName cannot be empty or null." })]
        public void TestValidityMessage(string surname, string forename, bool isValid, string[] isValidMessage)
        {
            var name = new Name(surname, forename);
            var valid = name.Validation();
            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult
                {
                    IsValid = isValid
                };
                expected.Messages.AddRange(isValidMessage);
                expectedList.Add(expected);
            }

            Assertions.ValidationListsEqual(expectedList, valid);
        }

        [Test]
        public void TestValidityMessageBothNamesNull()
        {
            var name = new Name(null, null);
            var valid = name.Validation();
            var expectedList = new List<ValidationResult>();
            var expectedSurnameError = new ValidationResult
            {
                IsValid = false
            };
            expectedSurnameError.Messages.AddRange(new string[] { "PrimaryName cannot be empty or null." });
            expectedList.Add(expectedSurnameError);

            var expectedForenameError = new ValidationResult
            {
                IsValid = false
            };
            expectedForenameError.Messages.AddRange(new string[] { "SecondaryName cannot be empty or null." });
            expectedList.Add(expectedForenameError);

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
