namespace Balyasny.Common
{
    public interface ISecurity
    {
        int SecurityMasterId { get; }
        string Ticker { get; }
        string SecurityId { get; }
        SecurityType SecurityType { get; }
    }
}
