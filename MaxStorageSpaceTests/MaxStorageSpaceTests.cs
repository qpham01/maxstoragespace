using NUnit.Framework;

namespace MaxStorageSpaceTests
{
    public class MaxStorageSpaceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(3L, 3L, new long[0], new long[0], 1L)]
        [TestCase(3L, 3L, new long[0], new[] { 1L }, 2L)]
        [TestCase(3L, 3L, new[] { 2L }, new long[0], 2L)]
        [TestCase(3L, 3L, new[] { 1L }, new[] { 1L }, 4L)]
        [TestCase(3L, 3L, new[] { 1L, 2L }, new[] { 1L, 2L }, 9L)]
        [TestCase(6L, 6L, new[] { 1L, 2L }, new[] { 1L, 2L }, 9L)]
        [TestCase(6L, 6L, new []{ 4L }, new []{ 2L }, 4L)]
        [TestCase(6L, 6L, new[] { 1L, 2L }, new[] { 1L, 2L }, 9L)]
        [TestCase(6L, 6L, new[] { 4L, 5L }, new[] { 4L, 5L }, 9L)]
        [TestCase(6L, 6L, new[] { 3L, 4L, 5L }, new[] { 4L, 5L }, 12L)]
        [TestCase(6L, 6L, new[] { 3L, 4L, 5L }, new[] { 1L, 4L, 5L }, 12L)]
        [TestCase(6L, 6L, new[] { 3L, 4L, 5L }, new[] { 2L, 4L, 5L }, 12L)]
        [TestCase(6L, 6L, new[] { 3L, 4L, 5L }, new[] { 3L, 4L, 5L }, 16L)]
        [TestCase(10000L, 10000L, new[] { 5003L, 5004L, 5005L }, 
            new[] { 5003L, 5004L, 5005L }, 16L)]
        [TestCase(10000000000L, 10000000000L, new[] { 5000000003L, 5000000004L, 5000000005L }, 
            new[] { 5000000003L, 5000000004L, 5000000005L }, 16L)]
        [TestCase(10000000000L, 10000000000L, new[] { 5000000003L, 5000000004L, 5000000005L, 27L },
            new[] { 5000000003L, 5000000004L, 5000000005L, 4992L }, 16L)]
        public void TestCalculate(long n, long m, long[] h, long[] v, long expected)
        {
            var maxSize = MaxStorageSpace.MaxStorageSpace.Calculate(n, m, h, v);
            Assert.AreEqual(expected, maxSize);
        }
    }
}