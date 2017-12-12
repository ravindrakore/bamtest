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
    public class OrderMarkerUnitTests
    {
        private readonly Mock<IPositionCacheService> positionServiceMock;
        private readonly Mock<IOrderRoutingService> orderRoutingServiceMock;
        private readonly Mock<ISecurityMasterService> securityMasterServiceMock;
        private OrderMarker ordermarker;
        public OrderMarkerUnitTests()
        {
            this.positionServiceMock = new Mock<IPositionCacheService>();
            this.orderRoutingServiceMock = new Mock<IOrderRoutingService>();
            this.securityMasterServiceMock = new Mock<ISecurityMasterService>();
            this.ordermarker = new OrderMarker(positionServiceMock.Object, securityMasterServiceMock.Object);
        }

        [TestMethod]
        public void Equity_OrderProcessing_BuyOrder()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" };
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

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Buy, Portfolio = "TraderX", Price = 100M, Quantity = 100, SecurityMasterId = 1 }
            });

            Assert.AreEqual(1, markedOrders.Count());
        }

        [TestMethod]
        public void Option_OrderProcessing_BuyOrder()
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

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Buy, Portfolio = "TraderX", Price = 100M, Quantity = 100, SecurityMasterId = 1 }
            });

            Assert.AreEqual(1, markedOrders.Count());
        }

        [TestMethod]
        public void Equity_OrderProcessing_BuyOrder_MarkedAs_Buy_OrderMakerType()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" };
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

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Buy, Portfolio = "TraderX", Price = 100M, Quantity = 100, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(OrderMarkerType.Buy, markedOrders[0].OrderMarkerType);
        }

        [TestMethod]
        public void Option_OrderProcessing_BuyOrder_MarkedAs_BuyToOpen_OrderMakerType()
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

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Buy, Portfolio = "TraderX", Price = 100M, Quantity = 100, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(OrderMarkerType.BuyToOpen, markedOrders[0].OrderMarkerType);
        }

        [TestMethod]
        public void Equity_OrderProcessing_SellOrder_MarkedAs_SellLong_OrderMakerType()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" };
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

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Sell, Portfolio = "TraderX", Price = 100M, Quantity = 50, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(OrderMarkerType.SellLong, markedOrders[0].OrderMarkerType);
        }

        [TestMethod]
        public void Option_OrderProcessing_SellOrder_MarkedAs_SellToClose_OrderMakerType()
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

            var markedOrder = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Sell, Portfolio = "TraderX", Price = 100M, Quantity = 50, SecurityMasterId = 1 }
            }).First();

            Assert.AreEqual(OrderMarkerType.SellToClose, markedOrder.OrderMarkerType);
        }


        [TestMethod]
        public void Equity_OrderProcessing_SellOrder_MarkedAs_Split_SellLongAndSellShort_OrderMakerType()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" };
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

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Sell, Portfolio = "TraderX", Price = 100M, Quantity = 150, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(2, markedOrders.Count);
            Assert.AreEqual(OrderMarkerType.SellShort, markedOrders[1].OrderMarkerType);
            Assert.AreEqual(OrderMarkerType.SellLong, markedOrders[0].OrderMarkerType);
        }

        [TestMethod]
        public void Option_OrderProcessing_SellOrder_MarkedAs_Split_SellToCloseAndSellToOpen_OrderMakerType()
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

            var markedOrders =  this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Sell, Portfolio = "TraderX", Price = 100M, Quantity = 150, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(2, markedOrders.Count);
            Assert.AreEqual(OrderMarkerType.SellToOpen, markedOrders[1].OrderMarkerType);
            Assert.AreEqual(OrderMarkerType.SellToClose, markedOrders[0].OrderMarkerType);
        }

        [TestMethod]
        public void Equity_OrderProcessing_SellOrder_MarkedAs_SellShort_OrderMakerType()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" };
            this.securityMasterServiceMock.Setup(s => s.GetSecurityById(1)).Returns(ibmSecurity);

            this.positionServiceMock.Setup(p => p.GetPositionByKey(It.IsAny<PositionKey>())).Returns((IPosition)null);

            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Sell, Portfolio = "TraderX", Price = 100M, Quantity = 150, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(1, markedOrders.Count);
            Assert.AreEqual(OrderMarkerType.SellShort, markedOrders[0].OrderMarkerType);
        }

        [TestMethod]
        public void Option_OrderProcessing_SellOrder_MarkedAs_SellToOpen_OrderMakerType()
        {
            var ibmSecurity = new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Option, Ticker = "GOOGL" };
            this.securityMasterServiceMock.Setup(s => s.GetSecurityById(1)).Returns(ibmSecurity);

            this.positionServiceMock.Setup(p => p.GetPositionByKey(It.IsAny<PositionKey>())).Returns((IPosition)null);
                       
            var markedOrders = this.ordermarker.MarkOrders(new List<IOrder>()
            {
                new Order() { OrderType = OrderType.Sell, Portfolio = "TraderX", Price = 100M, Quantity = 150, SecurityMasterId = 1 }
            }).ToList();

            Assert.AreEqual(1, markedOrders.Count);
            Assert.AreEqual(OrderMarkerType.SellToOpen, markedOrders[0].OrderMarkerType);
        }
    }
}
