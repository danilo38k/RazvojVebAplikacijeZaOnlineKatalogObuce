namespace ObucaDanilo.Models
{
    public class Cart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }
        public User User { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
