using Xunit;
using ReedSolomon;

namespace ReedSolomon.Tests
{
    public class PolynomialTests
    {
        [Fact]
        public void Add_SameLength()
        {
            byte[] a = [1, 2, 3];
            byte[] b = [4, 5, 6];

            var result = Polynomial.Add(a, b);

            Assert.Equal(
            [
                (byte)(1 ^ 4),
                (byte)(2 ^ 5),
                (byte)(3 ^ 6)
            ], result);
        }

        [Fact]
        public void Add_DifferentLength()
        {
            byte[] a = [ 1, 2, 3 ];
            byte[] b = [ 5 ];

            var result = Polynomial.Add(a, b);

            Assert.Equal(new byte[]
            {
                1,
                2,
                (byte)(3 ^ 5)
            }, result);
        }

        [Fact]
        public void Add_WithZeros()
        {
            byte[] a = [ 0, 0, 1 ];
            byte[] b = [ 1 ];

            var result = Polynomial.Add(a, b);

            Assert.Equal([(byte)(1 ^ 1)], result);
        }

        [Fact]
        public void Add_ResultGetsTrimmed()
        {
            byte[] a = [ 1, 2 ];
            byte[] b = [ 1, 2 ];

            var result = Polynomial.Add(a, b);

            Assert.Equal(new byte[] { 0 }, result);
        }
        [Fact]
        public void Multiply_Basic()
        {
            byte[] a = [1, 1];
            byte[] b = [1, 1];
            var result = Polynomial.Multiply(a, b);
            Assert.Equal(new byte[] { 1, 0, 1 }, result);
        }
        [Fact]
        public void Multiply_ByZero()
        {
            byte[] a = [ 5, 3, 1 ];
            byte[] b = [ 0 ];

            var result = Polynomial.Multiply(a, b);

            Assert.Equal(new byte[] { 0 }, result);
        }
        [Fact]
        public void Multiply_Distributive()
        {
            byte[] a = [ 2, 3 ];
            byte[] b = [ 1, 1 ];
            byte[] c = [ 5 ];

            var left = Polynomial.Multiply(a, Polynomial.Add(b, c));
            var right = Polynomial.Add(
                Polynomial.Multiply(a, b),
                Polynomial.Multiply(a, c)
            );

            Assert.Equal(left, right);
        }
        [Fact]
        public void Divide_Basic()
        {
            byte[] a = [2, 3, 5];
            byte[] b = [1, 1];
            byte[] result = Polynomial.Divide(a,b);
            Assert.Equal(new byte[] { 4 }, result);
            
        }
    
    }
}