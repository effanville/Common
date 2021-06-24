using System;

namespace Common.Structure.Minimisation
{
    public class BracketMethod
    {
        private double ax, bx, cx, fa, fb, fc;

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
                fb = fa;
                temp = ax;
                ax = bx;
                bx = ax;
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

                    }
                }
                else if (1 == 1)
                {
                }
                else
                {
                    u = cx + Gold * (cx - bx);
                    fu = func(u);
                }
            }
        }
    }
}
