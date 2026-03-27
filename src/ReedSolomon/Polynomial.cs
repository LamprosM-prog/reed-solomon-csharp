using System;
using System.Linq;

namespace ReedSolomon
{
    public static class Polynomial
    {
        public static int Degree(byte[] poly)
        {
            Trim(poly);
            int degree = poly.Length - 1;
            return degree;
        }
        private static byte[] Trim(byte[] poly)
        {
            int i = 0;
            while (i < poly.Length - 1 && poly[i] == 0)
                i++;

            return poly.Skip(i).ToArray();
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
           while(Degree(b)<=Degree(a))
           {
                byte factor = GF256.Multiply(a[0], GF256.Inverse(b[0])); //satanic black magic
                for (int i = 0; i < b.Length; i++)
                {
                    byte bScaled = GF256.Multiply(b[i], factor);
                    a[i] = GF256.Add(a[i], bScaled);
                }
                a = Trim(a);
           }
            return a;
        }
    }
}