namespace Common.Structure.MathLibrary.Optimisation
{
    /// <summary>
    /// The exit condition for an optimisation routine.
    /// </summary>
    public enum ExitCondition
    {
        None,
        Error,
        InvalidValues,
        ExceedIterations,
        BoundTolerance,
        Converged,
    }
}
