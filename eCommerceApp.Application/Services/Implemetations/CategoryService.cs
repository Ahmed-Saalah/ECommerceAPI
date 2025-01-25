using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;

namespace eCommerceApp.Application.Services.Implemetations
{
    public class CategoryServic(IGeneric<Category> categoryInterface, IMapper mapper) : ICategoryService
    {
        public async Task<ServiceResponse> AddAsync(CreateCategory category)
        {
            var mappData = mapper.Map<Category>(category);
            int result = await categoryInterface.AddAsync(mappData);
            if (result > 0)
                return new ServiceResponse(true, "category added!");

            return new ServiceResponse(false, "category faild to be added");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            int result = await categoryInterface.DeleteAsync(id);
            
            return result > 0 ? new ServiceResponse(true, "category deleted!")
                : new ServiceResponse(false, "category not dound or faild to be deleted");
        }

        public async Task<IEnumerable<GetCategory>> GetAllAsync()
        {
            var rawData = await categoryInterface.GetAllAsync();
            if (!rawData.Any())
                return [];

            return mapper.Map<IEnumerable<GetCategory>>(rawData);
        }

        public async Task<GetCategory> GetByIdAsync(Guid id)
        {
            var rawData = await categoryInterface.GetByIdAsync(id);
            if (rawData == null)
                return new GetCategory();

            return mapper.Map<GetCategory>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateCategory category)
        {
            var mappData = mapper.Map<Category>(category);
            int result = await categoryInterface.UpdateAsync(mappData);
            if (result > 0)
                return new ServiceResponse(true, "category updated!");

            return new ServiceResponse(false, "category faild to be updated");
        }
    }
}