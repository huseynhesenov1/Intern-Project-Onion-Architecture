namespace Project.Application.Exceptions
{
    public class CampaignConflictException : Exception
    {
        public CampaignConflictException(string campaignName)
            : base($"'{campaignName}' adlı kampaniya ilə tarixlər üst-üstə düşür.") { }
    }
}
