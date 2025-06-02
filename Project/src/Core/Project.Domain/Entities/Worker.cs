using Project.Domain.Entities.Commons;

namespace Project.Domain.Entities
{
    public class Worker : BaseAuditableEntity
    {
        public string FinCode { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public District District { get; set; }
        public int DistrictId { get; set; }
    }
}
