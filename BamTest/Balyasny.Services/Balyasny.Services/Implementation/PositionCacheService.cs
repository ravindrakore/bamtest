using System.Collections.Generic;
using System.Linq;
using Balyasny.Common;
using Balyasny.Common.Implementation;
using System;

namespace Balyasny.Services.Implementation
{
    public class PositionCacheService : IPositionCacheService
    {
        private IDictionary<PositionKey, IPosition> positions;
        public PositionCacheService()
        {
            positions = new Dictionary<PositionKey, IPosition>();
        }

        public IEnumerable<IPosition> GetCurrentPositions()
        {
            return positions.Values.ToList().AsReadOnly();
        }

        public IPosition GetPositionByKey(PositionKey key)
        {
            IPosition result;
            if (this.positions.TryGetValue(key, out result))
            {
                return result;
            }

            return null;
        }

        public void SubmitOrder(IOrder order)
        {
            if (order == null)
                return;

            var key = PositionKey.Create(order.Portfolio, order.SecurityMasterId);
            switch(order.OrderType)
            {
                case OrderType.Buy:
                    this.ProcessBuyOrder(order, key);
                    break;
                case OrderType.Sell:
                    this.ProcessSellOrder(order, key);
                    break;
            }
        }

        private void ProcessSellOrder(IOrder order, PositionKey key)
        {
            IPosition position;
            if (this.positions.TryGetValue(key, out position))
            {
                var pos = this.CreatePositionFromSource(position);
                pos.AvgPrice = position.AvgPrice;
                if (!position.IsShort)
                {
                    pos.IsShort = position.Quantity < order.Quantity;
                    pos.Quantity = Math.Abs(position.Quantity - order.Quantity);
                }
                else
                {
                    pos.AvgPrice = ((position.AvgPrice * position.Quantity) + (order.Quantity * order.Price)) / (order.Quantity + position.Quantity);
                    pos.IsShort = true;
                    pos.Quantity += order.Quantity;
                }

                this.positions[key] = pos;
            }
            else
            {
                position = this.ToPosition(order);
                this.positions.Add(key, position);
            }
        }

        private void ProcessBuyOrder(IOrder order, PositionKey key)
        {
            IPosition position;
            if(this.positions.TryGetValue(key, out position))
            {
                var pos = this.CreatePositionFromSource(position);
                pos.AvgPrice = ((position.AvgPrice * position.Quantity) + (order.Quantity * order.Price)) / (order.Quantity + position.Quantity);
                if(position.IsShort)
                {
                    pos.IsShort = position.Quantity > order.Quantity;
                    pos.Quantity = Math.Abs(position.Quantity - order.Quantity);                    
                }
                else
                {
                    pos.IsShort = false;
                    pos.Quantity += order.Quantity;
                }

                this.positions[key] = pos;
            }
            else
            {
                position = this.ToPosition(order);
                this.positions.Add(key, position);
            }
        }

        private Position ToPosition(IOrder order)
        {
            var pos = new Position()
            {
                AvgPrice = order.Price,
                IsShort = order.OrderType == OrderType.Sell,
                Portfolio = order.Portfolio,
                Quantity = order.Quantity,
                SecurityMasterId = order.SecurityMasterId
            };

            return pos;
        }

        private Position CreatePositionFromSource(IPosition source)
        {
            var newPosition = new Position()
            {
                AvgPrice = source.AvgPrice,
                Quantity = source.Quantity,
                IsShort = source.IsShort,
                Portfolio = source.Portfolio,
                SecurityMasterId = source.SecurityMasterId
            };

            return newPosition;
        }

    }
}
