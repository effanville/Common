using System.Collections.Generic;

namespace Effanville.Common.Structure.MathLibrary.Optimisation;

public sealed class OptimisationErrorResult<T> : Results.ErrorResult<T>, IOptimisationResult<T>
{
    /// <inheritdoc/>
    public int NumIterations { get; }

    /// <inheritdoc/>
    public ExitCondition ReasonForExit { get; }
        
    /// <summary>
    /// Construct an instance.
    /// </summary>
    public OptimisationErrorResult(ExitCondition reasonForExit)
        : base(reasonForExit.ToString())
    {
        ReasonForExit = reasonForExit;
    }
        
    /// <summary>
    /// Construct an instance.
    /// </summary>
    public OptimisationErrorResult(ExitCondition reasonForExit, int numIterations)
        : base(reasonForExit.ToString())
    {
        ReasonForExit = reasonForExit;
        NumIterations = numIterations;
    }
        
    public OptimisationErrorResult(string message) : base(message)
    {
    }

    public OptimisationErrorResult(string message, IList<string> errors) : base(message, errors)
    {
    }
        
    /// <summary>
    /// The result of the optimisation is that there were too many iterations to find a 
    /// solution.
    /// </summary>
    public static OptimisationErrorResult<T> ExceedIterations(int numIterations) 
        => new(ExitCondition.ExceedIterations, numIterations);

    /// <summary>
    /// The result of the optimisation is that there was an error.
    /// </summary>
    public static OptimisationErrorResult<T> ErrorResult()
        => new(ExitCondition.Error.ToString());
}