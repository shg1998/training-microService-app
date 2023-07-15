namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart() { }

        public ShoppingCart(string username) => this.Username = username;

        public string Username { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var item in this.Items)
                    totalPrice += item.Price * item.Count;
                return totalPrice;
            }
        }
    }
}
