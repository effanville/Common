﻿using System;

using Effanville.Common.Structure.Validation;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.Validation
{
    [TestFixture]
    public sealed class ValidatingTests
    {
        [TestCase(1, "property", true, new string[] { })]
        [TestCase(-1, "property", false, new string[] { "property cannot take a negative value." })]
        [TestCase(0, "number", true, new string[] { })]
        public void NotNegativeValidation(double value, string property, bool result, string[] messages)
        {
            ValidationResult expected = null;
            if (!result)
            {
                expected = new ValidationResult
                {
                    IsValid = result
                };
                expected.Messages.AddRange(messages);
            }

            ValidationResult valid = Validating.NotNegative(value, property, null);

            Assert.That(expected, Is.EqualTo(valid));
        }

        [TestCase(1, 0, "property", true, new string[] { })]
        [TestCase(-1, 0, "property", false, new string[] { "property cannot take values below 0." })]
        [TestCase(0, 0, "number", true, new string[] { })]
        [TestCase(10, 9.2, "property", true, new string[] { })]
        [TestCase(7.29, 7.3, "property", false, new string[] { "property cannot take values below 7.3." })]
        [TestCase(-23.258, -23.258, "number", true, new string[] { })]
        [TestCase(Math.PI, Math.PI, "number", true, new string[] { })]
        public void NotLessThanValidation(double value, double limit, string property, bool result, string[] messages)
        {
            ValidationResult expected = null;
            if (!result)
            {
                expected = new ValidationResult
                {
                    IsValid = result
                };
                expected.Messages.AddRange(messages);
            }

            ValidationResult valid = Validating.NotLessThan(value, limit, property, null);

            Assert.That(expected, Is.EqualTo(valid));
        }

        [TestCase(-1, 0, "property", true, new string[] { })]
        [TestCase(1, 0, "property", false, new string[] { "property cannot take values above 0." })]
        [TestCase(0, 0, "number", true, new string[] { })]
        [TestCase(8, 9.2, "property", true, new string[] { })]
        [TestCase(7.31, 7.3, "property", false, new string[] { "property cannot take values above 7.3." })]
        [TestCase(-23.258, -23.258, "number", true, new string[] { })]
        [TestCase(Math.PI, Math.PI, "number", true, new string[] { })]
        public void NotGreaterThanValidation(double value, double limit, string property, bool result, string[] messages)
        {
            ValidationResult expected = null;
            if (!result)
            {
                expected = new ValidationResult
                {
                    IsValid = result
                };
                expected.Messages.AddRange(messages);
            }

            ValidationResult valid = Validating.NotGreaterThan(value, limit, property, null);

            Assert.That(expected, Is.EqualTo(valid));
        }

        [TestCase(-1, 0, "property", false, new string[] { "property was expected to be equal to 0." })]
        [TestCase(1, 0, "property", false, new string[] { "property was expected to be equal to 0." })]
        [TestCase(0, 0, "number", true, new string[] { })]
        [TestCase(8, 9.2, "property", false, new string[] { "property was expected to be equal to 9.2." })]
        [TestCase(7.31, 7.3, "property", false, new string[] { "property was expected to be equal to 7.3." })]
        [TestCase(-23.258, -23.258, "number", true, new string[] { })]
        [TestCase(Math.PI, Math.PI, "number", true, new string[] { })]
        public void NotEqualToValidation(double value, double equality, string property, bool result, string[] messages)
        {
            ValidationResult expected = null;
            if (!result)
            {
                expected = new ValidationResult
                {
                    IsValid = result
                };
                expected.Messages.AddRange(messages);
            }

            ValidationResult valid = Validating.NotEqualTo(value, equality, property, null);

            Assert.That(expected, Is.EqualTo(valid));
        }

        [TestCase("hello", "property", true, new string[] { })]
        [TestCase("", "property", false, new string[] { "property cannot be empty or null." })]
        [TestCase(null, "property", false, new string[] { "property cannot be empty or null." })]
        public void NotNullOrEmptyValidation(string value, string property, bool result, string[] messages)
        {
            ValidationResult expected = null;
            if (!result)
            {
                expected = new ValidationResult
                {
                    IsValid = result
                };
                expected.Messages.AddRange(messages);
            }

            ValidationResult valid = Validating.IsNotNullOrEmpty(value, property, null);

            Assert.That(expected, Is.EqualTo(valid));
        }
    }
}
