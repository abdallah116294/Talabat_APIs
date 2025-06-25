
using AutoMapper;
using Talabat.Core.Entities;
using Talabat_APIs.DTO;

namespace Talabat_APIs.Helpers
{
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        //Get appsettings value from the configuration
        private readonly IConfiguration _configuration;
        public ProductPictureURLResolver(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}{source.PictureUrl}";
            }
            else
            {
                return string.Empty; // Return an empty string if PictureUrl is null or empty
            }
        }
    }
}
