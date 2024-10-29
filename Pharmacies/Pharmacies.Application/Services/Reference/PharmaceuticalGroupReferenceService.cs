using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services.Reference;

public class PharmaceuticalGroupReferenceService(
    IMapper mapper,
    IReferenceRepository<Position, PharmaceuticalGroup, int ,int> referenceRepository)
    : IReferenceService<PositionDto, PharmaceuticalGroupDto, int , int>
{
    public async Task<IDictionary<PositionDto, IEnumerable<PharmaceuticalGroupDto>>> GetAllForAll()
    {
        var allRelations = await referenceRepository.GetAllForAll();

        var result = allRelations.ToDictionary(
            kvp => mapper.Map<PositionDto>(kvp.Key),
            kvp => kvp.Value.Select(mapper.Map<PharmaceuticalGroupDto>)
        );

        return result;
    }

    public async Task<IEnumerable<PharmaceuticalGroupDto>> GetFor(int parentKey)
    {
        var relatedEntities = await referenceRepository.GetFor(parentKey);

        return relatedEntities.Select(mapper.Map<PharmaceuticalGroupDto>);
    }

    public async Task SetRelation(int parentKey, List<int> childKeys)
    {
        await referenceRepository.SetRelation(parentKey, childKeys);
    }
}