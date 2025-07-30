using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService   
    {
        //Create Order by
        Task<Order> CreateOrderAsync(string buyerEmail, string basketID, int deliveryMethodId, Address ShippingAddress);
        //Get Orders by BuyerEmail
        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);
        //Get Order by Id
        Task<Order> GetOrderByIdForSpecificUserAsync(int id, string buyerEmail);

    }
}
