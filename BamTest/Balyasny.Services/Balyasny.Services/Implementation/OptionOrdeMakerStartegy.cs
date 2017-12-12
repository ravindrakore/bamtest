using Balyasny.Common;
using System.Collections.Generic;

namespace Balyasny.Services.Implementation
{
    public class OptionOrdeMakerStartegy : OrderMarkerStrategy
    {
        public OptionOrdeMakerStartegy(IPositionCacheService positionService) : base(positionService)
        {

        }

        public override IEnumerable<IOrder> MarkOrder(IOrder order)
        {
            var result = new List<IOrder>();
            var existingPosition = this.GetExisitingPosition(order);
            if (order.OrderType == OrderType.Buy)
            {
                this.ProcessBuy(order, result, existingPosition);
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
                markedOrder.OrderMarkerType = OrderMarkerType.SellToClose;
                markedOrder.Quantity = (existingPosition.Quantity > order.Quantity) ? order.Quantity : existingPosition.Quantity;
                result.Add(markedOrder);
                var remaining = order.Quantity - existingPosition.Quantity;
                if (remaining != 0)
                {
                    var markedSellShortOrder = this.CreateSplitOrderFromSource(order);
                    markedSellShortOrder.OrderMarkerType = OrderMarkerType.SellToOpen;
                    markedSellShortOrder.Quantity = remaining;
                    result.Add(markedSellShortOrder);
                }                
            }
            else
            {
                var markedSellShortOrder = this.CreateSplitOrderFromSource(order);
                markedSellShortOrder.OrderMarkerType = OrderMarkerType.SellToOpen;
                markedSellShortOrder.Quantity = order.Quantity;
                result.Add(markedSellShortOrder);
            }
        }

        private void ProcessBuy(IOrder order, List<IOrder> result, IPosition existingPosition)
        {
            if (existingPosition != null && existingPosition.IsShort)
            {
                var markedOrder = this.CreateSplitOrderFromSource(order);
                markedOrder.OrderMarkerType = OrderMarkerType.BuyToClose;
                markedOrder.Quantity = order.Quantity < existingPosition.Quantity ? existingPosition.Quantity : order.Quantity;
                result.Add(markedOrder);

                var remaining = order.Quantity - existingPosition.Quantity;
                if (remaining >= 0)
                {
                    var markedBuyToOpenOrder = this.CreateSplitOrderFromSource(order);
                    markedBuyToOpenOrder.OrderMarkerType = OrderMarkerType.BuyToOpen;
                    markedBuyToOpenOrder.Quantity = remaining;
                    result.Add(markedBuyToOpenOrder);
                }
            }
            else
            {
                var markedBuyToOpenOrder = this.CreateSplitOrderFromSource(order);
                markedBuyToOpenOrder.OrderMarkerType = OrderMarkerType.BuyToOpen;
                markedBuyToOpenOrder.Quantity = order.Quantity;
                result.Add(markedBuyToOpenOrder);
            }
        }
    }
}
