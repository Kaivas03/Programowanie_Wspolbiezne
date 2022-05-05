using Logic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test
{
    [TestClass]
    public class LogicTest
    {

        private LogicAbstractApi LApi;
        private Mock<TimerApi> timer;


        [TestMethod]
        public void getCount()
        {
            timer = new Mock<TimerApi>();
            LApi = LogicAbstractApi.CreateApi(800, 600, timer.Object);
            LApi.CreateBallsList(5);
            Assert.AreEqual(5, LApi.GetCount);
            LApi.CreateBallsList(-3);
            Assert.AreEqual(2, LApi.GetCount);
            LApi.CreateBallsList(-3);
            Assert.AreEqual(0, LApi.GetCount);
        }

        [TestMethod]
        public void getRadiusInterval()
        {
            timer = new Mock<TimerApi>();
            LApi = LogicAbstractApi.CreateApi(800, 600, timer.Object);
            LApi.CreateBallsList(1);
            int radius = LApi.GetSize(0);
            Assert.IsTrue(radius >= 20);
            Assert.IsTrue(radius <= 40);
        }
        [TestMethod]
        public void getValidCoordinates()
        {
            timer = new Mock<TimerApi>();
            LApi = LogicAbstractApi.CreateApi(800, 600, timer.Object);
            Assert.AreEqual(800, LApi.Width);
            Assert.AreEqual(600, LApi.Height);
            LApi.CreateBallsList(1);
            int radius = LApi.GetSize(0);
            int x = LApi.GetX(0);
            int y = LApi.GetY(0);
            Assert.IsTrue(x >= radius);
            Assert.IsTrue(y >= radius);
            Assert.IsTrue(x <= LApi.Width - radius);
            Assert.IsTrue(y <= LApi.Height - radius);
        }


    }
}