using System;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    public static class DownhillSimplex
    {
        public static VectorMinResult Minimise(Func<double[], double> func)
        {
            return new VectorMinResult(ExitCondition.ExceedIterations);
        }
    }
}
