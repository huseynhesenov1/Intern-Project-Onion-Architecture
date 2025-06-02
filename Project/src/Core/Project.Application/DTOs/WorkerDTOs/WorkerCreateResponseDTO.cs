namespace Project.Application.DTOs.WorkerDTOs
{
    public record WorkerCreateResponseDTO
    {
        public int WorkerId { get; set; }
        public string WorkerToken { get; set; }
    }
}
