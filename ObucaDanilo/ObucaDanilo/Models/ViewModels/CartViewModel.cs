namespace ObucaDanilo.Models.ViewModels
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => ProductPrice * Quantity;
    }
}
