using Balyasny.Common;
using Balyasny.Services.Implementation;
using System.Collections.Generic;

namespace Balyasny.Services
{
    public interface IPositionCacheService
    {
        IPosition GetPositionByKey(PositionKey key);
        IEnumerable<IPosition> GetCurrentPositions();
        void SubmitOrder(IOrder order);
    }
}
