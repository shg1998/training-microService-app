namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }
        public List<BasketShoppingModel> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
