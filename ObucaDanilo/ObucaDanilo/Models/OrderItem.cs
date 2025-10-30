namespace ObucaDanilo.Models
{
    public class OrderItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
