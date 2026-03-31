using Microsoft.VisualStudio.TestPlatform.Utilities;
using ReedSolomon;
using Xunit;
using Xunit.Abstractions;

namespace ReedSolomon.Tests
{
    public  class HortonTests
    {
        private readonly ITestOutputHelper _output;

        public HortonTests(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void Evaluate_AtZero_ReturnsConstant()
        {
            byte[] poly = new byte[] { 139, 1 }; // 1 + 139x
            byte result = Decoder.Evaluate(poly, 0);
            Assert.Equal((byte)1, result);
        }
        [Fact]
        public void Evaluate_AtOne_ReturnsXorOfCoeffs()
        {
            byte[] poly = [139, 1];

            byte expected = GF256.Add(139,1);
            byte result = Decoder.Evaluate(poly, (byte)1);

            Assert.Equal((byte)expected, (byte)result);
        }
        [Fact]
        public void Evaluate_MatchesManualComputation()
        {
            byte[] poly = [136, 42, 1]; // 136x^2 + 42x + 1
            byte x = GF256.Helper(3);     //  α^3

            // manual computation: ((136*α^3) + 42)*α^3 + 1
            byte manual = GF256.Add(GF256.Multiply(GF256.Multiply(136, x), x), GF256.Add(GF256.Multiply(42, x), 1));

            byte horner = Decoder.Evaluate(poly, x);

            Assert.Equal(manual, horner);
        }
        [Fact]
        public void Evaluate_AtOne_Debug()
        {
            byte[] poly = { 139, 1 }; // 139x + 1
            byte x = 1;
            byte result = 0;

            for (int i = 0; i < poly.Length; i++)
            {
                byte beforeMult = result;
                result = GF256.Multiply(result, x);
                _output.WriteLine($"Step {i}: Multiply({beforeMult}, {x}) = {result}");

                byte beforeAdd = result;
                result = GF256.Add(result, poly[i]);
                _output.WriteLine($"Step {i}: Add({beforeAdd}, {poly[i]}) = {result}");
            }

            _output.WriteLine($"Final Evaluate result: {result}");
        }
    }
}
