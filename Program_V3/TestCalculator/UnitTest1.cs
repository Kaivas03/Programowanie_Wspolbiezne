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

            Assert.AreEqual(calc.Multiplication(5, 6), 30);
            Assert.AreEqual(calc.Multiplication(-1, 1), -1);
            Assert.AreEqual(calc.Multiplication(4444, 2222), 9874568);

            Assert.AreEqual(calc.Division(6, 6), 1);

        }

        [TestMethod]
        public void TestMethod2()
        {

            Assert.AreEqual(calc.Subtraction(5, 6), -1);
            Assert.AreEqual(calc.Subtraction(-1, 1), -2);
            Assert.AreEqual(calc.Subtraction(44444444, 22222222), 22222222);

        }

        [TestMethod]
        public void TestMethod3()
        {

            Assert.AreEqual(calc.Multiplication(5, 6), 30);
            Assert.AreEqual(calc.Multiplication(-1, 1), -1);
            Assert.AreEqual(calc.Multiplication(4444, 2222), 9874568);

        }

        [TestMethod]
        public void TestMethod4()
        {

            Assert.AreEqual(calc.Division(6, 6), 1);
            Assert.AreEqual(calc.Division(12, -6), -2);
            Assert.AreEqual(calc.Division(12, 0), 1);
            Assert.AreEqual(calc.Division(4444, 2222), 2);

        }

    }
}