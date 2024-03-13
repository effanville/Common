namespace Effanville.Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// The type of the root finding algorithm.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// The bisection method.
            /// </summary>
            Bisection,

            /// <summary>
            /// The FalsePosition method.
            /// </summary>
            FalsePosition,

            /// <summary>
            /// The NewtonRaphson method.
            /// </summary>
            NewtonRaphson,

            /// <summary>
            /// The Ridders method.
            /// </summary>
            Ridders,

            /// <summary>
            /// The Secant method.
            /// </summary>
            Secant,

            /// <summary>
            /// The VWDB method.
            /// </summary>
            VWDB
        }
    }
}
