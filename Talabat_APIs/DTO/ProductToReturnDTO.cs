using Talabat.Core.Entities;

namespace Talabat_APIs.DTO
{
    public class ProductToReturnDTO
    {
        // public int Id { get; set; }// Uncomment if you want to use BaseEntity
        public int Id { get; set; } // Rational: 1 Product can have 1 Id, but 1 Id can have many Products
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        //Rational: 1 Product can have 1 Brand, but 1 Brand can have many Products
        public int ProductBrandId { get; set; }
        public string ProductBrand { get; set; }
        //Rational: 1 Product can have 1 Type, but 1 Type can have many Products
        public int  ProductTypeId { get; set; }
        public string ProductType { get; set; }

    }
}
