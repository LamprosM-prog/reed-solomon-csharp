using System;

namespace ReedSolomon
{
    public static  class GF256
    {
        static byte[] log = new byte[256];
        static byte[] exp = new byte[512];
        static GF256()
        {
            uint value = 1;
            for (int i = 0; i < 255; i++)
            {
                exp[i] = (byte)value;
                log[value] = (byte)i;
                value <<= 1;
                if (value >= 256)
                {
                    value ^= 0x11D;
                }
                    
            }
            for(int i = 255; i< 512; i++)
            {
                exp[i] = exp[i - 255];
            }
        }
        public static byte Add(byte a, byte b) => (byte)(a ^ b);
        public static byte Multiply(byte a, byte b)
        {
            if (a == 0 || b == 0) return 0;
            return exp[log[a] + log[b]]; 
        }

        public static byte Divide(byte a, byte b)
        {
            if (b == 0) throw new DivideByZeroException();
            if (a == 0) return 0;
            return exp[log[a] - log[b] + 255]; 
        }

        public static byte Inverse(byte a)
        {
            if (a == 0) throw new ArithmeticException("No inverse for zero.");
            return exp[255 - log[a]];
        }
        public static byte Helper(int i) => exp[i];
        public static byte HelperLog(int i) => log[i];
    }
}