namespace Balyasny.Common.Implementation
{
    public class Order : IOrder
    {
      public string Portfolio { get; set; }
      public decimal Quantity { get; set; }
      public decimal Price { get; set; }      
      public int SecurityMasterId { get; set; }
      public OrderMarkerType OrderMarkerType { get; set; }
      public OrderType OrderType { get; set; }
    }
}
