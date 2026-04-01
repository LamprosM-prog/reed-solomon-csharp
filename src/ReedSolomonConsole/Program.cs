using System;
using System.Diagnostics;   
using ReedSolomon;

namespace ReedSolomonConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(" ");
            
            Console.WriteLine("Enter how many errors would you like? \n The theoretical limit is 127," +
                "but for complexity reasons the hard limit is 64 \n (Note: For errors higher than 16 expect high latency)");
            string numberOfErrors = Console.ReadLine();
            
            
            
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            
            
            
            
            
            int t = 4;
            byte[] messageTest = [1, 2, 3, 4];
            Console.WriteLine($"Message:{string.Join(" ", messageTest)} // Polynomial form {PolynomialPrinter.PrintPolynomial(messageTest)}");
            byte[] codeWordTest = Encoder.Encode(messageTest, t);
            Console.WriteLine($"The codeword is : {string.Join($" ", codeWordTest)} // Polynomial form {PolynomialPrinter.PrintPolynomial(codeWordTest)}");
            byte[] codeWordCorrupted = (byte[])Noise(codeWordTest);
            Console.WriteLine($"Noise corrupted codeword: {string.Join(" ", codeWordCorrupted)} // Polynomial form {PolynomialPrinter.PrintPolynomial(codeWordTest)}");
            byte[] syndromes = Decoder.ComputeSyndromes(codeWordTest, t);
            bool AreThereErrors = Decoder.CheckForErrors(syndromes);
            if (AreThereErrors == false)
            {
                Console.WriteLine("No errors detected");
            }
            else
            {
                Console.WriteLine($"Syndromes : {string.Join(" ", syndromes)}");
                byte[] lambda = Decoder.BerlekampMassey(syndromes);
                var errorPositions = Decoder.ChienSearch(lambda, codeWordCorrupted.Length);
                byte[] codeword = Decoder.Forney(lambda, codeWordCorrupted, syndromes, errorPositions);
                Console.WriteLine($"Corrected codeword is = {string.Join(" ", codeword)}");
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0000}", ts.Milliseconds);
            Console.WriteLine($"Runtime: {elapsedTime} ms");
            Console.ReadKey();
        }
        public static void Start()
        {

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