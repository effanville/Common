namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    public sealed class VectorMinResult
    {
        public int NumIterations
        {
            get;
        }

        public double[] MinimisingPoint
        {
            get;
        }

        public double MinimisingValue
        {
            get;
        }

        public ExitCondition ReasonForExit
        {
            get;
        }

        public VectorMinResult(double[] minimisingPoint, double minimisingValue, ExitCondition reasonForExit, int numIterations)
        {
            MinimisingPoint = minimisingPoint;
            MinimisingValue = minimisingValue;
            ReasonForExit = reasonForExit;
            NumIterations = numIterations;
        }

        public VectorMinResult(ExitCondition reasonForExit)
        {
            ReasonForExit = reasonForExit;
        }

        public VectorMinResult(ExitCondition reasonForExit, int numIterations)
        {
            ReasonForExit = reasonForExit;
            NumIterations = numIterations;
        }
    }
}
