﻿using NUnit.Framework;
using StructureCommon.DataStructures;
using System;

namespace StructureCommon.Tests.DataStructures
{
    public class TimeList_Tests
    {
        [TestCase("1/1/2018", 0, "1/1/2018", 0, false)]
        public void AnyTests(DateTime firstDate, double firstValue, DateTime secondDate, double secondValue, bool result)
        {

            TimeList newList = new TimeList();
            Assert.AreEqual(result, newList.Any());
        }
    }
}
