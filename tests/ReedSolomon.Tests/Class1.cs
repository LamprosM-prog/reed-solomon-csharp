using Xunit;
using ReedSolomon;

namespace ReedSolomon.Tests
{


    public class ChienCalibrationTests
    {
        [Fact]
        public void ChienCalibration()
        {
            byte[] message = new byte[] { 1, 2, 3, 4 };
            byte[] clean = Encoder.Encode(message, 2);

            for (int errorPos = 0; errorPos < clean.Length; errorPos++)
            {
                byte[] corrupted = (byte[])clean.Clone();
                corrupted[errorPos] ^= 0xFF;

                byte[] s = Decoder.ComputeSyndromes(corrupted, 2);
                byte[] lambda = Decoder.BerlekampMassey(s);

                for (int i = 0; i < 255; i++)
                {
                    byte x = GF256.Helper(255 - i);
                    byte eval = Decoder.Evaluate(lambda, x);
                    if (eval == 0)
                        Console.WriteLine($"errorPos={errorPos} → root at i={i}");
                }
            }
        }
    }
}
