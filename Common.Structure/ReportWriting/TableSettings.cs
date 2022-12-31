    using System.Collections.Generic;

namespace Common.Structure.ReportWriting
{   
    public sealed class TableSettings
    {
        public bool FirstColumnAsHeader
        {
            get;
            set;
        }

        public int MaxColumnWidth
        {
            get;
            set;
        }

        public IList<int> ColumnWidths
        {
            get;
            set;
        }

        public TableSettings()
        {
        }

        public static TableSettings FromSettings(TableSettings other)
        {
            return new TableSettings()
            {
                FirstColumnAsHeader = other.FirstColumnAsHeader,
                ColumnWidths = other.ColumnWidths
            };
        }

        public static TableSettings Default()
        {
            return new TableSettings()
            {
                FirstColumnAsHeader = false
            };
        }
    }
}