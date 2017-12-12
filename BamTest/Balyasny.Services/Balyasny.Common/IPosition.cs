namespace Balyasny.Common
{
    public interface IPosition
    {
        int SecurityMasterId { get; }
        string Portfolio { get; }
        decimal Quantity { get; }
        decimal AvgPrice { get; }
        bool IsShort { get; }
    }
}
