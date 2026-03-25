using System;
using System.Collections.Generic;
using System.Text;
using ReedSolomon;

namespace ReedSolomon
{
    public static  class Encoder
    {
        public static byte[] Encode(byte[] message, int eccLength)
        {
            byte[] messageShifted = new byte[message.Length + eccLength];
            for (int i = 0; i < (messageShifted.Length - eccLength); i++)
            {
                messageShifted[i] = message[i];
            }
            byte[] inductivePolynomial = Generator.Build(eccLength);
            Console.WriteLine(string.Join(" ", inductivePolynomial)); //remove all console.writeline later
            byte[] msgShiftedCopy = messageShifted.ToArray();
            byte[] remainder = Polynomial.Divide(msgShiftedCopy, inductivePolynomial);
            Console.WriteLine(string.Join(" ", remainder));
            byte[] codeWord = new byte[messageShifted.Length];
            for (int i = 0; i < message.Length; i++)
            {
                codeWord[i] = messageShifted[i];
            }
            for (int i = 0; i < remainder.Length; i++)
            {
                codeWord[i+remainder.Length] = remainder[i];
            }
            return codeWord;
        }
    }
}
