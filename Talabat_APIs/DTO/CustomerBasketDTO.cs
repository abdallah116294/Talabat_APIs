using System.ComponentModel.DataAnnotations;

namespace Talabat_APIs.DTO
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItmeDTO> Itmes { get; set; }
    }
}
