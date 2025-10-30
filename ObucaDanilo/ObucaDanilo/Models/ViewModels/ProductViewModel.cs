namespace ObucaDanilo.Models.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public List<string> CategoryNames { get; set; } = new List<string>();
    }
}