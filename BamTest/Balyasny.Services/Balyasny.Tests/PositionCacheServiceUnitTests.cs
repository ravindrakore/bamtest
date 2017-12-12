using Balyasny.Common.Implementation;
using Balyasny.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Balyasny.Tests
{
    [TestClass]
    public class PositionCacheServiceUnitTests
    {
        [TestMethod]
        public void NewOrder_CreatesPosition()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100.45M,
                Quantity =100
            });

            var count = service.GetCurrentPositions().Count();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void GetPositionByKey_ValidPortFolio()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100.45M,
                Quantity = 100
            });

            var pos = service.GetPositionByKey(PositionKey.Create("TraderX", 1));
            Assert.IsNotNull(pos);
        }

        [TestMethod]
        public void GetPositionByKey_InValidPortFolio()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100.45M,
                Quantity = 100
            });

            var pos = service.GetPositionByKey(PositionKey.Create("Trader", 1));
            Assert.IsNull(pos);
        }

        [TestMethod]
        public void GetPositionByKey_Multitrades()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100.45M,
                Quantity = 100
            });

            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 200.00M,
                Quantity = 100
            });

            var posCount = service.GetCurrentPositions().Count();
            Assert.AreEqual(1, posCount);
        }


        [TestMethod]
        public void GetPositionByKey_Multitrades_ValidQuantity()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100.45M,
                Quantity = 100
            });

            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 200.00M,
                Quantity = 100
            });

            var pos = service.GetPositionByKey(PositionKey.Create("TraderX", 1));
            Assert.AreEqual(200, pos.Quantity);
        }

        [TestMethod]
        public void GetPositionByKey_Multitrades_ValidAvgPrice()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100M,
                Quantity = 100
            });

            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 200.00M,
                Quantity = 100
            });

            var pos = service.GetPositionByKey(PositionKey.Create("TraderX", 1));
            Assert.AreEqual(150, pos.AvgPrice);
        }

        [TestMethod]
        public void GetPositionByKey_Multitrades_ChangeSide()
        {
            var service = new PositionCacheService();
            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Buy,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 100M,
                Quantity = 100
            });

            service.SubmitOrder(new Order()
            {
                OrderType = Common.OrderType.Sell,
                SecurityMasterId = 1,
                Portfolio = "TraderX",
                Price = 200.00M,
                Quantity = 200
            });

            var pos = service.GetPositionByKey(PositionKey.Create("TraderX", 1));
            Assert.IsTrue(pos.IsShort);
        }
    }
}
