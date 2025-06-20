namespace Project.Application.DTOs.Campaign
{
    public record CreateCampaignInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DistrictId { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
