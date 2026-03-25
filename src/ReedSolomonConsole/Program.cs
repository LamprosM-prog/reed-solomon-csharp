using System;
using ReedSolomon;

namespace ReedSolomonConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int t = 4;
            byte[] messageTest = [1, 2, 3, 4];
            byte[] codeWordTest = Encoder.Encode(messageTest, 4);           
            Console.WriteLine(string.Join($" ", codeWordTest));
            Console.ReadKey();
        }
    }
}