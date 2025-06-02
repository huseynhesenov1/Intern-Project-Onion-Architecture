namespace Project.Application.DTOs.ProductDTOs
{
    public record ProductUpdateDTO
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
