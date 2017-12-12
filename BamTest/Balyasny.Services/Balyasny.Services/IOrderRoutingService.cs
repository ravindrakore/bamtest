using Balyasny.Common;
using System.Collections.Generic;

namespace Balyasny.Services
{
    public interface IOrderRoutingService
    {
        void Route(IEnumerable<IOrder> orders);
    }
}
