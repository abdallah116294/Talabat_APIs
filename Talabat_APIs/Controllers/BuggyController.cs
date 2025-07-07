using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        [HttpGet("NotFound")]
        public ActionResult GetNotFound()
        {
            var product = _dbcontext.Products.Find(42); 
            if(product == null)
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound));
            }
            return Ok(product);
        }
        [HttpGet("ServerError")]        
        public ActionResult GetServerError()
        {
            var product = _dbcontext.Products.Find(42);
            var productToReturn = product.ToString(); // This will throw a NullReferenceException if product is null
            return Ok(productToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return BadRequest($"This is a bad request with id: {id}");
        }

    }
}
