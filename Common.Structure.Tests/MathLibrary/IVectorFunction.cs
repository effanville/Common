using Common.Structure.MathLibrary.Optimisation.Vector;

namespace Common.Structure.Tests.MathLibrary
{
    public interface IVectorFunction
    {
        VectorEvaluationPoint GlobalMinimum
        {
            get;
        }

        VectorEvaluationPoint GlobalMaximum
        {
            get;
        }

        double Value(double[] value);

        double[] Gradient(double[] value);


        double[,] Hessian(double[] value);
    }
}