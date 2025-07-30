using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{
    
    public class OrderController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CreateOrder")]
        public async Task<ActionResult> CreateOrder([FromBody]OrderDTo orderDTo)
        {
            try
            {
                var BuyerEmail=User.FindFirstValue(ClaimTypes.Email);
                var MappedAddres = _mapper.Map<AddressDTO, Address>(orderDTo.ShippingAddress);
                var order = await _orderService.CreateOrderAsync(BuyerEmail, orderDTo.BasketID, orderDTo.DeliveryMethodId, MappedAddres);
                if (order == null)
                {
                    return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Order creation failed."));
                }
                    return  Ok(new APIResponse<Order>()
                {
                    Data = order,
                    Status = "Success",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest,$"Exception happend{ex} "));
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetOrdersForSpecificUser")]
        public async Task<ActionResult> GetOrdersForSpecificUser()
        {
            try
            {
                var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
                var orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
                if (orders == null || !orders.Any())
                {
                    return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "No orders found for this user."));
                }
                return Ok(new APIResponse<IReadOnlyList<Order>>()
                {
                    Data = orders,
                    Status = "Success",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, $"Exception happend{ex} "));
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetOrderByIdForSpecificUser/{id}")]
        public async Task<ActionResult> GetOrderByIdForSpecificUser(int id)
        {
            try
            {
                var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
                var order = await _orderService.GetOrderByIdForSpecificUserAsync(id, BuyerEmail);
                if (order == null)
                {
                    return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "Order not found."));
                }
                return Ok(new APIResponse<Order>()
                {
                    Data = order,
                    Status = "Success",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, $"Exception happend{ex} "));
            }
        }
    }
}
