namespace OrderApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
    }
}
