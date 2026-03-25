using System;
using System.Collections.Generic;
using System.Text;

namespace ReedSolomon
{
    public static class Generator
    {
        public static byte[] Build(int eccLength)
        {
            byte[] g = [1];
            for (int i = 1; i <= eccLength; i++)
            {
                byte[] term = [1, GF256.Helper(i)]; 
                g = Polynomial.Multiply(g, term);
            }
            return g;
        }
    }
}
