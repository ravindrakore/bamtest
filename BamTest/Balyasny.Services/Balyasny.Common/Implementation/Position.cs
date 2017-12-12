namespace Balyasny.Common.Implementation
{
    public class Position : IPosition
    {
        public int SecurityMasterId { get; set; }
        public string Portfolio { get; set; }
        public decimal Quantity { get; set; }      
        public decimal AvgPrice { get; set; }
        public bool IsShort { get; set; }
    }
}
