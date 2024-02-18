using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;

namespace Effanville.Common.Structure.Tests.DataStructures.Money
{
    public static class TimeListTestData
    {
        public const string NullListKey = "NullTimeList";
        public const string EmptyListKey = "EmptyTimeList";
        public const string SingleEntryKey = "SingleEntry";
        public const string SingleEntryZeroValueKey = "SingleEntryZeroValue";
        public const string TwoEntryKey = "TwoEntry";
        public const string TwoEntryKey2 = "TwoEntry2";
        public const string TwoEntryZeroValuesKey = "TwoEntryZeroValues";
        public const string ThreeEntryKey1 = "ThreeEntry1";
        public const string ThreeEntryKey2 = "ThreeEntry2";
        public const string FourEntryKey = "FourEntry";
        public const string FourEntryKey2 = "FourEntry2";
        public const string FourEntryZeroValuesKey = "FourEntryZeroValues";
        public const string TenEntryKey = "TenEntry";
        public const string HundredEntryKey = "HundredEntry";
        public const string ThousandEntryKey = "ThousandEntry";
        public const string TenThousandEntryKey = "TenThousandEntry";

        public static string[] AdmissibleKeys => new[]
        {
            NullListKey,
            EmptyListKey,
            SingleEntryKey,
            TwoEntryKey,
            TwoEntryKey2,
            TwoEntryZeroValuesKey,
            ThreeEntryKey1,
            ThreeEntryKey2,
        };

        private static Dictionary<string, TimeList> fExampleData;

        private static void AddData(TimeList sut, int numberEntriesToAdd, DateTime startDate, decimal startValue, int dayIncrement, decimal valueIncrement)
        {
            DateTime latestDate = startDate;
            decimal latestValue = startValue;
            for (int i = 0; i < numberEntriesToAdd; i++)
            {
                sut.SetData(latestDate, latestValue);
                latestDate = latestDate.AddDays(dayIncrement);
                latestValue += valueIncrement;
            }
        }

        private static void SetupTestLists()
        {
            fExampleData = new Dictionary<string, TimeList>
            {
                { NullListKey, null },
                { EmptyListKey, new TimeList() }
            };

            TimeList tl0 = new TimeList();
            tl0.SetData(new DateTime(2018, 1, 1), 0.0m);
            fExampleData.Add(SingleEntryZeroValueKey, tl0);

            TimeList tl1 = new TimeList();
            tl1.SetData(new DateTime(2018, 1, 1), 1000);
            fExampleData.Add(SingleEntryKey, tl1);

            TimeList tl2 = new TimeList();
            tl2.SetData(new DateTime(2018, 1, 1), 1000);
            tl2.SetData(new DateTime(2018, 6, 1), 1250);
            fExampleData.Add(TwoEntryKey, tl2);

            var timelist = new TimeList();
            timelist.SetData(new DateTime(2012, 8, 31), 824.59m);
            timelist.SetData(new DateTime(2019, 12, 1), 824.594m);
            fExampleData.Add(TwoEntryKey2, timelist);

            TimeList tl9 = new TimeList();
            tl9.SetData(new DateTime(2018, 1, 1), 0.0m);
            tl9.SetData(new DateTime(2019, 1, 1), 0.0m);
            fExampleData.Add(TwoEntryZeroValuesKey, tl9);

            TimeList tl3 = new TimeList();
            tl3.SetData(new DateTime(2017, 1, 1), 1000);
            tl3.SetData(new DateTime(2018, 1, 1), 1100);
            tl3.SetData(new DateTime(2018, 6, 1), 1200);
            fExampleData.Add(ThreeEntryKey1, tl3);

            TimeList tl4 = new TimeList();
            tl4.SetData(new DateTime(2017, 1, 1), 1000);
            tl4.SetData(new DateTime(2018, 1, 1), -1100);
            tl4.SetData(new DateTime(2018, 6, 1), 1200);
            fExampleData.Add(ThreeEntryKey2, tl4);

            TimeList tl10 = new TimeList();
            tl10.SetData(new DateTime(2018, 1, 1), 0.0m);
            tl10.SetData(new DateTime(2019, 1, 1), 0.0m);
            tl10.SetData(new DateTime(2019, 5, 1), 0.0m);
            tl10.SetData(new DateTime(2019, 5, 5), 0.0m);
            fExampleData.Add(FourEntryZeroValuesKey, tl10);

            TimeList tl11 = new TimeList();
            tl11.SetData(new DateTime(2018, 1, 1), 0.0m);
            tl11.SetData(new DateTime(2019, 1, 1), 0.0m);
            tl11.SetData(new DateTime(2019, 5, 1), 2.0m);
            tl11.SetData(new DateTime(2019, 5, 5), 2.0m);
            fExampleData.Add(FourEntryKey, tl11);

            TimeList tl12 = new TimeList();
            tl12.SetData(new DateTime(2018, 1, 1), 0.0m);
            tl12.SetData(new DateTime(2019, 1, 1), 1.0m);
            tl12.SetData(new DateTime(2019, 5, 1), 2.0m);
            tl12.SetData(new DateTime(2019, 5, 5), 2.0m);
            fExampleData.Add(FourEntryKey2, tl12);

            TimeList tl5 = new TimeList();
            AddData(tl5, 10, new DateTime(2018, 5, 4), 2.0m, 55, 12.2m);
            fExampleData.Add(TenEntryKey, tl5);

            TimeList tl6 = new TimeList();
            AddData(tl6, 100, new DateTime(2014, 5, 4), 2.0m, 55, 12.2m);
            fExampleData.Add(HundredEntryKey, tl6);

            TimeList tl7 = new TimeList();
            AddData(tl7, 1000, new DateTime(2010, 5, 4), 2.0m, 55, 12.2m);
            fExampleData.Add(ThousandEntryKey, tl7);

            TimeList tl8 = new TimeList();
            AddData(tl8, 10000, new DateTime(2000, 5, 4), 2.0m, 55, 12.2m);
            fExampleData.Add(TenThousandEntryKey, tl8);
        }

        public static TimeList GetTestTimeList(string key)
        {
            if (fExampleData == null)
            {
                SetupTestLists();
            }

            return !fExampleData.ContainsKey(key)
                ? throw new KeyNotFoundException($"TimeList with key '{key}' not found.")
                : fExampleData[key].Copy();
        }
    }
}