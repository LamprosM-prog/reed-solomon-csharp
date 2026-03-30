using System;
using System.Linq;

namespace ReedSolomon
{
    public static class Polynomial
    {
        public static int Degree(byte[] poly)
        {
            for (int i = poly.Length - 1; i >= 0; i--)
            {
                if (poly[i] != 0)
                    return i;
            }

            return -1; 
        }
        private static byte[] Trim(byte[] poly)
        {
            int i = poly.Length - 1;
            while (i > 0 && poly[i] == 0)
                i--;
            return poly.Take(i + 1).ToArray();
        }
        public static byte[] Add(byte[] a, byte[] b)
        {
            int maxLen = Math.Max(a.Length, b.Length);
            byte[] result = new byte[maxLen];
            int aIndex = a.Length - 1;
            int bIndex = b.Length - 1;
            int rIndex = maxLen - 1;

            while (rIndex >= 0)
            {
                byte av = aIndex >= 0 ? a[aIndex] : (byte)0;
                byte bv = bIndex >= 0 ? b[bIndex] : (byte)0;

                result[rIndex] = GF256.Add(av, bv);

                aIndex--;
                bIndex--;
                rIndex--;
            }

            return Trim(result);
        }
        public static byte[] Multiply(byte[] a, byte[] b)
        {
            int degree = a.Length + b.Length - 1;
            int aIndex = a.Length - 1;
            int bIndex = b.Length - 1;
            byte[] result = new byte[degree];
            for (int i = aIndex; i >= 0; i--)
            {
                for(int j = bIndex; j >= 0; j--)
                {
                    int position = i + j;
                    result[position] = GF256.Add(result[position],GF256.Multiply(a[i], b[j]));
                   
                }

            }
            return Trim(result);
        }
        // a is the dividend  and b is the devisor (a/b)
        public static byte[] Divide(byte[] a, byte[] b)
        {
            a = Trim(a);
            b = Trim(b);

            while (Degree(b) <= Degree(a))
            {
                int degA = Degree(a);
                int degB = Degree(b);
                byte factor = GF256.Multiply(a[degA], GF256.Inverse(b[degB]));
                int shift = degA - degB;

                for (int i = 0; i <= degB; i++)
                {
                    byte bScaled = GF256.Multiply(b[i], factor);
                    a[i + shift] = GF256.Add(a[i + shift], bScaled);
                }

                a = Trim(a);
            }

            return a;
        }
    }
}