using System.Net;

namespace TheGentlemanLibrary.Application.Models.BaseModels
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; } = false;
        public T? Data { get; set; }
        public IEnumerable<string>? ErrorMessages { get; set; }
        public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;

        public ApiResponse() { }

        private ApiResponse(T? result, bool isSuccess, IEnumerable<string>? errorMessages = default, int statusCode = 200)
        {
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages;
            Data = result;
            StatusCode = statusCode;
        }

        public static ApiResponse<T> Success(T result, int statusCode = 200)
            => new(result, true, statusCode: statusCode);

        public static ApiResponse<T> Fail(T? result, IEnumerable<string>? errorMessages = default, int statusCode = 400)
            => new(result, false, errorMessages, statusCode);

        public static ApiResponse<T> Fail(IEnumerable<string>? errorMessages = default, int statusCode = 400)
            => new(default, false, errorMessages, statusCode);
    }
}