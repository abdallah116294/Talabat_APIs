using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;
using Talabat.Repository;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepositort<Product> _productRepository;
        private readonly IGenericRepositort<ProductType> _productTypeRepository;
        private readonly IGenericRepositort<ProductBrand> _productBrandRepo;
        private readonly IMapper _mapper;

        // private readonly ISpecification<Product> _productSpecification;
        public ProductsController(IGenericRepositort<Product> productRepo,IMapper mapper,IGenericRepositort<ProductType> productTypeRepo,IGenericRepositort<ProductBrand>productBrandRepo )
        {
            _productRepository = productRepo;
            _mapper = mapper;
            _productTypeRepository = productTypeRepo;
            _productBrandRepo = productBrandRepo;
        }
        //Get all products
        [HttpGet(Name = "api/GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<IReadOnlyList<Product>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorsApiResponse))]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(Params); // This will get all products
            var products = await _productRepository.GetAllWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
           if(MappedProducts == null || !MappedProducts.Any())
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound));
            }
            //return Ok(MappedProducts);
            //return result;
            var finalProducts = new APIResponse<IReadOnlyList<ProductToReturnDTO>>
            {
                Data = MappedProducts,
                Status = "Success",
            };
            return Ok(finalProducts);
            // return Ok(MappedProducts);
            //  return result;
        }
        //Get Product by Id
        [HttpGet("{id}", Name = "api/GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductToReturnDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorsApiResponse))]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _productRepository.GetByIdWithSpecAsync(Spec);
            var MappedProduct = _mapper.Map<Product, ProductToReturnDTO>(product);
            if (MappedProduct == null)
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound));

            }
            var result= new APIResponse<ProductToReturnDTO>
            {
                Data = MappedProduct,
                Status = "Success", 
            };
            return Ok(result);
        }
        //Get All Product Types
        [HttpGet("GetProtductTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductType>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorsApiResponse))]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductType()
        {
            var productTypes = await _productTypeRepository.GetAllAsync();
            if (productTypes == null)
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound));
            }
            return Ok(new APIResponse<IEnumerable<ProductType>>
            {
                Data = productTypes,
                Status = "Successs"
            });
        }
        //Get All Product Brands
        [HttpGet("GetProductBrands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductBrand>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorsApiResponse))]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrand()
        {
            var productBrands = await _productBrandRepo.GetAllAsync();
            if (productBrands == null)
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound));
            }
            return Ok(new APIResponse<IReadOnlyList<ProductBrand>>
            {
                Data = productBrands,
                Status = "Successs"
            });
        }
    }
}
