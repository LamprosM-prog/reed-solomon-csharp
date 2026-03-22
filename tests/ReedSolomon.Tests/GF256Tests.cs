using ReedSolomon;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ReedSolomon.Tests
{
    public class GF256Debug
    {
        private readonly ITestOutputHelper _output;

        public GF256Debug(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void PrintMultiplicationTable()
        {
            _output.WriteLine("GF(2^8) Multiplication Table (first 16 values):");

            for (byte i = 0; i < 16; i++)
            {
                for (byte j = 0; j < 16; j++)
                {
                    _output.WriteLine($"{GF256.Multiply(i, j),3} ");
                }
                _output.WriteLine("");
            }
        }

        [Fact]
        public void PrintDivisionTable()
        {
            _output.WriteLine("GF(2^8) Division Table (first 16 values):");

            for (byte i = 1; i < 16; i++)
            {
                for (byte j = 1; j < 16; j++)
                {
                    _output.WriteLine($"{GF256.Divide(i, j),3} ");
                }
                _output.WriteLine("");
            }
        }

        [Fact]
        public void GeneralTest()
        {
            byte a = 45;
            byte b = 7;
            byte c = GF256.Multiply(a, b);
            byte d = GF256.Divide(c, b);
            byte inv = GF256.Inverse(a);

            _output.WriteLine($"Check: {a} * {b} / {b} = {d}");
            _output.WriteLine($"Inverse of {a} = {inv}");
        }


    }  
}