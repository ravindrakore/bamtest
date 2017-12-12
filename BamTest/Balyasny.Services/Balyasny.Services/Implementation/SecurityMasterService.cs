using System;
using System.Collections.Generic;
using Balyasny.Common;
using Balyasny.Common.Implementation;

namespace Balyasny.Services.Implementation
{
    // This is sample implemention of security master service/ or interface security master product. It is not part of the solution but requited for the solution.
    public class SecurityMasterService : ISecurityMasterService
    {
        // maintaing few local securities in dictionary for test data.
        private readonly IDictionary<int, ISecurity> securities = new Dictionary<int, ISecurity>();
        public SecurityMasterService()
        {
            this.Init();
        }

        public ISecurity GetSecurityById(int securityMasterId)
        {
            if (securities.ContainsKey(securityMasterId))
                return this.securities[securityMasterId];
            return null;
        }

        private void Init()
        {
            this.securities.Add(1, new Security() { SecurityId = "Google US Equity", SecurityMasterId = 1, SecurityType = SecurityType.Equity, Ticker = "GOOGL" });
            this.securities.Add(2, new Security() { SecurityId = "IBM US Equity", SecurityMasterId = 2, SecurityType = SecurityType.Equity, Ticker = "IBM" });
            this.securities.Add(3, new Security() { SecurityId = "MSFT US Equity", SecurityMasterId = 3, SecurityType = SecurityType.Equity, Ticker = "MSFT" });
            this.securities.Add(4, new Security() { SecurityId = "AAPL US Equity Option", SecurityMasterId = 4, SecurityType = SecurityType.Option, Ticker = "AAPL" });
            this.securities.Add(5, new Security() { SecurityId = "TSLA US Equity Option", SecurityMasterId = 5, SecurityType = SecurityType.Option, Ticker = "TSLA" });
        }
    }
}
