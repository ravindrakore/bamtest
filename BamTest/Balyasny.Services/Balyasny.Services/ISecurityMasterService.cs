using Balyasny.Common;

namespace Balyasny.Services
{
    public interface ISecurityMasterService
    {
        ISecurity GetSecurityById(int securityMasterId);
    }
}
