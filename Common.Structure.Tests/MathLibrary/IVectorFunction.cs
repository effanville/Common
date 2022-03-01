namespace Common.Structure.Tests.MathLibrary
{
    public interface IVectorFunction
    {
        double[] GlobalMinimum
        {
            get;
        }

        double[] GlobalMaximum
        {
            get;
        }

        double Value(double[] value);

        double[] Gradient(double[] value);


        double[,] Hessian(double[] value);
    }
}