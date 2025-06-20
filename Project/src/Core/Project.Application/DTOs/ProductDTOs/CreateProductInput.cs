namespace Project.Application.DTOs.ProductDTOs
{
    public record CreateProductInput
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
