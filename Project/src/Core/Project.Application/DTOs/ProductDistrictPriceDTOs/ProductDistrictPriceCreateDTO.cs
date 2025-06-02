namespace Project.Application.DTOs.ProductDistrictPriceDTOs
{
    public record ProductDistrictPriceCreateDTO
    {
        public int ProductId { get; set; }
        public int DistrictId { get; set; }
        public decimal Price { get; set; }

    }
}
