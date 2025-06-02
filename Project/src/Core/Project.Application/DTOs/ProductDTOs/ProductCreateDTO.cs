namespace Project.Application.DTOs.ProductDTOs
{
    public record ProductCreateDTO
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
