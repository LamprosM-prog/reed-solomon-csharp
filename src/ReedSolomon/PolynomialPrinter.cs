using System;
using System.Collections.Generic;
using System.Text;

namespace ReedSolomon
{
    public static  class PolynomialPrinter
    {
        public static string PrintPolynomial(byte[] poly, bool lowestIndexIsConstant = false)
        {
            if (poly == null || poly.Length == 0)
                return "0";

            StringBuilder sb = new StringBuilder();
            int degree;

            if (lowestIndexIsConstant)
            {
                
                int lastNonZero = poly.Length - 1;
                while (lastNonZero >= 0 && poly[lastNonZero] == 0)
                    lastNonZero--;

                if (lastNonZero < 0)
                    return "0";

                for (int i = lastNonZero; i >= 0; i--)
                {
                    byte coeff = poly[i];
                    if (coeff == 0) continue;

                    if (sb.Length > 0) sb.Append(" + ");

                    if (!(coeff == 1 && i != 0))
                        sb.Append(coeff);

                    if (i > 0)
                    {
                        sb.Append('x');
                        if (i > 1) sb.Append('^').Append(i);
                    }
                }
            }
            else
            {
                degree = poly.Length - 1;
                for (int i = 0; i < poly.Length; i++)
                {
                    byte coeff = poly[i];
                    if (coeff == 0)
                    {
                        degree--;
                        continue;
                    }

                    if (sb.Length > 0) sb.Append(" + ");

                    if (!(coeff == 1 && degree != 0))
                        sb.Append(coeff);

                    if (degree > 0)
                    {
                        sb.Append('x');
                        if (degree > 1) sb.Append('^').Append(degree);
                    }

                    degree--;
                }
            }

            return sb.Length > 0 ? sb.ToString() : "0";
        }

    }
}
