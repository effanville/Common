namespace Effanville.Common.Structure.MathLibrary.Optimisation
{
    /// <summary>
    /// The exit condition for an optimisation routine.
    /// </summary>
    public enum ExitCondition
    {
        /// <summary>
        /// There is no exit condition specified.
        /// </summary>
        None,

        /// <summary>
        /// There was an error in the optimisation.
        /// </summary>
        Error,

        /// <summary>
        /// The values provided were invalid.
        /// </summary>
        InvalidValues,

        /// <summary>
        /// The optimisation exceeded the valid number of iterations.
        /// </summary>
        ExceedIterations,

        /// <summary>
        /// The optimisation converged within the bound tolerance.
        /// </summary>
        BoundTolerance,

        /// <summary>
        /// The optimisation converged.
        /// </summary>
        Converged,
    }
}
