using Balyasny.Common;
using System.Collections.Generic;

namespace Balyasny.Services.Implementation
{
    public class EquityOrdeMakerStartegy : OrderMarkerStrategy
    {
        public EquityOrdeMakerStartegy(IPositionCacheService positionService) : base(positionService)
        {

        }

        public override IEnumerable<IOrder> MarkOrder(IOrder order)
        {
            var result = new List<IOrder>();
            var existingPosition = this.GetExisitingPosition(order);
            if (order.OrderType == OrderType.Buy)
            {
                this.ProcessBuy(order, result);
            }
            else
            {
                this.ProcessSell(order, result, existingPosition);
            }

            return result;
        }

        private void ProcessSell(IOrder order, List<IOrder> result, IPosition existingPosition)
        {
            if (existingPosition != null && !existingPosition.IsShort)
            {
                var markedOrder = this.CreateSplitOrderFromSource(order);
                markedOrder.OrderMarkerType = OrderMarkerType.SellLong;
                markedOrder.Quantity = existingPosition.Quantity > order.Quantity ? order.Quantity : existingPosition.Quantity;
                result.Add(markedOrder);
                var remaining = order.Quantity - existingPosition.Quantity;
                if (remaining > 0)
                {
                    var markedSellShortOrder = this.CreateSplitOrderFromSource(order);
                    markedSellShortOrder.OrderMarkerType = OrderMarkerType.SellShort;
                    markedSellShortOrder.Quantity = remaining;
                    result.Add(markedSellShortOrder);
                }                
            }
            else
            {
                var markedSellShortOrder = this.CreateSplitOrderFromSource(order);
                markedSellShortOrder.OrderMarkerType = OrderMarkerType.SellShort;
                markedSellShortOrder.Quantity = order.Quantity;
                result.Add(markedSellShortOrder);
            }
        }        

        private void ProcessBuy(IOrder order, List<IOrder> result)
        {
            var markedOrder = this.CreateSplitOrderFromSource(order);
            markedOrder.OrderMarkerType = OrderMarkerType.Buy;
            markedOrder.Quantity = order.Quantity;
            result.Add(markedOrder);
        }
    }

}
