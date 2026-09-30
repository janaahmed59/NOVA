namespace NOVA.Application.Features.Orders.Common
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AddressId { get; set; }
        public string? ShippingAddress { get; set; }
        public List<OrderItemResponse> Items { get; set; } = [];
    }
}