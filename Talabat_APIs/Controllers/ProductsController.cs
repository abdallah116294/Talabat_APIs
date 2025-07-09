using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;
using Talabat.Repository;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;
using Talabat_APIs.Helpers;

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
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(Params); // This will get all products
            var products = await _productRepository.GetAllWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            if (MappedProducts == null || !MappedProducts.Any())
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound));
            }
            var countSpec = new ProductWithFiltrationForCountAsync(Params) ;
            var returnedProducts = new Pagination<ProductToReturnDTO>()
           {
                Data = MappedProducts,
                PageIndex = Params.PageIndex,
                PageSize = Params.PageSize,
                Count = await _productRepository.CountAsync(countSpec),
            };
            var result = new APIResponse<Pagination<ProductToReturnDTO>>()
            {
                Data = returnedProducts,
                Status = "Success",
            };
            return Ok(result);;
        }
        // Search Products by Name
        [HttpGet("SearchProductByName")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> SearchProductsByName([FromQuery]string Search)
        {
            //Check if the search string is null or empty   
            if (string.IsNullOrEmpty(Search))
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Search term cannot be null or empty."));
            }
            //Create a specification for searching products by name
            //This specification will filter products based on the search term provided in the query string
            //The search term is converted to lowercase to ensure case-insensitive matching

            var Spec = new ProductWithSearchSpecification(Search);
            //Get all products based on the specification
            var products = await _productRepository.GetAllWithSpecAsync(Spec);
            //Map the products to DTOs
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            //Check if the mapped products are null or empty
            if (MappedProducts == null || !MappedProducts.Any())
            {
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "No products found with the specified criteria."));
            }
            //Return the mapped products in a successful API response
            return Ok(new APIResponse<IReadOnlyList<ProductToReturnDTO>>
            {
                Data = MappedProducts,
                Status = "Success"
            });
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
