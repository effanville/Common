using Common.Structure.Results;

namespace Common.Structure.MathLibrary.Optimisation
{
    public sealed class OptimisationSuccessResult<T> : SuccessResult<T>, IOptimisationResult<T>
    {
        /// <inheritdoc/>
        public int NumIterations { get; }

        /// <inheritdoc/>
        public ExitCondition ReasonForExit { get; }

        public OptimisationSuccessResult(T data) : base(data)
        {
        }
        
        public OptimisationSuccessResult(T data, ExitCondition reasonForExit, int numIterations) : this(data)
        {
            ReasonForExit = reasonForExit;
            NumIterations = numIterations;
        }
    }
}