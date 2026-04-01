using System;
using System.Collections.Generic;
using System.Text;
using ReedSolomon;

namespace ReedSolomon
{
    public static  class Encoder
    {
        public static byte[] Encode(byte[] message, int eccLength) // Reed Solomon!
        {
            byte[] messageShifted = new byte[message.Length + eccLength];
            for (int i = 0; i < (messageShifted.Length - eccLength); i++)
            {
                messageShifted[i] = message[i];
            }
            byte[] inductivePolynomial = Generator.Build(eccLength);
            Console.WriteLine($"Inductive: {string.Join(" ", inductivePolynomial)}  // Polynomial form : G(x) = " +
                $"{PolynomialPrinter.PrintPolynomial(inductivePolynomial)}"); 
            byte[] msgShiftedCopy = messageShifted.ToArray();
            byte[] remainder = Polynomial.Divide(msgShiftedCopy, inductivePolynomial);
            Console.WriteLine($"Remainder: {string.Join(" ", remainder)} // Polynomial form: R(x) = " +
                $"{PolynomialPrinter.PrintPolynomial(remainder)}");
            byte[] codeWord = new byte[messageShifted.Length];
            for (int i = 0; i < message.Length; i++)
            {
                codeWord[i] = messageShifted[i];
            }
            for (int i = 0; i < remainder.Length; i++)
            {
                codeWord[i+message.Length] = remainder[i];
            }
            return codeWord;
        }
    }
}
