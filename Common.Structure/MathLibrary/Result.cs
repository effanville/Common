namespace Common.Structure.MathLibrary
{
    /// <summary>
    /// A result class without a type.
    /// </summary>
    public sealed class Result
    {
        /// <summary>
        /// Create an error result from an error.
        /// </summary>
        /// <typeparam name="T">The type of the result to return.</typeparam>
        /// <param name="error">The error to encapsulate.</param>
        public static Result<T> ErrorResult<T>(string error)
        {
            return new Result<T>(error);
        }
    }
}
