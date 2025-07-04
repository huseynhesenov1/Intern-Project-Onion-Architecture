﻿namespace Project.Application.DTOs.ProductDistrictPriceDTOs
{
    public record CreateProductDistrictPriceOutput
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DistrictId { get; set; }
        public decimal Price { get; set; }

    }
}
