using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat_APIs.DTO;

namespace Talabat_APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()    
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureURLResolver>()); // Custom resolver for PictureUrl
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, Talabat.Core.Entities.Order_Aggregate.Address>();
            CreateMap<CustomerBasketDTO, CustomerBasket>()
                .ConstructUsing(src=>new CustomerBasket(src.Id))
                .ForMember(dest=>dest.Items,opt=>opt.MapFrom(src=>src.Itmes));
            CreateMap<BasketItmeDTO, BasketItem>();
        }
    }
}
