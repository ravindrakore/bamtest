namespace Balyasny.Common.Implementation
{
    public class Security : ISecurity
    {
        public int SecurityMasterId { get; set; }
        public string Ticker { get; set; }
        public string SecurityId { get; set; }
        public SecurityType SecurityType { get; set; }
    }
}
