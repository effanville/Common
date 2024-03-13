using System;
using System.Linq;

using Effanville.Common.Structure.Reporting;

namespace Effanville.Common.Structure.DataStructures
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
        public void CleanValues(double value)
        {
            lock (valuesLock)
            {
                for (int valueIndex = 0; valueIndex < fValues.Count; ++valueIndex)
                {
                    if (fValues[valueIndex].Value.Equals(value))
                    {
                        fValues.RemoveAt(valueIndex);
                        --valueIndex;
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
        public bool AddOrEditData(DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null)
        {
            if (TryEditData(oldDate, date, value, reportLogger))
            {
                return true;
            }

            SetData(date, value, reportLogger);
            return true;
        }

        /// <inheritdoc/>
        public void SetData(DateTime date, decimal value, IReportLogger reportLogger = null)
        {
            bool valueExists = false;
            bool edited = false;
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            if (fValues[i].Value != value)
                            {
                                edited = true;
                                reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, $"{nameof(TimeList)}.{nameof(SetData)}", $"Edit value {date} value changed from {fValues[i].Value} to {value}");
                            }

                            fValues[i].Value = value;
                            valueExists = true;
                        }
                    }
                }

                if (!valueExists)
                {
                    DailyValuation valuation = new DailyValuation(date, value);
                    fValues.Add(valuation);
                    reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, $"{nameof(TimeList)}.{nameof(SetData)}", $"Add value {date}-{value}.");
                    Sort();
                    edited = true;
                }
            }

            if (edited)
            {
                OnDataEdit(this);
            }
        }

        /// <inheritdoc/>
        public bool TryEditData(DateTime oldDate, DateTime newDate, decimal value, IReportLogger reportLogger = null)
        {
            bool edited = false;
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == oldDate)
                        {
                            reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, $"{nameof(TimeList)}.{nameof(TryEditData)}", $"{oldDate}-{fValues[i].Value} changed to {newDate} - {value}");
                            fValues[i].SetData(newDate, value);
                            edited = true;
                        }
                    }
                }
            }

            if (edited)
            {
                OnDataEdit(this);
            }

            return edited;
        }

        /// <inheritdoc/>
        public bool TryDeleteValue(DateTime date, IReportLogger reportLogger = null)
        {
            bool deleted = false;
            lock (valuesLock)
            {
                if (fValues.Any())
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, $"{nameof(TimeList)}.{nameof(TryDeleteValue)}", $"Value {date}-{fValues[i].Value} deleted.");
                            fValues.RemoveAt(i);
                            deleted = true;
                        }
                    }
                }
            }

            if (deleted)
            {
                OnDataEdit(this);
            }

            return deleted;
        }

        /// <inheritdoc/>
        public bool TryGetValue(DateTime date, out decimal value)
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
