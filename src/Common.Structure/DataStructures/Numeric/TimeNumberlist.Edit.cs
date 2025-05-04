using System;
using System.Linq;

using Effanville.Common.Structure.DataEdit;

namespace Effanville.Common.Structure.DataStructures.Numeric
{
    public partial class TimeNumberList
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
        public UpdateResult<DailyNumeric> AddOrEditData(DateTime oldDate, DateTime date, double value)
        {
            UpdateResult<DailyNumeric> editResult = TryEditData(oldDate, date, value);
            if (editResult.Success)
            {
                return editResult;
            }

            return SetData(date, value);
        }

        /// <inheritdoc/>
        public UpdateResult<DailyNumeric> SetData(DateTime date, double value)
        {
            lock (valuesLock)
            {
                if (fValues.Count != 0)
                {
                    for (int i = 0; i < fValues.Count; i++)
                    {
                        if (fValues[i].Day == date)
                        {
                            DailyNumeric oldValue = fValues[i].Copy();
                            fValues[i].Value = value;
                            return UpdateResult.Change(oldValue, fValues[i].Copy());
                        }
                    }
                }

                DailyNumeric valuation = new DailyNumeric(date, value);
                fValues.Add(valuation);
                Sort();
                return UpdateResult.Add(valuation);
            }
        }

        /// <inheritdoc/>
        public UpdateResult<DailyNumeric> TryEditData(DateTime oldDate, DateTime newDate, double value)
        {
            lock (valuesLock)
            {
                if (fValues.Count == 0)
                {
                    return UpdateResult.Fail(new DailyNumeric(oldDate, value), isChange: true);
                }

                for (int i = 0; i < fValues.Count; i++)
                {
                    var thisValue = fValues[i];
                    if (thisValue.Day == oldDate)
                    {
                        if (thisValue.Value != value)
                        {
                            DailyNumeric oldValue = thisValue.Copy();
                            thisValue.SetData(newDate, value);

                            return UpdateResult.Change(oldValue, thisValue.Copy());
                        }
                    }
                }

                return UpdateResult.Fail(new DailyNumeric(oldDate, value), isChange: true);
            }
        }

        /// <inheritdoc/>
        public UpdateResult<DailyNumeric> TryDeleteValue(DateTime date)
        {
            lock (valuesLock)
            {
                if (fValues.Count == 0)
                {
                    return UpdateResult.Fail(new DailyNumeric(date, default), isDelete: true);
                }

                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        var value = fValues[i].Copy();
                        fValues.RemoveAt(i);
                        return UpdateResult.Delete(value);
                    }
                }

                return UpdateResult.Fail(new DailyNumeric(date, default), isDelete: true);
            }
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
