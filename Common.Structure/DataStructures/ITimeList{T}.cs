using System;

using Common.Structure.Reporting;

namespace Common.Structure.DataStructures
{
    /// <summary>
    /// A list of <typeparamref name="T"/>s that is ordered by the <see cref="Daily{T}.Day"/> property.
    /// Contains methods to add and alter the data, as well as calculate values.
    /// </summary>
    public interface ITimeList<T> where T : IComparable, IComparable<T>, IEquatable<T>
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
        Daily<T> this[int index]
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
        /// Checks if value on <param name="date"/> exists. If exists then <param name="index"/> is output.
        /// </summary>
        bool ValueExists(DateTime date, out int index);

        /// <summary>
        /// obtains first instance of the value for the date requested. Returns false if no data.
        /// </summary>
        bool TryGetValue(DateTime date, out T value);

        /// <summary>
        /// Sets data in the TimeList on the date provided. Adds if it doesnt exist, edits if it does.
        /// </summary>
        /// <param name="date">The date to edit data on.</param>
        /// <param name="value">The value to set for this date.</param>
        /// <param name="reportLogger">Reports the logging of this action.</param>
        void SetData(DateTime date, T value, IReportLogger reportLogger = null);

        /// <summary>
        /// Edits data in the TimeList on the date provided if it can.
        /// </summary>
        /// <param name="oldDate">The date to edit data on.</param>
        /// <param name="newDate">The date to set the value to be stored on.</param>
        /// <param name="value">The value to set for this date.</param>
        /// <param name="reportLogger">Reports the logging of this action.</param>
        bool TryEditData(DateTime oldDate, DateTime newDate, T value, IReportLogger reportLogger = null);

        /// <summary>
        /// Deletes data if exists. If deletes, returns true.
        /// </summary>
        /// <param name="date">The date to edit data on.</param>
        /// <param name="reportLogger">Reports the logging of this action.</param>
        bool TryDeleteValue(DateTime date, IReportLogger reportLogger = null);

        /// <summary>
        /// Returns the linearly interpolated value of the List on the date provided.
        /// </summary>
        /// <param name="date">The date to retrieve the value on.</param>
        Daily<T> Value(DateTime date);

        /// <summary>
        /// Returns the value of the <see cref="TimeList"/> with various methods to interpolate. The value prior to the first date returns the first date,
        /// and a value after returns the value after.
        /// </summary>
        /// <param name="date">The date to calculate the value on.</param>
        /// <param name="interpolationFunction">The function to interpolate between values. This is a function
        /// of the <see cref="Daily{T}"/> before the date to calculate,
        /// the date, and the <see cref="Daily{T}"/> after the calculation date, and returns the value on that date. For example, this could
        /// linearly interpolate between values.</param>
        /// <returns>
        /// A valuation with the date and the value on that date. The date is not necessarily the date requested. For example
        /// if the prior evaluator returns a different date, then that date is recorded.</returns>
        Daily<T> Value(DateTime date, Func<Daily<T>, Daily<T>, DateTime, double> interpolationFunction);

        /// <summary>
        /// Returns the value of the <see cref="TimeList"/> with various methods to interpolate.
        /// </summary>
        /// <param name="date">The date to calculate the value on.</param>
        /// <param name="priorEstimator">The Function to estimate the value prior to the first value. This is a function of the first valuation and the date to evaluate on.</param>
        /// <param name="postEstimator">The Function to estimate the value after the final value. This is a function of the Last valuation and the date to evaluate on.</param>
        /// <param name="interpolationFunction">The function to interpolate between values. This is a function
        /// of the <see cref="Daily{T}"/> before the date to calculate,
        /// the date, and the <see cref="Daily{T}"/> after the calculation date, and returns the value on that date. For example, this could
        /// linearly interpolate between values.</param>
        /// <returns>
        /// A valuation with the date and the value on that date. The date is not necessarily the date requested. For example
        /// if the prior evaluator returns a different date, then that date is recorded.</returns>
        Daily<T> Value(DateTime date, Func<Daily<T>, DateTime, Daily<T>> priorEstimator, Func<Daily<T>, DateTime, Daily<T>> postEstimator, Func<Daily<T>, Daily<T>, DateTime, double> interpolationFunction);
    }
}
