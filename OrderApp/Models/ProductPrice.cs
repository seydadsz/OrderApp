namespace OrderApp.Models
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime ValidFrom { get; set; }
    }
}
