using Talabat.Core.Entities;

namespace Talabat_APIs.Errors
{
    public class ApiVialidationErrorResponse : ErrorsApiResponse
    {
        public ApiVialidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
