using System.ComponentModel.DataAnnotations;

namespace Talabat_APIs.DTO
{
    public class BasketItmeDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Range(0.1,double.MaxValue,ErrorMessage ="Price can not be Zero")]
        [Required]
        public decimal Price { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Quantity Must be one Itme at Least")]
        [Required]
        public int Quantity { get; set; }
    }
}