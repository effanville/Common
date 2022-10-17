using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    internal class StrongWolfeLineSearcher : ILineSearcher
    {
        public double C1
        {
            get;
        }
        public double C2
        {
            get;
        }
        public double ParameterTolerance
        {
            get;
        }
        public int MaximumIterations
        {
            get;
        }
        public StrongWolfeLineSearcher(double c1, double c2, double parameterTolerance, int maxIterations = 10)
        {
            C1 = c1;
            C2 = c2;
            ParameterTolerance = parameterTolerance;
            MaximumIterations = maxIterations;
        }
        public Result<VectorFuncEvaluation> FindConformingStep(
            double[] startingPoint,
            double startingValue,
            double[] startingDerivative,
            double[] searchDirection,
            Func<double[], double> func,
            int maxIterations = 100)
        {
            return FindConformingStep(
                startingPoint,
                startingValue, startingDerivative,
                searchDirection,
                double.PositiveInfinity,
                func,
                maxIterations);
        }

        private bool WolfeCondition(double stepDd, double initialDd)
        {
            return Math.Abs(stepDd) > C2 * Math.Abs(initialDd);
        }

        public Result<VectorFuncEvaluation> FindConformingStep(
            double[] startingPoint,
            double startingValue,
            double[] startingDerivative,
            double[] searchDirection,
            double upperBound,
            Func<double[], double> func,
            int maxIterations = 100)
        {
            double lowerBound = 0.0;
            double step = 
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                double
            }
            throw new NotImplementedException();
        }
    }
}
