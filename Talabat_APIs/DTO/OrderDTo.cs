using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat_APIs.DTO
{
    public class  OrderDTo
    {
        [Required]
        public string BasketID { get; set; }
        public int DeliveryMethodId { get; set; }  
        public AddressDTO ShippingAddress  { get; set; }

    }
}
