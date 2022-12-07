using System;

namespace Common.Structure.Minimisation
{
    /// <summary>
    /// Contains methods for finding a lower and upper bound for a root of a function.
    /// </summary>
    public class BracketMethod
    {
        private double ax, bx, cx, fa, fb, fc;

        /// <summary>
        /// Find a bracket for a root in the region specified.
        /// </summary>
        public void Bracket(double a, double b, Func<double, double> func)
        {
            double Gold = 1.618034;
            double GLimit = 100.0;
            double tiny = 1e-20;
            ax = a;
            bx = b;
            double fu;
            fa = func(ax);
            fb = func(bx);
            if (fb > fa)
            {
                double temp = fa;
                fa = fb;
                fb = temp;
                temp = ax;
                ax = bx;
                bx = temp;
            }

            cx = bx + Gold + (bx - ax);
            fc = func(cx);
            while (fb > fc)
            {
                double r = (bx - ax) * (fb - fc);
                double q = (bx - cx) * (fb - fa);
                double u = bx - ((bx - cx) * q - (bx - ax) * r) / (2.0 * Math.Abs(Math.Max(Math.Abs(q - r), tiny)) * Math.Sign(q - r));
                double ulim = bx + GLimit * (cx - bx);
                if ((bx - u) * (u - cx) > 0.0)
                {
                    fu = func(u);
                    if (fu < fc)
                    {
                        ax = bx;
                        bx = u;
                        fa = fb;
                        fb = fu;
                        return;
                    }
                    else if (fu > fb)
                    {
                        cx = u;
                        fc = fu;
                        return;
                    }
                    u = cx + Gold * (cx - bx);
                    fu = func(u);
                }
                else if ((cx - u) * (u - ulim) > 0.0)
                {
                    fu = func(u);
                    if (fu < fc)
                    {
                        SHFT(ref bx, ref cx, ref u, cx + Gold * (cx - bx));
                        SHFT(ref fb, ref fc, ref fu, func(u));
                    }
                }
                else if ((u - ulim) * (ulim - cx) >= 0.0)
                {
                    u = ulim;
                    fu = func(u);
                }
                else
                {
                    u = cx + Gold * (cx - bx);
                    fu = func(u);
                }

                SHFT(ref ax, ref bx, ref cx, u);
                SHFT(ref fa, ref fb, ref fc, fu);
            }

            void SHFT(ref double first, ref double second, ref double c, double d)
            {
                first = second;
                second = c;
                c = d;
            }
        }
    }
}
