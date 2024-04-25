using Store.Repository.Specification.Product;
using Store.Service.Helper;
using Store.Service.Services.ProductService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public interface IProductService
    {
        public Task<ProductDetailsDto> GetProductByIdAsync(int? id);
        public Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input);
        public Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync();
        public Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync();

    }
}
