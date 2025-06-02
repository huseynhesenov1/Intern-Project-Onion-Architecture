namespace Project.Application.DTOs.OrderDTOs
{
    public record OrderCreateResponseDTO
    {
        public decimal TotalPrice { get; set; }
    }
}
