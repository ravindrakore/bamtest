using Microsoft.VisualStudio.TestTools.UnitTesting;
using Balyasny.Services.Implementation;
using Moq;
using Balyasny.Services;
using Balyasny.Common.Implementation;
using Balyasny.Common;
using System.Collections.Generic;
using System.Linq;

namespace Balyasny.Tests
{
    [TestClass]
    public class OrderServiceUnitTests
    {
        private readonly Mock<IPositionCacheService> positionServiceMock;
        private readonly Mock<IOrderRoutingService> orderRoutingServiceMock;
        private readonly Mock<ISecurityMasterService> securityMasterServiceMock;
        private IOrderService orderService;
        public OrderServiceUnitTests()
        {
            this.positionServiceMock = new Mock<IPositionCacheService>();
            this.orderRoutingServiceMock = new Mock<IOrderRoutingService>();
            this.securityMasterServiceMock = new Mock<ISecurityMasterService>();
            orderService = new OrderService(positionServiceMock.Object, securityMasterServiceMock.Object, orderRoutingServiceMock.Object);
        }

        [TestMethod]
        public void Equity_OrderProcessing_SubmitsOrderToRouter()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" };
            this.securityMasterServiceMock.Setup(s => s.GetSecurityById(1)).Returns(ibmSecurity);

            var ibmpos = new Position()
            {
                AvgPrice = 100M, IsShort = false, Portfolio = "TraderX", Quantity = 100, SecurityMasterId = 1
            };

            this.positionServiceMock.Setup(p => p.GetPositionByKey(It.IsAny<PositionKey>())).Returns(ibmpos);

            var MarkedOrderCount = 0;
            this.orderRoutingServiceMock.Setup(or => or.Route(It.IsAny<IEnumerable<IOrder>>())).Callback((IEnumerable<IOrder> orders) => { MarkedOrderCount = orders.Count(); });

            this.orderService.ProcessOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Buy, Portfolio = "TraderX", Price = 100M, Quantity = 100, SecurityMasterId = 1 }
            });

            Assert.AreEqual(1, MarkedOrderCount);            
        }

        [TestMethod]
        public void Option_OrderProcessing_SubmitsOrderToRouter()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Option, Ticker = "GOOGL" };
            this.securityMasterServiceMock.Setup(s => s.GetSecurityById(1)).Returns(ibmSecurity);

            var ibmpos = new Position()
            {
                AvgPrice = 100M,
                IsShort = false,
                Portfolio = "TraderX",
                Quantity = 100,
                SecurityMasterId = 1
            };

            this.positionServiceMock.Setup(p => p.GetPositionByKey(It.IsAny<PositionKey>())).Returns(ibmpos);

            var MarkedOrderCount = 0;
            this.orderRoutingServiceMock.Setup(or => or.Route(It.IsAny<IEnumerable<IOrder>>())).Callback((IEnumerable<IOrder> orders) => { MarkedOrderCount = orders.Count(); });

            this.orderService.ProcessOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Buy, Portfolio = "TraderX", Price = 100M, Quantity = 100, SecurityMasterId = 1 }
            });

            Assert.AreEqual(1, MarkedOrderCount);
        }           
    }
}
