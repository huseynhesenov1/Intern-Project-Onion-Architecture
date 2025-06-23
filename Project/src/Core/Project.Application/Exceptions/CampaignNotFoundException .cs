namespace Project.Application.Exceptions
{
    public class CampaignNotFoundException : Exception
    {
        public CampaignNotFoundException(int id)
            : base($"Kampaniya tapılmadı. ID: {id}") { }
    }
}
