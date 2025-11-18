namespace OrderApp.Models
{
    public class OrderWithDetails
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
    }
}
