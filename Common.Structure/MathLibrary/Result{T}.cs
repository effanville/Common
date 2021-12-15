namespace Common.Structure.MathLibrary
{
    /// <summary>
    /// A result class containing a value of a result or an Error message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// The resultant value to return.
        /// </summary>
        public T Value
        {
            get;
        }

        /// <summary>
        /// The error in case of no value being returned.
        /// </summary>
        public string Error
        {
            get;
        }

        /// <summary>
        /// Create an instance from a value.
        /// </summary>
        public Result(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Create an instance from an error.
        /// </summary>
        public Result(string error)
        {
            Error = error;
        }

        /// <summary>
        /// Create a Result object implicitly from a value.
        /// </summary>
        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }

        /// <summary>
        /// Is the result an error result.
        /// </summary>
        public bool IsError()
        {
            return !string.IsNullOrEmpty(Error);
        }
    }
}
