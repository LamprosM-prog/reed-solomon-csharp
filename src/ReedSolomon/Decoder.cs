using System;
using System.Collections.Generic;
using System.Text;

namespace ReedSolomon
{
    public static class Decoder
    {
   
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
            byte b = 1;
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
                    for(int j = 0; j < bestL.Length; j++)
                    {
                     
                        int target = j + shift;
                       
                        if (target < lambda.Length)
                        {
                            lambda[target] = GF256.Add(lambda[target],
                        GF256.Multiply(GF256.Divide(delta, b), bestL[j]));
                        } 
                       
                    }
                    if (2 * degree <= k) 
                    {
                        degree = k + 1 - degree;
                        bestL = temp;
                        b = delta;
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
            Console.WriteLine($"Lambda : {string.Join(" ",lambda)} // Lambda Polynomial Form Λ(χ) = {PolynomialPrinter.PrintPolynomial(lambda)}");
            return lambda; 
        }
        public static List<int> ChienSearch(byte[] lambda, int codewordLength)
        {
           lambda = Polynomial.Trim(lambda);
            Console.WriteLine(string.Join(" ",lambda));
            List<int> errorPositions = new List<int>();
            for (int i = 0; i <= 255; i++)
            {
                byte x = GF256.Helper(i);
                byte eval = Polynomial.Evaluate(lambda, x);
                if (eval == 0)
                {
                    int locatorExp = 255 - i;
                    int position = (codewordLength - 1) - locatorExp;
                    if (position >= 0 && position < codewordLength)
                        errorPositions.Add(position);
                    /*
                     Memonatory Note! This 4 lines took me 9 hours to write!
                    This is because this part of the code uncovered 6 bugs:
                   1) b = delta missing in BM 
                   2) BM shift direction — j = i - shift vs j + shift
                   3) Lambda not trimmed before reversing — leading zero corrupting Chien
                   4) Chien position mapping — wrong formula for root-to-index conversion
                   5) Reference bug in Main — codeWordCorrupted = codeWordTest sharing same array 
                   6) Degree function — result of Trim being discarded
                     */
                }
            }
            Console.WriteLine($"Error positions:  {string.Join(" ", errorPositions)} ");
            return errorPositions;
        }
        public static byte[] Forney(byte[] lambda, byte[] codeword, byte[] syndromes, List<int> errorPositions) //Error correction will be handled here too
        {
            byte[] syndromePoly =  syndromes.Reverse().ToArray();
            byte[] omega = Polynomial.Multiply(syndromePoly, lambda);
            byte[] lambdaDerivative = Polynomial.Derivative(lambda); //Λ'(x) 
            int twoT = syndromes.Length;
            if(omega.Length > twoT)
                omega = omega[^twoT..];
      
            foreach(int errorPosition in errorPositions)
            {
                
                int locatorExp = (codeword.Length - 1) - errorPosition;
                byte xk = GF256.Helper(locatorExp);
                byte xkInv = GF256.Inverse(xk);
                byte num = Polynomial.Evaluate(omega, xkInv);
                byte denominator = Polynomial.Evaluate(lambdaDerivative, xkInv);
                byte errorMagn =  GF256.Divide(num,denominator);
                //This is the error correction, forney's algorithm just returns the error magnitudes
                codeword[errorPosition] = GF256.Add(codeword[errorPosition], errorMagn); 
            }

            return codeword;
        }
    }
    
}
