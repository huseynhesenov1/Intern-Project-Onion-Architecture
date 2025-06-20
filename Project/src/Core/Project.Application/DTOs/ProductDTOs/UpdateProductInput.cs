namespace Project.Application.DTOs.ProductDTOs
{
    public record UpdateProductInput
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
