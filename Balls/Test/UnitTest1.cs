using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test
{
    [TestClass]
    public class BallTest
    {
        private readonly Ball ball = new Ball(1, 5, 5, 2, 2);
        [TestMethod]
        public void getSize()
        {
            Assert.AreEqual(ball.Size, 1);
        }
        [TestMethod]
        public void getCoordinates()
        {
            Assert.AreEqual(ball.X, 5);
            Assert.AreEqual(ball.Y, 5);
        }
        [TestMethod]
        public void getNewPosition()
        {
            ball.newPosition(8, 8);
            Assert.AreEqual(ball.X, 7);
            Assert.AreEqual(ball.Y, 7);
            ball.newPosition(8, 8);
            Assert.AreEqual(ball.X, 7);
            Assert.AreEqual(ball.Y, 7);
            ball.newPosition(8, 8);
            Assert.AreEqual(ball.X, 5);
            Assert.AreEqual(ball.Y, 5);
        }
    }
}