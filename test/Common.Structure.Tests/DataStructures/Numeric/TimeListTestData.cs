using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures.Numeric;

namespace Effanville.Common.Structure.Tests.DataStructures.Numeric
{
    public static class TimeListTestData
    {
        public const string NullListKey = "NullTimeList";
        public const string EmptyListKey = "EmptyTimeList";
        public const string SingleEntryKey = "SingleEntry";
        public const string TwoEntryKey = "TwoEntry";
        public const string ThreeEntryKey1 = "ThreeEntry1";
        public const string ThreeEntryKey2 = "ThreeEntry2";
        public const string FourEntrySameDayKey = "FourEntrySameDay";
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
            ThreeEntryKey1,
            ThreeEntryKey2,
        };

        private static Dictionary<string, TimeNumberList> fExampleData;

        private static void AddData(TimeNumberList sut, int numberEntriesToAdd, DateTime startDate, double startValue, int dayIncrement, double valueIncrement)
        {
            DateTime latestDate = startDate;
            double latestValue = startValue;
            for (int i = 0; i < numberEntriesToAdd; i++)
            {
                sut.SetData(latestDate, latestValue);
                latestDate = latestDate.AddDays(dayIncrement);
                latestValue += valueIncrement;
            }
        }

        private static void SetupTestLists()
        {
            fExampleData = new Dictionary<string, TimeNumberList>
            {
                { NullListKey, null },
                { EmptyListKey, new TimeNumberList() }
            };

            TimeNumberList tl1 = new TimeNumberList();
            tl1.SetData(new DateTime(2018, 1, 1), 1000);
            fExampleData.Add(SingleEntryKey, tl1);

            TimeNumberList tl2 = new TimeNumberList();
            tl2.SetData(new DateTime(2018, 1, 1), 1000);
            tl2.SetData(new DateTime(2018, 6, 1), 1250);
            fExampleData.Add(TwoEntryKey, tl2);

            TimeNumberList tl3 = new TimeNumberList();
            tl3.SetData(new DateTime(2017, 1, 1), 1000);
            tl3.SetData(new DateTime(2018, 1, 1), 1100);
            tl3.SetData(new DateTime(2018, 6, 1), 1200);
            fExampleData.Add(ThreeEntryKey1, tl3);

            TimeNumberList tl4 = new TimeNumberList();
            tl4.SetData(new DateTime(2017, 1, 1), 1000);
            tl4.SetData(new DateTime(2018, 1, 1), -1100);
            tl4.SetData(new DateTime(2018, 6, 1), 1200);
            fExampleData.Add(ThreeEntryKey2, tl4);

            TimeNumberList tlSameDay = new TimeNumberList();
            tlSameDay.SetData(new DateTime(2018, 1, 1), 0.0);
            tlSameDay.SetData(new DateTime(2019, 1, 1,2,4, 5), 1.0);
            tlSameDay.SetData(new DateTime(2019, 1, 1, 21,2, 4), 2.0);
            tlSameDay.SetData(new DateTime(2019, 5, 5), 2.0);
            fExampleData.Add(FourEntrySameDayKey, tlSameDay);
            
            TimeNumberList tl5 = new TimeNumberList();
            AddData(tl5, 10, new DateTime(2018, 5, 4), 2.0, 55, 12.2);
            fExampleData.Add(TenEntryKey, tl5);

            TimeNumberList tl6 = new TimeNumberList();
            AddData(tl6, 100, new DateTime(2014, 5, 4), 2.0, 55, 12.2);
            fExampleData.Add(HundredEntryKey, tl6);

            TimeNumberList tl7 = new TimeNumberList();
            AddData(tl7, 1000, new DateTime(2010, 5, 4), 2.0, 55, 12.2);
            fExampleData.Add(ThousandEntryKey, tl7);

            TimeNumberList tl8 = new TimeNumberList();
            AddData(tl8, 10000, new DateTime(2000, 5, 4), 2.0, 55, 12.2);
            fExampleData.Add(TenThousandEntryKey, tl8);
        }

        public static TimeNumberList GetTestTimeList(string key)
        {
            if (fExampleData == null)
            {
                SetupTestLists();
            }

            return !fExampleData.ContainsKey(key) ? throw new KeyNotFoundException("TimeList not found.") : fExampleData[key].Copy();
        }
    }
}