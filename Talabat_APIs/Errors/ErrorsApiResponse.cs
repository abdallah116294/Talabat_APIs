
namespace Talabat_APIs.Errors
{
    public class ErrorsApiResponse
    {
        public int StatusCode { get; set; } 
        public string? Message { get; set; }
        public ErrorsApiResponse(int statusCode,string? message =null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            //swtch case to return default messages for common status codes
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change.",
                _ => null
            };
        }
    }
}
