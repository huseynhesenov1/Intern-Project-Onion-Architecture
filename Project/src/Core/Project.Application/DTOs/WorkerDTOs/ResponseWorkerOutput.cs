namespace Project.Application.DTOs.WorkerDTOs
{
    public record ResponseWorkerOutput
    {
        public int WorkerId { get; set; }
        public string WorkerToken { get; set; }
    }
}
