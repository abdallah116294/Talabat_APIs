using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{
    
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        //GET 
        [HttpGet("{id}", Name = "GetBasket")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            if (BasketId != null)
            {
                var basket = await _basketRepository.GetBasketAsync(BasketId);
                if (basket == null)
                {
                    new CustomerBasket(BasketId);
                    return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "Basket not found."));
                }
               var response= new APIResponse<CustomerBasket>
                {
                    Data = basket,
                    Status = "Success"
                };
                 return Ok(response);
                //return response  is null?new CustomerBasket(BasketId):response;

            }
            else
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Basket ID cannot be null."));
            }
        }

        //Update
        [HttpPost("CreatBasket")]
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasket basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.Id))
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Invalid basket data."));
            }
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            return CreatedAtRoute("GetBasket", new { id = updatedBasket.Id }, updatedBasket);
        }
        // DeLeTe
        [HttpPost("{id}",Name ="DeleteBasket")]
        public async Task<ActionResult> DeleteCustomerBasket(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Basket ID cannot be null."));
            }
            var result = await _basketRepository.DeleteBasketAsync(id);
            if (!result)
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "Basket not found."));
            }
            return NoContent();
        }
    }
}
