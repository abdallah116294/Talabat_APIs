namespace Talabat_APIs.Errors
{
    public class ApiExceptionResponse : ErrorsApiResponse
    {
        public string?  Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? message = null,string? detaisl=null) : base(statusCode, message)
        {
            Details = detaisl ;
        }
    }
}
