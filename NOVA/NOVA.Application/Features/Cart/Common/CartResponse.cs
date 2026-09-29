namespace NOVA.Application.Features.Cart.Common
{
    public class CartResponse
    {
        public int Id { get; set; }
        public List<CartItemResponse> Items { get; set; } = [];

        public decimal TotalCartPrice => Items.Sum(item => item.TotalPrice); // computed property
        public int TotalItemsCount => Items.Sum(item => item.Quantity);
    }
}