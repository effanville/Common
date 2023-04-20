namespace Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// The type of the root finding algorithm.
        /// </summary>
        public enum Type
        {
            Bisection,
            FalsePosition,
            NewtonRaphson,
            Ridders,
            Secant,
            VWDB
        }
    }
}
