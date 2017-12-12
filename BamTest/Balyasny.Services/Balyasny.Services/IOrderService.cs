using Balyasny.Common;
using System;
using System.Collections.Generic;

namespace Balyasny.Services
{
    public interface IOrderService
    {
        void ProcessOrders(IEnumerable<IOrder> orders);
    }
}
