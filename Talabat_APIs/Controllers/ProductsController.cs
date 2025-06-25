using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;
using Talabat.Repository;
using Talabat_APIs.DTO;

namespace Talabat_APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepositort<Product> _productRepository;
        private readonly IMapper _mapper;

        // private readonly ISpecification<Product> _productSpecification;
        public ProductsController(IGenericRepositort<Product> productRepo,IMapper mapper)
        {
            _productRepository = productRepo;
            _mapper = mapper;
        }
        //Get all products
        [HttpGet(Name = "api/GetProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var Spec = new ProductWithBrandAndTypeSpecification(); // This will get all products
            var products = await _productRepository.GetAllWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(products);
            //  OkObjectResult result = new OkObjectResult(MappedProducts);
            //var finalResult=  new APIResponse<IEnumerable<Product>>
           // {
           //     Data =products,
           //     Status = "Success",
           // };
           //var  finalResult = new APIResponse<IEnumerable<ProductToReturnDTO>>
           // {
           //     Data = MappedProducts,
           //     Status = "Success",
           // };
           var finalProducts = new APIResponse<IEnumerable<ProductToReturnDTO>>
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
        public async Task<ActionResult<ProductToReturnDTO>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _productRepository.GetByIdWithSpecAsync(Spec);
            var MappedProduct = _mapper.Map<Product, ProductToReturnDTO>(product);
            if (MappedProduct == null)
            {
                return NotFound(new APIResponse<ProductToReturnDTO>
                {
                    Data = null,
                    Status = "Not Found",
                });
            }
            var result= new APIResponse<ProductToReturnDTO>
            {
                Data = MappedProduct,
                Status = "Success", 
            };
            return Ok(result);
        }
    }
}
