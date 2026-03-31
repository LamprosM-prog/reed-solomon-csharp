using System;
using System.Collections.Generic;
using System.Text;

namespace ReedSolomon
{
    public static class Decoder
    {
        public static byte Evaluate(byte[] poly, byte x) //Horton scheme
        {
            byte result = 0;
            for(int i = 0; i < poly.Length; i++)
            {
                result = GF256.Multiply(result, x);
                result = GF256.Add(result, poly[i]);

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
        public static bool CheckForErrors(byte[] syndromes) => syndromes.All(x => x == 0) ? false : true;
        
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
                    delta = GF256.Add(delta,
                             GF256.Multiply(lambda[i], syndromes[k - i])); //black satanic magic v2
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
            /*
             IMPORANT: To generate the polynomial λ(χ) we made it so 
            that the lowest index is the constant.But the polynomial operations
            and further functions treat polynomials as highest index = constant.
            This requires to reverse lambda before  returning it.
             */
            int lastNonZero = lambda.Length - 1;
            while (lastNonZero > 0 && lambda[lastNonZero] == 0)
                lastNonZero--;
            lambda = lambda.Take(lastNonZero + 1).ToArray();
            Array.Reverse(lambda);
            Console.WriteLine($"Lambda : {string.Join(" ",lambda)} // Lambda Polynomial Form {PolynomialPrinter.PrintPolynomial(lambda)}");
            return lambda; 
        }
        public static List<int> ChienSearch(byte[] lambda, int codewordLength)
        {
           lambda = Polynomial.Trim(lambda);
            Console.WriteLine(string.Join(" ",lambda));
            List<int> errorPositions = new List<int>();
            for (int i = 0; i < 255; i++)
            {
                byte x = GF256.Helper(255 - i);
                byte eval = Evaluate(lambda, x);
                if (eval == 0)
                {
                    Console.WriteLine($"Root found at i={i}, maps to position ???");
                }
            }
            Console.WriteLine($"Error positions:  {string.Join(" ", errorPositions)} ");
            return errorPositions;
        }
    
    }
    



}
