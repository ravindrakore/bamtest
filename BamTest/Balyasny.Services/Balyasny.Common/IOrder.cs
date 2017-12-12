namespace Balyasny.Common
{
    public interface IOrder
    {
        string Portfolio { get; }
        int SecurityMasterId { get; set; }
        decimal Quantity { get;}
        decimal Price { get; }        
        OrderMarkerType OrderMarkerType { get; }
        OrderType OrderType { get; }
    }
}
