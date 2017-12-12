using Balyasny.Common;
using Balyasny.Common.Implementation;
using System.Collections.Generic;

namespace Balyasny.Services.Implementation
{
    public abstract class OrderMarkerStrategy : IOrderMarkerStrategy
    {
        private readonly IPositionCacheService positionService;
        private readonly object syncObejct = new object();
        protected OrderMarkerStrategy(IPositionCacheService positionService)
        {
            this.positionService = positionService;
        }

        public IEnumerable<IOrder> MarkOrders(IEnumerable<IOrder> orders)
        {
            var result = new List<IOrder>();
            if (orders == null)
                return result;

            // this protect state in position service. Only one order is applied at time.
            lock (this.syncObejct)
            {
                foreach (var order in orders)
                {
                    var markedOrders = this.MarkOrder(order);
                    if (markedOrders != null)
                    {
                        result.AddRange(markedOrders);
                    }

                    this.SubmitOrder(order);
                }
            }

            return result;
        }

        public abstract IEnumerable<IOrder> MarkOrder(IOrder order);

        protected IPosition GetExisitingPosition(IOrder order)
        {
            var key = PositionKey.Create(order.Portfolio, order.SecurityMasterId);
            var existingPosition = this.positionService.GetPositionByKey(key);
            return existingPosition;
        }

        protected Order CreateSplitOrderFromSource(IOrder source)
        {
            Order destination = new Order();
            destination.SecurityMasterId = source.SecurityMasterId;
            destination.OrderType = source.OrderType;
            destination.Portfolio = source.Portfolio;            
            destination.Price = source.Price;
            return destination;
        }

        protected void SetOrderMark(Order order, OrderMarkerType type)
        {
            order.OrderMarkerType = type;
        }

        protected void SubmitOrder(IOrder order)
        {
            this.positionService.SubmitOrder(order);
        }
    }
}
