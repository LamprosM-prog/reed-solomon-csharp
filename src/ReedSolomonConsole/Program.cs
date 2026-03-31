using System;
using System.Diagnostics;
using ReedSolomon;

namespace ReedSolomonConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine($"exp[0]={GF256.Helper(0)} exp[1]={GF256.Helper(1)} exp[255]={GF256.Helper(255)}");
            //Console.WriteLine($"log[255]={GF256.HelperLog(255)}");
            //Console.WriteLine($"exp[175]={GF256.Helper(175)}");
            //Console.WriteLine($"exp[95]={GF256.Helper(95)}");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //byte[] message = new byte[] { 1, 2, 3, 4 };
            //byte[] clean = Encoder.Encode(message, 2);

            //for (int errorPos = 0; errorPos < clean.Length; errorPos++)
            //{
            //    byte[] corrupted = (byte[])clean.Clone();
            //    corrupted[errorPos] ^= 0xFF;

            //    byte[] s = Decoder.ComputeSyndromes(corrupted, 2);
            //    Console.WriteLine($"errorPos={errorPos} S=[{string.Join(",", s)}] ratio={GF256.Divide(s[1], s[0])}");
            //    byte[] lam = Decoder.BerlekampMassey(s);

            //    for (int i = 0; i < 255; i++)
            //    {
            //        byte x = GF256.Helper(255 - i);
            //        byte eval = Decoder.Evaluate(lam, x);
            //        if (eval == 0)
            //            Console.WriteLine($"errorPos={errorPos} -> root at i={i}");
            //    }
            //}












            int t = 2;
            byte[] messageTest = [1, 2, 3, 4];
            Console.WriteLine($"Message:{string.Join(" ", messageTest)}");
            byte[] codeWordTest = Encoder.Encode(messageTest, t);
            Console.WriteLine($"The codeword is : {string.Join($" ", codeWordTest)}");
            byte[] codeWordCorrupted = codeWordTest;
            codeWordTest[2] = 40;
            Console.WriteLine($"Noise corrupted codeword: {string.Join(" ", codeWordCorrupted)}");
            byte[] syndromes = Decoder.ComputeSyndromes(codeWordTest, t);
            Console.WriteLine($"ratio={GF256.Divide(syndromes[1], syndromes[0])}");
            Console.WriteLine($"log[s0]={GF256.HelperLog(syndromes[0])} log[s1]={GF256.HelperLog(syndromes[1])}");
            Console.WriteLine($"Inverse(139)={GF256.Inverse(139)}");
            Console.WriteLine($"Helper(3)={GF256.Helper(3)}");
            bool AreThereErrors = Decoder.CheckForErrors(syndromes);
            if (AreThereErrors == false)
            {
                Console.WriteLine("No errors detected");
            }
            else
            {
                Console.WriteLine($"Syndromes : {string.Join(" ", syndromes)}");
                byte[] lambda = Decoder.BerlekampMassey(syndromes);
                Decoder.ChienSearch(lambda, codeWordCorrupted.Length);
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