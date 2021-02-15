using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureCommon.DataStructures
{
    public partial class TimeList
    {
        /// <inheritdoc/>
        public DailyValuation Value(DateTime date)
        {
            if (fValues == null)
            {
                return new DailyValuation(DateTime.Today, 1.0);
            }
            if (date < FirstDate())
            {
                return FirstValuation();
            }
            if (date >= LatestDate())
            {
                return LatestValuation();
            }
            if (Count() == 1)
            {
                return fValues[0].Copy();
            }

            DailyValuation earlier = NearestEarlierValue(date);
            DailyValuation later = NearestLaterValue(date);

            double value = earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (date - earlier.Day).Days;
            return new DailyValuation(date, value);
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date, Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction)
        {
            return Value(date, (valuation, dateTime) => valuation, (valuation, dateTime) => valuation, interpolationFunction);
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date, Func<DailyValuation, DateTime, DailyValuation> priorEstimator, Func<DailyValuation, DateTime, DailyValuation> postEstimator, Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction)
        {
            if (fValues == null)
            {
                return new DailyValuation(DateTime.Today, 1.0);
            }
            if (date < FirstDate())
            {
                return priorEstimator(FirstValuation(), date);
            }
            if (date >= LatestDate())
            {
                return postEstimator(LatestValuation(), date);
            }
            if (Count() == 1)
            {
                return fValues[0].Copy();
            }

            return new DailyValuation(date, interpolationFunction(NearestEarlierValue(date), NearestLaterValue(date), date));
        }

        /// <summary>
        /// Returns linearly interpolated value of the List on the date provided.
        /// The value prior to the first value is zero.
        /// </summary>
        public DailyValuation ValueZeroBefore(DateTime date)
        {
            return Value(date, (valuation, dateTime) => new DailyValuation(date, 0.0), (valuation, datetime) => valuation, (earlier, later, calculationDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (calculationDate - earlier.Day).Days);
        }

        /// <summary>
        /// Returns the DailyValuation on or before the date specified.
        /// </summary>
        public DailyValuation NearestEarlierValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (date < FirstDate())
                {
                    return new DailyValuation(date, 0.0);
                }

                if (Count() == 1)
                {
                    return fValues[0].Copy();
                }

                if (date > LatestDate())
                {
                    return LatestValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means
                // the first value later.
                for (int i = Count() - 1; i > -1; i--)
                {
                    if (date > fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                    if (date == fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                }
            }

            return new DailyValuation(date, 0.0);
        }

        /// <summary>
        /// Returns DailyValuation closest to the date but earlier to it.
        /// If a strictly earlier one cannot be found then return null.
        /// </summary>
        public DailyValuation RecentPreviousValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                // Some cases can return early.
                if (Count() == 1 || date <= FirstDate())
                {
                    return new DailyValuation(date, 0.0);
                }

                if (date > LatestDate())
                {
                    return LatestValuation();
                }

                // go back in time until find a valuation that is after the date we want
                // Then the value we want is the previous in the vector.
                for (int i = Count() - 1; i > 0; i--)
                {
                    if (date == fValues[i].Day)
                    {
                        return fValues[i - 1].Copy();
                    }
                    if (date > fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                }
            }

            return new DailyValuation(date, 0.0);
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        public bool TryGetNearestEarlierValue(DateTime date, out DailyValuation value)
        {
            value = NearestEarlierValue(date);
            if (value == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// returns nearest valuation after the date provided in the timelist.
        /// </summary>
        public DailyValuation NearestLaterValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (Count() == 1)
                {
                    return fValues[0].Copy();
                }

                if (date > LatestDate())
                {
                    return null;
                }

                if (date < FirstDate())
                {
                    return FirstValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means
                // the first value later.
                for (int i = 0; i < Count(); i++)
                {
                    if (date < fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal DailyValuation NearestValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (Count() == 1)
                {
                    return fValues[0].Copy();
                }

                if (date > LatestDate())
                {
                    return LatestValuation();
                }

                if (date < FirstDate())
                {
                    return FirstValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means
                // the first value later.
                for (int i = 0; i < Count(); i++)
                {
                    if (date < fValues[i].Day)
                    {
                        if (fValues[i].Day - date < date - fValues[i - 1].Day)
                        {
                            return fValues[i].Copy();
                        }

                        return fValues[i - 1].Copy();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first date held in the vector, or default if cannot find any data
        /// </summary>
        public DateTime FirstDate()
        {
            return FirstValuation()?.Day ?? new DateTime();
        }

        /// <summary>
        /// Returns first value held, or 0 if no data.
        /// </summary>
        public double FirstValue()
        {
            return FirstValuation()?.Value ?? 0.0;
        }

        /// <summary>
        /// Returns first pair of date and value, or null if this doesn't exist.
        /// </summary>
        /// <returns></returns>
        public DailyValuation FirstValuation()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[0].Copy();
            }

            return null;
        }

        /// <summary>
        /// Returns latest date held, or default if no data.
        /// </summary>
        public DateTime LatestDate()
        {
            return LatestValuation()?.Day ?? new DateTime();
        }

        /// <summary>
        /// Returns latest value, or 0 if no data held.
        /// </summary>
        public double LatestValue()
        {
            return LatestValuation()?.Value ?? 0.0;
        }

        /// <summary>
        /// Returns a pair of date and value of the most recently held data, or null if no data held.
        /// </summary>
        public DailyValuation LatestValuation()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1].Copy();
            }

            return null;
        }

        /// <summary>
        /// returns all valuations on or between the two dates specified, or empty list if none held.
        /// </summary>
        public List<DailyValuation> GetValuesBetween(DateTime earlierTime, DateTime laterTime)
        {
            List<DailyValuation> valuesBetween = new List<DailyValuation>();

            foreach (DailyValuation value in fValues)
            {
                if (value.Day >= earlierTime && value.Day <= laterTime)
                {
                    valuesBetween.Add(value.Copy());
                }
            }

            return valuesBetween;
        }
    }
}
