using Xunit;

namespace CalculatorApp.Test
{
    public class MathHelperTest
    {
        private readonly MathFormulas MathFormulas;
        public MathHelperTest()
        {
            MathFormulas = new MathFormulas();
        }

        [Fact]      // This method does not take any params, so Fact
        public void IsEven_WhenNumberPassed_ReturnsResult()
        {
            Assert.False(MathFormulas.IsEven(1));
            Assert.True(MathFormulas.IsEven(2));
        }

        [Theory]            // Provide test data as parameters, so theory
        [InlineData(10, 5, -5)]
        [InlineData(2, 3, 1)]
        public void Diff_WhenCalled_ReturnsDifference(int x, int y, int diff)
        {
            Assert.Equal(diff, MathFormulas.Diff(x, y));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 2, 2)]
        [InlineData(4, 2, 0, 2)]
        [InlineData(-4, -2, 10, -2, -10)]
        public void Sum_WhenCalled_ReturnsSum(int expected, params int[] inputs)
        {
            Assert.Equal(expected, MathFormulas.Sum(inputs));
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(-1, -1, -2)]
        [InlineData(-10, 11, 1)]
        public void Add_WhenCalled_ReturnsAddedResult(int x, int y, int expected)
        {
            Assert.Equal(expected, MathFormulas.Add(x, y));
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(1, 2, 1)]
        public void Average_WhenCalled_ReturnsAvg(int expected, params int[] inputs)
        {
            Assert.Equal(expected, MathFormulas.Average(inputs));
        }

        [Theory]
        [MemberData(nameof(MathFormulas.Data), MemberType = typeof(MathFormulas))]
        public void Add_WhenCalled_RetunsSum(int x, int y, int expected)
        {
            Assert.Equal(expected, MathFormulas.Add(x, y));
        }

        [Theory]
        [ClassData(typeof(MathFormulas))]
        public void Add_WhenCalledWithClassData_ReturnsSum(int x, int y, int expected)
        {
            Assert.Equal(expected, MathFormulas.Add(x, y));
        }

        [Theory(Skip = "This is just a test")]
        [ClassData(typeof(MathFormulas))]
        public void Add_WhenCalledWithClassData_ReturnsSumSkip(int x, int y, int expected)
        {
            Assert.Equal(expected, MathFormulas.Add(x, y));
        }
    }
}
