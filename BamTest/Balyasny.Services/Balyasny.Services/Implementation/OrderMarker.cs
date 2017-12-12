using Balyasny.Common;
using System.Collections.Generic;
using System.Linq;

namespace Balyasny.Services.Implementation
{
    /// <summary>
    /// OrderMaker is marking order to long/short, buy/close according to existing positions. Here I am using OrderMarker type enum describe marker, but very can be chnaged create order of
    /// particular type using well defined classes like BuyOrder, BuyToOPenOrder(depend upon defined contract with order routing).
    /// </summary>
    public class OrderMarker
    {
        private readonly IPositionCacheService positionService;
        private readonly IOrderMarkerStrategy equityOrderMakerStartegy;
        private readonly IOrderMarkerStrategy optionOrderMakerStartegy;
        private readonly ISecurityMasterService securityMasterService;
        public OrderMarker(IPositionCacheService positionService, ISecurityMasterService securityMasterService)
        {
            this.positionService = positionService;
            this.securityMasterService = securityMasterService;
            this.equityOrderMakerStartegy = new EquityOrdeMakerStartegy(positionService);
            this.optionOrderMakerStartegy = new OptionOrdeMakerStartegy(positionService);
        }

        public IEnumerable<IOrder> MarkOrders(IEnumerable<IOrder> orders)
        {
            var result = new List<IOrder>();
            if (orders == null)
                return result;
            foreach (var ordersByStrategies in orders.GroupBy(order => this.GetStrategyByOrder(order)))
            {
                var strategy = ordersByStrategies.Key;
                if (strategy != null)
                {
                    var markedOrders = strategy.MarkOrders(orders.ToList());
                    if(markedOrders != null)
                    {
                        result.AddRange(markedOrders);
                    }
                }
            }

            return result;
        }

        private IOrderMarkerStrategy GetStrategyByOrder(IOrder order)
        {
            var security = this.GetSecurity(order);
            if (security != null)
            {
                switch (security.SecurityType)
                {
                    case SecurityType.Equity:
                        return this.equityOrderMakerStartegy;
                    case SecurityType.Option:
                        return this.optionOrderMakerStartegy;
                    case SecurityType.NotSupported:
                    default:
                        break;
                }
            }

            return null;
        }
        
        private ISecurity GetSecurity(IOrder order)
        {
            if (order == null) return null;
            return this.securityMasterService.GetSecurityById(order.SecurityMasterId);
        }
    }
}
