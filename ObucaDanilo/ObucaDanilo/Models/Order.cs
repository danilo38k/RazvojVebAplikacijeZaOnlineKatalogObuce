using System.ComponentModel.DataAnnotations;

namespace ObucaDanilo.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? ShippingAddress { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
