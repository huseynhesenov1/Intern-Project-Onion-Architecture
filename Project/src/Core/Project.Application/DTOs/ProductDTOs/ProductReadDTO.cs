namespace Project.Application.DTOs.ProductDTOs
{
    public record ProductReadDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
