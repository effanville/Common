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
            double interpolationFunction(DailyValuation earlier, DailyValuation later, DateTime day) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (day - earlier.Day).Days;
            return Value(
                date,
                (valuation, dateTime) => valuation,
                (valuation, dateTime) => valuation,
                interpolationFunction);
        }

        /// <inheritdoc/>
        public DailyValuation Value(
            DateTime date,
            Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction)
        {
            return Value(
                date,
                (valuation, dateTime) => valuation,
                (valuation, dateTime) => valuation,
                interpolationFunction);
        }

        /// <inheritdoc/>
        public DailyValuation Value(
            DateTime date,
            Func<DailyValuation, DateTime, DailyValuation> priorEstimator,
            Func<DailyValuation, DateTime, DailyValuation> postEstimator,
            Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction)
        {
            return Value(
                Values(),
                date,
                priorEstimator,
                postEstimator,
                interpolationFunction);
        }

        private static DailyValuation Value(List<DailyValuation> values, DateTime date, Func<DailyValuation, DateTime, DailyValuation> priorEstimator, Func<DailyValuation, DateTime, DailyValuation> postEstimator, Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction)
        {
            if (!values.Any())
            {
                return null;
            }

            var first = FirstValuation(values);
            if (date < first.Day)
            {
                return priorEstimator(first, date);
            }

            var latest = LatestValuation(values);
            if (date >= latest.Day)
            {
                return postEstimator(latest, date);
            }

            if (values.Count == 1)
            {
                return values[0].Copy();
            }

            return new DailyValuation(date, interpolationFunction(NearestEarlierValue(values, date), NearestLaterValue(values, date), date));
        }

        /// <summary>
        /// Returns linearly interpolated value of the List on the date provided.
        /// The value prior to the first value is zero.
        /// </summary>
        public DailyValuation ValueZeroBefore(DateTime date)
        {
            return Value(
                date,
                (valuation, dateTime) => new DailyValuation(date, 0.0),
                (valuation, datetime) => valuation,
                (earlier, later, calculationDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (calculationDate - earlier.Day).Days);
        }

        /// <summary>
        /// Returns the DailyValuation on or before the date specified.
        /// </summary>
        public DailyValuation NearestEarlierValue(DateTime date)
        {
            return NearestEarlierValue(Values(), date);
        }

        private static DailyValuation NearestEarlierValue(List<DailyValuation> values, DateTime date)
        {
            if (values.Any())
            {
                if (date < values[0].Day)
                {
                    return new DailyValuation(date, 0.0);
                }

                if (values.Count == 1)
                {
                    return values[0].Copy();
                }

                var latest = LatestValuation(values);
                if (date > latest.Day)
                {
                    return latest;
                }

                // list sorted with earliest at start. First occurence greater than value means
                // the first value later.
                for (int i = values.Count - 1; i > -1; i--)
                {
                    if (date > values[i].Day)
                    {
                        return values[i].Copy();
                    }
                    if (date == values[i].Day)
                    {
                        return values[i].Copy();
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
            return RecentPreviousValue(Values(), date);
        }

        private static DailyValuation RecentPreviousValue(List<DailyValuation> values, DateTime date)
        {
            if (values != null && values.Any())
            {
                // Some cases can return early.
                if (values.Count == 1 || date <= FirstValuation(values).Day)
                {
                    return new DailyValuation(date, 0.0);
                }

                var latest = LatestValuation(values);
                if (date > latest.Day)
                {
                    return latest;
                }

                // go back in time until find a valuation that is after the date we want
                // Then the value we want is the previous in the vector.
                for (int i = values.Count - 1; i > 0; i--)
                {
                    if (date == values[i].Day)
                    {
                        return values[i - 1].Copy();
                    }
                    if (date > values[i].Day)
                    {
                        return values[i].Copy();
                    }
                }
            }

            return new DailyValuation(date, 0.0);
        }

        /// <summary>
        /// returns nearest valuation after the date provided in the timelist.
        /// </summary>
        public DailyValuation NearestLaterValue(DateTime date)
        {
            return NearestLaterValue(Values(), date);
        }

        private static DailyValuation NearestLaterValue(List<DailyValuation> values, DateTime date)
        {
            if (values.Any())
            {
                if (values.Count == 1)
                {
                    return values[0].Copy();
                }

                if (date > LatestValuation(values).Day)
                {
                    return null;
                }

                var first = FirstValuation(values);
                if (date < first.Day)
                {
                    return first;
                }

                // list sorted with earliest at start. First occurence greater than value means
                // the first value later.
                for (int i = 0; i < values.Count; i++)
                {
                    if (date < values[i].Day)
                    {
                        return values[i].Copy();
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
            return NearestValue(Values(), date);
        }

        private static DailyValuation NearestValue(List<DailyValuation> values, DateTime date)
        {
            if (values != null && values.Any())
            {
                if (values.Count == 1)
                {
                    return values[0].Copy();
                }

                var latest = LatestValuation(values);
                if (date > latest.Day)
                {
                    return latest;
                }

                var first = FirstValuation(values);
                if (date < first.Day)
                {
                    return first;
                }

                // list sorted with earliest at start. First occurence greater than value means
                // the first value later.
                for (int i = 0; i < values.Count; i++)
                {
                    if (date < values[i].Day)
                    {
                        if (values[i].Day - date < date - values[i - 1].Day)
                        {
                            return values[i].Copy();
                        }

                        return values[i - 1].Copy();
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
            return FirstValuation(Values());
        }

        private static DailyValuation FirstValuation(List<DailyValuation> values)
        {
            if (values.Any())
            {
                return values[0].Copy();
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
            return LatestValuation(Values());
        }

        private static DailyValuation LatestValuation(List<DailyValuation> values)
        {
            if (values.Any())
            {
                return values[values.Count - 1].Copy();
            }

            return null;
        }

        /// <summary>
        /// returns all valuations on or between the two dates specified, or empty list if none held.
        /// </summary>
        public List<DailyValuation> GetValuesBetween(DateTime earlierTime, DateTime laterTime)
        {
            return GetValuesBetween(Values(), earlierTime, laterTime);
        }

        private static List<DailyValuation> GetValuesBetween(List<DailyValuation> values, DateTime earlierTime, DateTime laterTime)
        {
            List<DailyValuation> valuesBetween = new List<DailyValuation>();

            foreach (DailyValuation value in values)
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
