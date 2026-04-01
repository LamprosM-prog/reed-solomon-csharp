using System;
using System.Diagnostics;
using System.Text;
using ReedSolomon;

namespace ReedSolomonConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Reed-Solomon Encoder, This encoder uses GF(256) table, \n" +
                "Berlekamp-Massey algorithm, Chien search, and Forney’s algorithm to correct errors in a message");
            Start();
        }
        public static void Start()
        {
            int limit = 64;
            int numberOfErrors = -1;

            while (true)
            {
                Console.WriteLine("Enter how many errors would you like? " +
                    "\nThe theoretical limit is 127, but for complexity reasons the hard limit is 64 " +
                    "\n(Note: For errors higher than 16 expect high latency,\nit is also recommended that an even ammount of errors is selected )");

                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out numberOfErrors))
                {
                    Console.WriteLine("Please enter a valid integer.");
                    continue; 
                }

                if (numberOfErrors <= 0 || numberOfErrors > limit)
                {
                    Console.WriteLine($"Error limit exceeded (must be between 0 and {limit}).");
                    continue; 
                }

                break; 
            }

            
            string messageString = "";
            while (true)
            {
                Console.WriteLine($"Enter message of AT LEAST {numberOfErrors} characters \nASCII characters only:");
                messageString = Console.ReadLine() ?? "";
                if (messageString.Length < numberOfErrors)
                {
                    Console.WriteLine("Message too short.");
                    continue; 
                }
                break; 
            }

            BeginEncode(messageString, numberOfErrors);
        }
        public static void BeginEncode(string messageString, int numberOfErrors)
        {
        
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int t = numberOfErrors * 2;
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            Console.WriteLine($"Byte form of Message:  {string.Join(" ", message)} " +
                $"// Polynomial form : M(x) = {PolynomialPrinter.PrintPolynomial(message)}");
            byte[] codeWord = ReedSolomon.Encoder.Encode(message, t);
            string codewordString = Encoding.ASCII.GetString(codeWord);
            Console.WriteLine($"Codeword in string : {codewordString}");
            Console.WriteLine($"The ReedSolomon codeword is : {string.Join($" ", codeWord)} " +
                $"// Polynomial form C(x) = {PolynomialPrinter.PrintPolynomial(codeWord)}");
            byte[] corrupted = (byte[])codeWord.Clone();
            byte[] codeWordCorrupted = (byte[])Noise(corrupted, numberOfErrors);
            string codewordCorruptedString = Encoding.UTF8.GetString(codeWordCorrupted);
            Console.WriteLine($"Corrupted codeword string : {codewordCorruptedString}");
            Console.WriteLine($"Noise corrupted codeword: {string.Join(" ", codeWordCorrupted)} // Polynomial form {PolynomialPrinter.PrintPolynomial(codeWordCorrupted)}");
            byte[] syndromes = ReedSolomon.Decoder.ComputeSyndromes(codeWordCorrupted, t);
            bool AreThereErrors = ReedSolomon.Decoder.CheckForErrors(syndromes);
            if (AreThereErrors == false)
            {
                Console.WriteLine("No errors detected");
            }
            else
            {
                Console.WriteLine($"Syndromes : {string.Join(" ", syndromes)}");
                byte[] lambda = ReedSolomon.Decoder.BerlekampMassey(syndromes);
                var errorPositions = ReedSolomon.Decoder.ChienSearch(lambda, codeWordCorrupted.Length);
                byte[] codeword = ReedSolomon.Decoder.Forney(lambda, codeWordCorrupted, syndromes, errorPositions);
                Console.WriteLine($"Corrected codeword is = {string.Join(" ", codeword)}");
                byte[] codewordMessage = codeword[..message.Length];
                string originalMessage = Encoding.ASCII.GetString(codewordMessage);
                Console.WriteLine($"Original message : {originalMessage}");
            }
            
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0000}", ts.Milliseconds);
            Console.WriteLine($"Runtime: {elapsedTime} ms");
            Console.WriteLine("Type 'Y' to enter a new message");
            var userInput = Console.ReadLine();
            if (userInput == "Y")
                Start();
            else
                return;
        }
        public static byte[] Noise(byte[] codeWord, int numberOfErrors)
        {
            if (numberOfErrors <= 0) return codeWord;

            Random rnd = new Random();
            int n = codeWord.Length;

            List<int> indices = new List<int>();
            for (int i = 0; i < n; i++) indices.Add(i);


            for (int i = n - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }

            
            for (int k = 0; k < numberOfErrors && k < n; k++)
            {
                int index = indices[k];
                byte original = codeWord[index];
                byte newValue;
                do
                {
                    newValue = (byte)rnd.Next(0, 256);
                } while (newValue == original); 
                codeWord[index] = newValue;
            }

            return codeWord;
        }

    } 
}