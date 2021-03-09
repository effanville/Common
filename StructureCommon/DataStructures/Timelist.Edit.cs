using System;
using System.Linq;
using StructureCommon.Reporting;

namespace StructureCommon.DataStructures
{
    public partial class TimeList
    {
        /// <inheritdoc/>
        public bool ValueExists(DateTime date, out int index)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        index = i;
                        return true;
                    }
                }
            }

            index = -1;
            return false;
        }

        /// <inheritdoc/>
        public bool TryAddValue(DateTime date, double value, IReportLogger reportLogger = null)
        {
            for (int i = 0; i < fValues.Count; i++)
            {
                if (fValues[i].Day == date)
                {
                    return false;
                }
            }

            AddData(date, value, reportLogger);
            return true;
        }

        /// <inheritdoc/>
        public bool TryEditData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.EditingData, $"Editing Data: {date} value changed from {fValues[i].Value} to {value}");
                        OnDataEdit(this);
                        fValues[i].SetValue(value);

                        return true;
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryEditData(DateTime oldDate, DateTime newDate, double value, IReportLogger reportLogger = null)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == oldDate)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.EditingData, $"Editing Data: {oldDate} value changed from {fValues[i].Value} to {newDate} - {value}");
                        fValues[i].SetData(newDate, value);
                        OnDataEdit(this);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public void TryEditDataOtherwiseAdd(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (!TryEditData(oldDate, date, value, reportLogger))
            {
                AddData(date, value, reportLogger);
            }
        }

        /// <inheritdoc/>
        public bool TryDeleteValue(DateTime date, IReportLogger reportLogger = null)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted value: date - {date} and value - {fValues[i].Value}");
                        fValues.RemoveAt(i);
                        OnDataEdit(this);
                        return true;
                    }
                }
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Deleting Value: Could not find data on date {date}.");
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetValue(DateTime date, out double value)
        {
            value = 0;
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        value = fValues[i].Copy().Value;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Adds value to the data.
        /// </summary>
        private void AddData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            DailyValuation valuation = new DailyValuation(date, value);
            fValues.Add(valuation);
            OnDataEdit(this);
            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Added on {date} value {value}.");
            Sort();
        }

        /// <summary>
        /// Orders the list according to date.
        /// </summary>
        private void Sort()
        {
            if (fValues != null && fValues.Any())
            {
                fValues = fValues.OrderBy(x => x.Day).ToList();
            }
        }
    }
}
