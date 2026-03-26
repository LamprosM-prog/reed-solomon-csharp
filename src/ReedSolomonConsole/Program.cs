using System;
using System.Diagnostics;
using ReedSolomon;

namespace ReedSolomonConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int t = 4;
            byte[] messageTest = [1, 2, 3, 4];
            byte[] codeWordTest = Encoder.Encode(messageTest, t);
            Console.WriteLine($"The codeword is : {string.Join($" ", codeWordTest)}");
            Console.WriteLine($"Noise corrupted code word:{string.Join(" ",Noise(codeWordTest))}");
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0000}", ts.Milliseconds);
            Console.WriteLine($"Runtime: {elapsedTime} ms");
            Console.ReadKey();
        }

        public static byte[] Noise(byte[] codeWord)
        {
            Random rnd = new Random(255);
            int numberOfErrors = 2; //will be based on how many the errors the user wants
            for (int i = 0; i < numberOfErrors; i++)
            {
                codeWord[rnd.Next(0, codeWord.Length - 1)] = (byte)rnd.Next(0, 255);

            }
            return codeWord;
        }

    } 
}