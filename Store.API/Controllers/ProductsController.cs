using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.Product;
using Store.Service.HandlingResponses;
using Store.Service.Helper;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.DTOs;

namespace Store.API.Controllers
{
    [Route("api/[controller]")] //could add /[action] or make it on http tag
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet("brands")]
        [Cache(30)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands()
        {
            return Ok(await productService.GetAllBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes()
        {
            return Ok(await productService.GetAllTypesAsync());
        }
        [HttpGet("products")]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery] ProductSpecification input)
        {
            return Ok(await productService.GetAllProductsAsync(input));
        }
        [HttpGet("ProductById")]
        public async Task<ActionResult<ProductDetailsDto>> GetProduct(int? id)
        {
            if (id == null)
                return BadRequest(new CustomException(400, "Id is Null"));

            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new CustomException(404));
            return Ok(product);
        }
    }
}
