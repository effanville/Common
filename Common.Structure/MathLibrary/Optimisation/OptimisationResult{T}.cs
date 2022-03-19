namespace Common.Structure.MathLibrary.Optimisation
{
    /// <summary>
    /// A wrapper around a result for an optimisation algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class OptimisationResult<T>
    {
        /// <summary>
        /// Records the number of iterations of the algorithm.
        /// </summary>
        public int NumIterations
        {
            get;
        }

        /// <summary>
        /// The value of the result of the optimisation.
        /// </summary>
        public T ResultValue
        {
            get;
        }

        /// <summary>
        /// The reason/exit code for why the optimisation algorithm finished.
        /// </summary>
        public ExitCondition ReasonForExit
        {
            get;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public OptimisationResult(T resultValue, ExitCondition reasonForExit, int numIterations)
        {
            ResultValue = resultValue;
            ReasonForExit = reasonForExit;
            NumIterations = numIterations;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public OptimisationResult(ExitCondition reasonForExit)
        {
            ReasonForExit = reasonForExit;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public OptimisationResult(ExitCondition reasonForExit, int numIterations)
        {
            ReasonForExit = reasonForExit;
            NumIterations = numIterations;
        }

        /// <summary>
        /// The result of the optimisation is that there were too many iterations to find a 
        /// solution.
        /// </summary>
        public static OptimisationResult<T> ExceedIterations()
        {
            return new OptimisationResult<T>(ExitCondition.ExceedIterations);
        }

        /// <summary>
        /// The result of the optimisation is that there was an error.
        /// </summary>
        public static OptimisationResult<T> Error()
        {
            return new OptimisationResult<T>(ExitCondition.Error);
        }
    }
}
