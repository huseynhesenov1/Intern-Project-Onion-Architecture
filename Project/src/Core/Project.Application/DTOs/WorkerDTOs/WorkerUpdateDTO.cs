namespace Project.Application.DTOs.WorkerDTOs
{
    public record WorkerUpdateDTO
    {
        public string FinCode { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int DistrictId { get; set; }
    }
}
