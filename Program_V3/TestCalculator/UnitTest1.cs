using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCalculator
{
    [TestClass]
    public class UnitTest1 { 

        private Calculator.Calculator calc;


    [TestInitialize]
    public void Initialize()
    {
        calc = new Calculator.Calculator();
    }

    [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(calc.Addition(5, 6), 11);
            Assert.AreEqual(calc.Addition(-1, 1), 0);
            Assert.AreEqual(calc.Addition(44444444, 22222222), 66666666);

        }

        public void TestMethod2()
        {
            Assert.AreEqual(calc.Subtraction(5, 6), 11);
            Assert.AreEqual(calc.Subtraction(-1, 1), 0);
            Assert.AreEqual(calc.Subtraction(44444444, 22222222), 66666666);

        }
    }
}