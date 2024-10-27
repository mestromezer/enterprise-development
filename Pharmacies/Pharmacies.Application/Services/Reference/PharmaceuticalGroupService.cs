using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services.Reference;

public class PharmaceuticalGroupService(IRepository<PharmaceuticalGroup, int> repository, IMapper mapper)
    : IEntityService<PharmaceuticalGroupDto, int>
{
    public async Task<List<PharmaceuticalGroupDto>> GetAll()
    {
        var pharmaceuticalGroups = await repository.GetAsList();
        return mapper.Map<List<PharmaceuticalGroupDto>>(pharmaceuticalGroups);
    }

    public async Task<List<PharmaceuticalGroupDto>> GetAll(Func<PharmaceuticalGroupDto, bool> predicate)
    {
        var pharmaceuticalGroups = await repository.GetAsList();
        var pharmaceuticalGroupDtos = mapper.Map<List<PharmaceuticalGroupDto>>(pharmaceuticalGroups);
        return pharmaceuticalGroupDtos.Where(predicate).ToList();
    }

    public async Task<PharmaceuticalGroupDto?> GetByKey(int key)
    {
        var pharmaceuticalGroup = await repository.GetByKey(key);
        return pharmaceuticalGroup != null ? mapper.Map<PharmaceuticalGroupDto>(pharmaceuticalGroup) : null;
    }

    public async Task Add(PharmaceuticalGroupDto entityDto)
    {
        var pharmaceuticalGroup = mapper.Map<PharmaceuticalGroup>(entityDto);
        await repository.Add(pharmaceuticalGroup);
    }

    public async Task Update(int key, PharmaceuticalGroupDto entityDto)
    {
        var pharmaceuticalGroup = mapper.Map<PharmaceuticalGroup>(entityDto);
        await repository.Update(key, pharmaceuticalGroup);
    }

    public async Task Delete(int key)
    {
        await repository.Delete(key);
    }
}