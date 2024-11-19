using System.Collections.Generic;

using Effanville.Common.Structure.NamingStructures;
using Effanville.Common.Structure.Validation;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.NamingStructures
{
    [TestFixture]
    public sealed class NameTests
    {
        [Test]
        public void CanCreate()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            Name name = new Name(surname, forename);

            Assert.That(name.SecondaryName, Is.EqualTo(forename));
            Assert.That(name.PrimaryName, Is.EqualTo(surname));
        }

        [Test]
        public void CanEdit()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            Name name = new Name(surname, forename);

            Assert.That(name.SecondaryName, Is.EqualTo(forename));
            Assert.That(name.PrimaryName, Is.EqualTo(surname));

            name.EditName("Smith", "Steve");
            Assert.That(name.SecondaryName, Is.EqualTo("Steve"));
            Assert.That(name.PrimaryName, Is.EqualTo("Smith"));
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
            Name player = new Name(surname, forename);
            Assert.That(player.Equals(new Name(testingSurname, testingForename)), Is.EqualTo(expected));
        }

        [Test]
        public void ToStringCorrect()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            Name name = new Name(surname, forename);

            Assert.That(name.SecondaryName, Is.EqualTo(forename));
            Assert.That(name.PrimaryName, Is.EqualTo(surname));
            Assert.That(name.ToString(), Is.EqualTo($"{surname}-{forename}"));
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
            Name name = new Name(surname, forename);
            bool valid = name.Validate();
            Assert.That(valid, Is.EqualTo(isValid));
        }

        [TestCase("Smith", "Steve", true, new string[] { })]
        [TestCase("Bloggs", "Joe", true, new string[] { })]
        [TestCase("Bloggs", "", false, new string[] { "SecondaryName cannot be empty or null." })]
        [TestCase("", "Joe", false, new string[] { "PrimaryName cannot be empty or null." })]
        [TestCase("Bloggs", null, false, new string[] { "SecondaryName cannot be empty or null." })]
        [TestCase(null, "Joe", false, new string[] { "PrimaryName cannot be empty or null." })]
        public void TestValidityMessage(string surname, string forename, bool isValid, string[] isValidMessage)
        {
            Name name = new Name(surname, forename);
            List<ValidationResult> valid = name.Validation();
            List<ValidationResult> expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                ValidationResult expected = new ValidationResult
                {
                    IsValid = isValid
                };
                expected.Messages.AddRange(isValidMessage);
                expectedList.Add(expected);
            }

            Assert.That(valid, Is.EqualTo(expectedList).AsCollection);
        }

        [Test]
        public void TestValidityMessageBothNamesNull()
        {
            Name name = new Name(null, null);
            List<ValidationResult> valid = name.Validation();
            List<ValidationResult> expectedList = new List<ValidationResult>();
            ValidationResult expectedSurnameError = new ValidationResult
            {
                IsValid = false
            };
            expectedSurnameError.Messages.AddRange(new string[] { "PrimaryName cannot be empty or null." });
            expectedList.Add(expectedSurnameError);

            ValidationResult expectedForenameError = new ValidationResult
            {
                IsValid = false
            };
            expectedForenameError.Messages.AddRange(new string[] { "SecondaryName cannot be empty or null." });
            expectedList.Add(expectedForenameError);

            Assert.That(valid, Is.EqualTo(expectedList).AsCollection);
        }
    }
}
