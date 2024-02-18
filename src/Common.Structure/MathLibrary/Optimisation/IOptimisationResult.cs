namespace Effanville.Common.Structure.MathLibrary.Optimisation;

public interface IOptimisationResult<TData>
{
    /// <summary>
    /// Records the number of iterations of the algorithm.
    /// </summary>
    public int NumIterations { get; }

    /// <summary>
    /// The reason/exit code for why the optimisation algorithm finished.
    /// </summary>
    public ExitCondition ReasonForExit { get; }
}