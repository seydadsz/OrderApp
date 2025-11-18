namespace OrderApp.Models
{
    public class ProductPriceWithProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public decimal Price { get; set; }
        public DateTime ValidFrom { get; set; }

        public string StockName { get; set; } = string.Empty;
    }
}
