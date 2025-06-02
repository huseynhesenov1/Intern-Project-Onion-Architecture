namespace Project.Application.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }


        public string ErrorDetail { get; set; }

        public static ApiResponse<T> Success(T data, string message = "")
        {
            return new ApiResponse<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T> { IsSuccess = false, Message = message };
        }

        public static ApiResponse<T> Fail(string message, string detail)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                ErrorDetail = detail
            };
        }
    }
}
