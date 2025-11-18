namespace OrderApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string? ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
