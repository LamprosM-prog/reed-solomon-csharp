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
            Console.WriteLine($"Message:{string.Join(" ", messageTest)}");
            byte[] codeWordTest = Encoder.Encode(messageTest, t);
            Console.WriteLine($"The codeword is : {string.Join($" ", codeWordTest)}");
            byte[] codeWordCorrupted = Noise(codeWordTest);
            Console.WriteLine($"Noise corrupted codeword: {string.Join(" ", codeWordCorrupted)}");
            byte[] syndromes = Decoder.ComputeSyndromes(codeWordTest, t);
            bool AreThereErrors = Decoder.CheckForErrors(syndromes);
            if (AreThereErrors == false) {
                Console.WriteLine("No errors detected");            
            }
            else
            {
                Console.WriteLine($"Syndromes : {string.Join(" ", syndromes)}");
                Decoder.BerlekampMassey(syndromes);
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0000}", ts.Milliseconds);
            Console.WriteLine($"Runtime: {elapsedTime} ms");
            Console.ReadKey();
        }

        public static byte[] Noise(byte[] codeWord)
        {
            Random rnd = new Random();
            int numberOfErrors = 2; //Will be based on user
            for (int i = 0; i < numberOfErrors; i++)
            {
                int index = rnd.Next(0, codeWord.Length);
                codeWord[index] = (byte)rnd.Next(0, 256);
            }
            return codeWord;
        }

    } 
}