namespace Project.Application.DTOs.OrderDTOs
{
    public record CreateOrderResponse
    {
        public decimal TotalPrice { get; set; }
    }
}
