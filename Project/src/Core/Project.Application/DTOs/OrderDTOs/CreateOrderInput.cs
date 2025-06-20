namespace Project.Application.DTOs.OrderDTOs
{
    public record CreateOrderInput
    {
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
    }
}
