namespace ObucaDanilo.Models.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }

    public class OrderItemViewModel
    {
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
