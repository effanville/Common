using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Structure.DataStructures
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

            var vals = ValuesOnOrBeforeAndAfter(values, date);
            return new DailyValuation(date, interpolationFunction(vals.Item1, vals.Item2, date));
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

        private static (DailyValuation, DailyValuation) ValuesOnOrBeforeAndAfter(List<DailyValuation> values, DateTime date)
        {
            if (!values.Any())
            {
                return (null, null);
            }

            var first = FirstValuation(values);
            if (values.Count == 1)
            {
                return (first.Copy(), first.Copy());
            }

            if (date < first.Day)
            {
                return (null, first.Copy());
            }

            var latest = LatestValuation(values);
            if (date > latest.Day)
            {
                return (latest.Copy(), null);
            }

            int index = GetIndexOf(values, date);

            return (values[index].Copy(), values[index + 1].Copy());
        }

        private static int GetIndexOf(List<DailyValuation> values, DateTime date)
        {
            int lowerIndex = 0;
            int upperIndex = values.Count - 1;
            int midIndex = (int)Math.Floor((double)((values.Count - 1) / 2.0));
            while (lowerIndex + 1 != upperIndex)
            {
                if (date < values[midIndex].Day)
                {
                    upperIndex = midIndex;
                    midIndex = (int)Math.Floor((double)((lowerIndex + upperIndex) / 2));
                }
                else if (date >= values[midIndex].Day)
                {
                    lowerIndex = midIndex;
                    midIndex = (int)Math.Floor((double)((lowerIndex + upperIndex) / 2));
                }
            }

            return lowerIndex;
        }

        /// <summary>
        /// Returns the DailyValuation on or before the date specified.
        /// </summary>
        public DailyValuation ValueOnOrBefore(DateTime date)
        {
            return ValueOnOrBefore(Values(), date);
        }

        private static DailyValuation ValueOnOrBefore(List<DailyValuation> values, DateTime date)
        {
            if (!values.Any())
            {
                return null;
            }

            var first = FirstValuation(values);
            if (date < first.Day)
            {
                return null;
            }

            var latest = LatestValuation(values);
            if (date >= latest.Day)
            {
                return latest;
            }

            for (int i = values.Count - 1; i > -1; i--)
            {
                if (date >= values[i].Day)
                {
                    return values[i].Copy();
                }
            }

            return null;
        }

        /// <summary>
        /// Returns DailyValuation closest to the date but earlier to it.
        /// If a strictly earlier one cannot be found then return null.
        /// </summary>
        public DailyValuation ValueBefore(DateTime date)
        {
            return ValueBefore(Values(), date);
        }

        private static DailyValuation ValueBefore(List<DailyValuation> values, DateTime date)
        {
            if (values != null && values.Any())
            {
                if (date <= FirstValuation(values).Day)
                {
                    return null;
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
        /// If there is no date after the value, then null is returned.
        /// </summary>
        public DailyValuation ValueAfter(DateTime date)
        {
            return ValueAfter(Values(), date);
        }

        private static DailyValuation ValueAfter(List<DailyValuation> values, DateTime date)
        {
            if (!values.Any())
            {
                return null;
            }

            var first = FirstValuation(values);

            if (date >= LatestValuation(values).Day)
            {
                return null;
            }

            if (date < first.Day)
            {
                return first;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (date < values[i].Day)
                {
                    return values[i].Copy();
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
        /// Returns first value held, or <see cref="double.NaN"/> if no data.
        /// </summary>
        public double FirstValue()
        {
            return FirstValuation()?.Value ?? double.NaN;
        }

        /// <summary>
        /// Returns first pair of date and value, or null if this doesn't exist.
        /// </summary>
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
        /// Returns latest value, or <see cref="double.NaN"/> if no data held.
        /// </summary>
        public double LatestValue()
        {
            return LatestValuation()?.Value ?? double.NaN;
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
        /// Returns all valuations on or between the two dates specified, or empty list if none held.
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
