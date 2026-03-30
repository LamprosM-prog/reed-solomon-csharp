using System;
using System.Collections.Generic;
using System.Text;

namespace ReedSolomon
{
    public static class Decoder
    {
        private static byte Evaluate(byte[] lambda, byte x) //Horner's scheme
        {
            byte result = 0;
            for (int i = lambda.Length - 1; i >= 0; i--) 
            {
                result = GF256.Multiply(lambda[i], x);
                result = GF256.Add(result, x);   
            }
            return result;
        }
        
        
        public static byte[] ComputeSyndromes(byte[] codeword, int eccLength)
        {
            byte[] syndromes = new byte[eccLength];
            for (int i = 0; i < eccLength; i++)
            {
                byte x = GF256.Helper(i + 1); //prime element
                byte result = 0;
                foreach (byte coeff in codeword) //  !!!!!Need all the coefs of the codeword not just the ECC!!!!!
                {
                    result = GF256.Add(GF256.Multiply(result, x), coeff);
                }
                syndromes[i] = result;
            }
            return syndromes;
        }
        public static bool CheckForErrors(byte[] syndromes) => !syndromes.All(x => x == 0);
        
        //lowest  index = constant
        public static byte[] BerlekampMassey(byte[] syndromes) 
        {
            byte[] lambda = new byte[syndromes.Length + 1];
            byte[] bestL = new byte[lambda.Length];
            lambda[0] = 1;
            bestL[0] = 1;
            int degree = 0;
            int m = -1;
            int k = 0;
            while (k < syndromes.Length)
            {
                byte delta = syndromes[k];
                for (int i = 1; i <= degree; i++)
                {
                    delta = GF256.Add(delta,GF256.Multiply(lambda[i], syndromes[k - i])); //black satanic magic v2
                }
                if (delta != 0)
                {
                    byte[] temp = (byte[])lambda.Clone();
                    int shift = k - m;
                    // λ(x) = λ(x) - δ * x^(k - m) * B(x)
                    for(int i = 0; i < bestL.Length; i++)
                    {
                        int j = i - shift;
                        if(j >= 0 && j < bestL.Length)
                        {
                            lambda[i] = GF256.Add(lambda[i], GF256.Multiply(delta, bestL[j]));
                        } 
                       
                    }
                    if (2 * degree <= k) 
                    {
                        degree = k + 1 - degree;
                        bestL = temp;
                        m = k;
                    }
                }

                k++;
            }


            Console.WriteLine($"Lambda : {string.Join(" ",lambda)} // Lambda Polynomial Form {PolynomialPrinter.PrintPolynomial(lambda, true)}");
            return lambda; 
        }
        public static List<int> ChienSearch(byte[] lambda, int n)
        {
            List<int> errorPositions = new List<int>();

            for (int i = 0; i < n; i++)
            {
                // x = α^(-i) = α^(255 - i)
                byte x = GF256.Helper((255 - i) % 255);
                byte y = Evaluate(lambda, x);
                if (y == 0)
                {
                    int index = n - 1 - i;
                    errorPositions.Add(index);
                }
               
            }
            Console.WriteLine($"Degree of Λ(χ): {Polynomial.Degree(lambda)}");
            if(Polynomial.Degree(lambda) == errorPositions.Count)
            {
                Console.WriteLine($"Degree of Λ(χ) is equal to the errors. Test successful");
            }
            else
            {
                Console.WriteLine("Error in chien search");
            }
            Console.WriteLine($"Number of errors {errorPositions.Count}");
            Console.WriteLine($"Positions : {string.Join(" ", errorPositions)} ");
            return errorPositions;
        }





    }
}
