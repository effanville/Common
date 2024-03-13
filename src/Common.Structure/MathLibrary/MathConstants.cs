using System;

namespace Effanville.Common.Structure.MathLibrary
{
    internal static class MathConstants
    {
        public const double Pi = Math.PI;
        public const double GoldenRatio = 1.61803398875;
        public const double InverseGoldenRatio = 0.61803399;
        public const double C = 1 - InverseGoldenRatio;
        public const double Tiny = 1e-20;
        public const double Eps = 3e-8;
        public const double TolX = 4 * Eps;
        public const double STPMX = 100.0;
        public const double ZEps = 1e-10;
        public const double CGold = 0.3819660;
    }
}