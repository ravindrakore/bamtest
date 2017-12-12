using Balyasny.Common;
using System.Collections.Generic;

namespace Balyasny.Services
{
    public interface IOrderMarkerStrategy
    {
        IEnumerable<IOrder> MarkOrders(IEnumerable<IOrder> orders);
    }
}
