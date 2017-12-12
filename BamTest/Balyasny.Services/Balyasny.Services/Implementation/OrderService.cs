using Balyasny.Common;
using System.Collections.Generic;
using System.Linq;
namespace Balyasny.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IPositionCacheService positionService;
        private readonly ISecurityMasterService securityService;
        private readonly IOrderRoutingService orderRoutingService;
        private readonly OrderMarker orderMaker;        
        public OrderService(IPositionCacheService  positionService, ISecurityMasterService securityService, IOrderRoutingService orderRoutingService)
        {
            this.positionService = positionService;
            this.securityService = securityService;
            this.orderRoutingService = orderRoutingService;
            this.orderMaker = new OrderMarker(positionService, securityService);
        }

        public void ProcessOrders(IEnumerable<IOrder> orders)
        {
            var markedOrders = this.orderMaker.MarkOrders(orders);
            if(markedOrders != null && markedOrders.Any())
            {
                this.orderRoutingService.Route(markedOrders);
            }
        }
    }
}
