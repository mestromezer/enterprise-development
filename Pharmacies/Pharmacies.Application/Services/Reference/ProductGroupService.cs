using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services.Reference;

public class ProductGroupService(IRepository<ProductGroup, int> repository, IMapper mapper)
    : IEntityService<ProductGroupDto, int>
{
    public async Task<List<ProductGroupDto>> GetAll()
    {
        var productGroups = await repository.GetAsList();
        return mapper.Map<List<ProductGroupDto>>(productGroups);
    }

    public async Task<List<ProductGroupDto>> GetAll(Func<ProductGroupDto, bool> predicate)
    {
        var productGroups = await repository.GetAsList();
        var productGroupDtos = mapper.Map<List<ProductGroupDto>>(productGroups);
        return productGroupDtos.Where(predicate).ToList();
    }

    public async Task<ProductGroupDto?> GetByKey(int key)
    {
        var productGroup = await repository.GetByKey(key);
        return productGroup != null ? mapper.Map<ProductGroupDto>(productGroup) : null;
    }

    public async Task Add(ProductGroupDto entityDto)
    {
        var productGroup = mapper.Map<ProductGroup>(entityDto);
        await repository.Add(productGroup);
    }

    public async Task Update(int key, ProductGroupDto entityDto)
    {
        var productGroup = mapper.Map<ProductGroup>(entityDto);
        await repository.Update(key, productGroup);
    }

    public async Task Delete(int key)
    {
        await repository.Delete(key);
    }
}