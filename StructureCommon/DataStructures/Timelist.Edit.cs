using System;
using System.Linq;
using StructureCommon.Reporting;

namespace StructureCommon.DataStructures
{
    public partial class TimeList
    {
        /// <inheritdoc/>
        public void CleanValues()
        {
            lock (valuesLock)
            {
                if (fValues.Count <= 1)
                {
                    return;
                }

                var lastValue = fValues[0];
                for (int valueIndex = 1; valueIndex < fValues.Count; ++valueIndex)
                {
                    if (fValues[valueIndex].Value.Equals(lastValue.Value))
                    {
                        fValues.RemoveAt(valueIndex);
                        --valueIndex;
                    }
                    else
                    {
                        lastValue = fValues[valueIndex];
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool ValueExists(DateTime date, out int index)
        {
            var values = Values();
            if (values.Any())
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].Day == date)
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
        [Obsolete("should use set data instead.")]
        public bool TryAddValue(DateTime date, double value, IReportLogger reportLogger = null)
        {
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            return false;
                        }
                    }
                }

                AddData(date, value, reportLogger);
            }

            return true;
        }

        /// <inheritdoc/>
        public void SetData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.EditingData, $"Editing Data: {date} value changed from {fValues[i].Value} to {value}");
                            OnDataEdit(this);
                            fValues[i].Value = value;
                            return;
                        }
                    }
                }

                AddData(date, value, reportLogger);
            }
        }

        /// <inheritdoc/>
        [Obsolete("should use set data instead.")]
        public bool TryEditData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.EditingData, $"Editing Data: {date} value changed from {fValues[i].Value} to {value}");
                            OnDataEdit(this);
                            fValues[i].Value = value;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryEditData(DateTime oldDate, DateTime newDate, double value, IReportLogger reportLogger = null)
        {
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == oldDate)
                        {
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.EditingData, $"Editing Data: {oldDate} value changed from {fValues[i].Value} to {newDate} - {value}");
                            fValues[i].SetData(newDate, value);
                            OnDataEdit(this);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryDeleteValue(DateTime date, IReportLogger reportLogger = null)
        {
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DeletingData, $"Deleted value: date - {date} and value - {fValues[i].Value}");
                            fValues.RemoveAt(i);
                            OnDataEdit(this);
                            return true;
                        }
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
            var values = Values();
            if (values.Any())
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].Day == date)
                    {
                        value = values[i].Copy().Value;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds value to the data. Requires to be called within the 
        /// lock <see cref="valuesLock"/> on<see cref="fValues"/>.
        /// </summary>
        private void AddData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            DailyValuation valuation = new DailyValuation(date, value);
            fValues.Add(valuation);

            OnDataEdit(this);
            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.AddingData, $"Adding value: added on {date} value {value}.");
            Sort();
        }

        /// <summary>
        /// Orders the list according to date.Requires to be called within the
        /// lock <see cref="valuesLock"/> on<see cref="fValues"/>.
        /// </summary>
        private void Sort()
        {
            if (fValues.Any())
            {
                fValues = fValues.OrderBy(x => x.Day).ToList();
            }
        }
    }
}
