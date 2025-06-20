namespace Project.Application.DTOs.WorkerDTOs
{
    public record CreateWorkerOutput
    {
        public int WorkerId { get; set; }
        public string FinCode { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int DistrictId { get; set; }

    }
}
