namespace Common.Structure.MathLibrary.Optimisation.Scalar
{
    public sealed class ScalarMinResult
    {
        public double MinimisingPoint
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

        public ScalarMinResult(double minimisingPoint, double minimisingValue, ExitCondition reasonForExit)
        {
            MinimisingPoint = minimisingPoint;
            MinimisingValue = minimisingValue;
            ReasonForExit = reasonForExit;
        }

        public ScalarMinResult(ExitCondition reasonForExit)
        {
            ReasonForExit = reasonForExit;
        }

        public static ScalarMinResult ExceedIterations()
        {
            return new ScalarMinResult(ExitCondition.ExceedIterations);
        }

        public override bool Equals(object obj)
        {
            return obj is ScalarMinResult result
                && MinimisingPoint == result.MinimisingPoint
                && MinimisingValue == result.MinimisingValue
                   && ReasonForExit == result.ReasonForExit;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + MinimisingPoint.GetHashCode();
            hashCode = 23 * hashCode + MinimisingValue.GetHashCode();
            hashCode = 23 * hashCode + ReasonForExit.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{ReasonForExit}-Point:{MinimisingPoint}-Value:{MinimisingValue}";
        }
    }
}
