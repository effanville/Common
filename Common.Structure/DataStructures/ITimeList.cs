using System;

using Common.Structure.Reporting;

namespace Common.Structure.DataStructures
{
    /// <summary>
    /// A list of <see cref="DailyValuation"/>s that is ordered by the <see cref="DailyValuation.Day"/> property.
    /// Contains methods to add and alter the data, as well as calculate values.
    /// </summary>
    public interface ITimeList
    {
        /// <summary>
        /// Event that is raised when data is edited.
        /// </summary>
        event EventHandler DataEdit;

        /// <summary>
        /// Access the list at the specified index. This returns a copy of the data.
        /// </summary>
        /// <param name="index">The index to access data at.</param>
        /// <returns>A copy of the data at the index.</returns>
        DailyValuation this[int index]
        {
            get;
        }

        /// <summary>
        /// Returns true if contains any entries.
        /// </summary>
        bool Any();

        /// <summary>
        /// Returns the number of valuations in the timelist.
        /// </summary>
        int Count();

        /// <summary>
        /// Removes unnecessary values in the list. where the value is the same as the previous one.
        /// </summary>
        void CleanValues();

        /// <summary>
        /// Removes all instances of the value specified in the list. where the value is the same as the previous one.
        /// </summary>
        void CleanValues(double value);

        /// <summary>
        /// Checks if value on <param name="date"/> exists. If exists then <param name="index"/> is output.
        /// </summary>
        bool ValueExists(DateTime date, out int index);

        /// <summary>
        /// obtains first instance of the value for the date requested. Returns false if no data.
        /// </summary>
        bool TryGetValue(DateTime date, out double value);

        /// <summary>
        /// Edits data in the TimeList on the date provided if it can.
        /// </summary>
        /// <param name="oldDate">The date to edit data on.</param>
        /// <param name="newDate">The date to set the value to be stored on.</param>
        /// <param name="value">The value to set for this date.</param>
        /// <param name="reportLogger">Reports the logging of this action.</param>
        bool TryEditData(DateTime oldDate, DateTime newDate, double value, IReportLogger reportLogger = null);

        /// <summary>
        /// Sets data in the TimeList on the date provided. Adds if it doesnt exist, edits if it does.
        /// </summary>
        /// <param name="date">The date to edit data on.</param>
        /// <param name="value">The value to set for this date.</param>
        /// <param name="reportLogger">Reports the logging of this action.</param>
        void SetData(DateTime date, double value, IReportLogger reportLogger = null);

        /// <summary>
        /// Deletes data if exists. If deletes, returns true.
        /// </summary>
        bool TryDeleteValue(DateTime date, IReportLogger reportLogger = null);

        /// <summary>
        /// Returns the linearly interpolated value of the List on the date provided.
        /// </summary>
        /// <param name="date">The date to retrieve the value on.</param>
        DailyValuation Value(DateTime date);

        /// <summary>
        /// Returns the value of the <see cref="TimeList"/> with various methods to interpolate. The value prior to the first date returns the first date,
        /// and a value after returns the value after.
        /// </summary>
        /// <param name="date">The date to calculate the value on.</param>
        /// <param name="interpolationFunction">The function to interpolate between values. This is a function
        /// of the <see cref="DailyValuation"/> before the date to calculate,
        /// the date, and the <see cref="DailyValuation"/> after the calculation date, and returns the value on that date. For example, this could
        /// linearly interpolate between values.</param>
        /// <returns>
        /// A valuation with the date and the value on that date. The date is not necessarily the date requested. For example
        /// if the prior evaluator returns a different date, then that date is recorded.</returns>
        DailyValuation Value(DateTime date, Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction);

        /// <summary>
        /// Returns the value of the <see cref="TimeList"/> with various methods to interpolate.
        /// </summary>
        /// <param name="date">The date to calculate the value on.</param>
        /// <param name="priorEstimator">The Function to estimate the value prior to the first value. This is a function of the first valuation and the date to evaluate on.</param>
        /// <param name="postEstimator">The Function to estimate the value after the final value. This is a function of the Last valuation and the date to evaluate on.</param>
        /// <param name="interpolationFunction">The function to interpolate between values. This is a function
        /// of the <see cref="DailyValuation"/> before the date to calculate,
        /// the date, and the <see cref="DailyValuation"/> after the calculation date, and returns the value on that date. For example, this could
        /// linearly interpolate between values.</param>
        /// <returns>
        /// A valuation with the date and the value on that date. The date is not necessarily the date requested. For example
        /// if the prior evaluator returns a different date, then that date is recorded.</returns>
        DailyValuation Value(DateTime date, Func<DailyValuation, DateTime, DailyValuation> priorEstimator, Func<DailyValuation, DateTime, DailyValuation> postEstimator, Func<DailyValuation, DailyValuation, DateTime, double> interpolationFunction);

        /// <summary>
        /// Provides a new <see cref="TimeList"/> with multiplicative inverses as values,
        /// and where the value 0 is mapped to <see cref="double.PositiveInfinity"/>.
        /// </summary>
        TimeList Inverted();
    }
}
