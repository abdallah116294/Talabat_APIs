using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{
    
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper=mapper;
        }
        //GET 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{BasketId}")]
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
                //return response  is null?new CustomerBasket(Id):response;

            }
            else
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Basket ID cannot be null."));
            }
        }

        //Update Or Create 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        //Get an error while mapping from CustomerBasket to CustomerBasketDTO I will solve it later 
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasket basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.Id))
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Invalid basket data."));
            }
           // var MappedCustomerBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            if (CreatedOrUpdatedBasket is null)
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Failed to update or create the basket."));
            }
            var response = new APIResponse<CustomerBasket>
            {
                Data = CreatedOrUpdatedBasket,
                Status = "Success"
            };
            return Ok(response);
        }
        // DeLeTe
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
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
