﻿using System;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DisplayClasses
{
    [TestFixture]
    public class DayValue_NamedTests
    {
        [TestCase("name", "company", "name", "company", 0)]
        [TestCase("name", "company", "name", "ompany", -1)]
        [TestCase("name", "ompany", "name", "company", 1)]
        [TestCase("ame", "company", "name", "company", -1)]
        [TestCase("name", "company", "ame", "company", 1)]
        public void ComparisonCorrect(string name1, string company1, string name2, string company2, int expected)
        {
            Labelled<Name, DailyValuation> one = new Labelled<Name, DailyValuation>(new Name(company1, name1), new DailyValuation(new DateTime(), 0));
            Labelled<Name, DailyValuation> two = new Labelled<Name, DailyValuation>(new Name(company2, name2), new DailyValuation(new DateTime(), 0));
            Assert.That(one.CompareTo(two), Is.EqualTo(expected));
        }

        [TestCase("name", "company", "12/5/2019", 5, "company-name-05/12/2019, 5")]
        [TestCase(null, "company", "12/5/2019", 5, "company-05/12/2019, 5")]
        [TestCase("name", null, "12/5/2019", 5, "-name-05/12/2019, 5")]
        [TestCase("name", "company", "12/5/2019", 0, "company-name-05/12/2019, 0")]
        [TestCase("", "company", "12/5/2019", 5, "company-05/12/2019, 5")]
        public void ToStringTests(string name1, string company1, DateTime date, decimal value, string expected)
        {
            Labelled<Name, DailyValuation> one = new Labelled<Name, DailyValuation>(new Name(company1, name1), new DailyValuation(date, value));
            Assert.That(one.ToString(), Is.EqualTo(expected));
        }
    }
}
