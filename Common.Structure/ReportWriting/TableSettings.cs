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

        public int? MaxColumnWidth
        {
            get;
            set;
        }

        public int? MinColumnWidth
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
                ColumnWidths = other.ColumnWidths,
                MaxColumnWidth = other.MaxColumnWidth,
                MinColumnWidth = other.MinColumnWidth,
            };
        }

        public static TableSettings Default()
        {
            return new TableSettings()
            {
                FirstColumnAsHeader = false
            };
        }

        public string WidthStyleName()
        {
            if (!MaxColumnWidth.HasValue && !MinColumnWidth.HasValue)
            {
                return null;
            }

            if (MaxColumnWidth.HasValue && !MinColumnWidth.HasValue)
            {
                return $"width-{MaxColumnWidth}";
            }

            return $"width-{MaxColumnWidth}-{MinColumnWidth}";
        }

        public int GetColumnWidth(int columnIndex)
        {
            if (ColumnWidths == null || columnIndex < 0 || columnIndex > ColumnWidths.Count - 1)
            {
                return 0;
            }

            int specifiedColumnWidth = ColumnWidths[columnIndex];
            if (MinColumnWidth.HasValue && specifiedColumnWidth < MinColumnWidth)
            {
                return MinColumnWidth.Value;
            }

            if (MaxColumnWidth.HasValue && specifiedColumnWidth > MaxColumnWidth)
            {
                return MaxColumnWidth.Value;
            }

            return specifiedColumnWidth;
        }
    }
}