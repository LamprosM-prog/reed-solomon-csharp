using System;
using System.Collections.Generic;
using System.Text;

namespace ReedSolomon
{
    public static  class PolynomialPrinter
    {
        public static string PrintPolynomial(byte[] poly)
        {
            if (poly == null || poly.Length == 0)
                return "0";

            StringBuilder sbPoly = new StringBuilder();
            int degree = poly.Length - 1;
            for (int i = 0; i < poly.Length; i++)
            {
                byte coeff = poly[i];

                if (coeff == 0)
                {
                    degree--;
                    continue;
                }

                
                if (sbPoly.Length > 0)
                    sbPoly.Append(" + ");

               
                if (!(coeff == 1 && degree != 0))
                    sbPoly.Append(coeff);

                if (degree > 0)
                {
                    sbPoly.Append('x');
                    sbPoly.Append('^').Append(degree);
                }

                degree--;
            }

            return sbPoly.Length > 0 ? sbPoly.ToString() : "0";
        }

    }
}
