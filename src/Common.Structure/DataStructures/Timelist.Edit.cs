using System;
using System.Linq;

using Effanville.Common.Structure.ChangeLogging;

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
                    if (fValues[valueIndex].Value.Equals(Convert.ToDecimal(value)))
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
        public UpdateResult<DailyValuation> SetData(DateTime date, decimal value)
        {
            try
            {
                lock (valuesLock)
                {
                    if (fValues.Count != 0)
                    {
                        for (int i = 0; i < fValues.Count; i++)
                        {
                            if (fValues[i].Day == date)
                            {
                                DailyValuation oldValue = fValues[i].Copy();
                                fValues[i].Value = value;
                                return UpdateResult.Change(oldValue, fValues[i].Copy());
                            }
                        }
                    }

                    DailyValuation valuation = new DailyValuation(date, value);
                    fValues.Add(valuation);
                    Sort();
                    return UpdateResult.Add(valuation);
                }
            }
            finally
            {
                OnDataEdit(this);
            }
        }

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> TryEditData(DateTime oldDate, DateTime newDate, decimal value)
        {
            try
            {
                lock (valuesLock)
                {
                    if (fValues.Count == 0)
                    {
                        return UpdateResult.Fail(new DailyValuation(oldDate, value), isChange: true);
                    }

                    for (int i = 0; i < fValues.Count; i++)
                    {
                        var thisValue = fValues[i];
                        if (thisValue.Day == oldDate)
                        {
                            if (thisValue.Value != value)
                            {
                                DailyValuation oldValue = thisValue.Copy();
                                thisValue.SetData(newDate, value);

                                return UpdateResult.Change(oldValue, thisValue.Copy());
                            }
                        }
                    }

                    return UpdateResult.Fail(new DailyValuation(oldDate, value), isChange: true);
                }
            }
            finally
            {
                OnDataEdit(this);
            }
        }

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> TryDeleteValue(DateTime date)
        {
            try
            {
                lock (valuesLock)
                {
                    if (fValues.Count == 0)
                    {
                        return UpdateResult.Fail(new DailyValuation(date, default), isDelete: true);
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

                    return UpdateResult.Fail(new DailyValuation(date, default), isDelete: true);
                }
            }
            finally
            {
                OnDataEdit(this);
            }
        }

        /// <inheritdoc/>
        public bool TryGetValue(DateTime date, out decimal value)
        {
            value = 0;
            var values = Values();
            if (values.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Day == date)
                {
                    value = values[i].Copy().Value;
                    return true;
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
