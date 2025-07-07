using System.Net;
using System.Text.Json;
using Talabat_APIs.Errors;

namespace Talabat_APIs.MiddelWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleWare(RequestDelegate Next, ILogger<ExceptionMiddleWare>logger,IHostEnvironment environment)
        {
            _next = Next;
            this._logger = logger;
            this._environment = environment;
        }
        //InvokeAsync method is called for each request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message); // Log the exception
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Set the response status code to 500
                context.Response.ContentType = "application/json"; // Set the response content type to JSON
                var response = _environment.IsDevelopment() ? 
                    new ApiExceptionResponse(context.Response.StatusCode,ex.Message, ex.StackTrace.ToString()) : 
                    new ApiExceptionResponse(context.Response.StatusCode); // Create an error response based on the environment
                
                var JsonOption = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Use camel case for property names
                };
                var JsonResponse = JsonSerializer.Serialize(response, JsonOption); // Serialize the response to JSON
                await context.Response.WriteAsync(JsonResponse); // Write the error response as JSON
            }
        }
    }
}
