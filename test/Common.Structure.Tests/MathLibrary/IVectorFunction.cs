using Effanville.Common.Structure.MathLibrary.Optimisation.Vector;

namespace Effanville.Common.Structure.Tests.MathLibrary
{
    public interface IVectorFunction
    {
        VectorFuncEval GlobalMinimum
        {
            get;
        }

        VectorFuncEval GlobalMaximum
        {
            get;
        }

        double Value(double[] value);

        double[] Gradient(double[] value);


        double[,] Hessian(double[] value);
    }
}