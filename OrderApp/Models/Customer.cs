namespace OrderApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        public string? Email { get; set; }  
    }
}
