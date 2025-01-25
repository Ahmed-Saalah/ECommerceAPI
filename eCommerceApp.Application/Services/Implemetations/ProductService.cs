using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;

namespace eCommerceApp.Application.Services.Implemetations
{
    public class ProductService(IGeneric<Product> productInterface, IMapper mapper) : IProductService
    {
        public async Task<ServiceResponse> AddAsync(CreateProduct product)
        {
            var mappData = mapper.Map<Product>(product);
            int result = await productInterface.AddAsync(mappData);
            if (result > 0)
                return new ServiceResponse(true, "Product added!");

            return new ServiceResponse(false, "Product faild to be added");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            int result = await productInterface.DeleteAsync(id);

            return result > 0 ? new ServiceResponse(true, "Product deleted!") 
                : new ServiceResponse(false, "Product not dound or faild to be deleted");
        }

        public async Task<IEnumerable<GetProduct>> GetAllAsync()
        {
           var rawData = await productInterface.GetAllAsync();
            if (!rawData.Any())
                return [];

            return   mapper.Map<IEnumerable<GetProduct>>(rawData);
        }

        public async Task<GetProduct> GetByIdAsync(Guid id)
        {
            var rawData = await productInterface.GetByIdAsync(id);
            if (rawData == null)
                return new GetProduct();

            return mapper.Map<GetProduct>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateProduct product)
        {
            var mappData = mapper.Map<Product>(product);
            int result = await productInterface.UpdateAsync(mappData);
            if (result > 0)
                return new ServiceResponse(true, "Product updated!");

            return new ServiceResponse(false, "Product faild to be updated");
        }
    }
}
