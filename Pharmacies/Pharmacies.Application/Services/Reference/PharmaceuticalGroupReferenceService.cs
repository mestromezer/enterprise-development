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
    IReferenceRepository<Position, PharmaceuticalGroup> referenceRepository)
    : IReferenceService<PositionDto, PharmaceuticalGroupDto>
{
    public async Task<IDictionary<PositionDto, IEnumerable<PharmaceuticalGroupDto>>> GetAllForAll()
    {
        var allRelations = await referenceRepository.GetAllForAll();

        var result = allRelations.ToDictionary(
            kvp => mapper.Map<PositionDto>(kvp.Key),
            kvp => kvp.Value.Select(child => mapper.Map<PharmaceuticalGroupDto>(child))
        );

        return result;
    }

    public async Task<IEnumerable<PharmaceuticalGroupDto>> GetFor(PositionDto parentKey)
    {
        var parentEntity = mapper.Map<Position>(parentKey);

        var relatedEntities = await referenceRepository.GetFor(parentEntity);

        return relatedEntities.Select(child => mapper.Map<PharmaceuticalGroupDto>(child));
    }

    public async Task SetRelation(PositionDto parentKey, List<PharmaceuticalGroupDto> childKeys)
    {
        var parentEntity = mapper.Map<Position>(parentKey);
        var childEntities = childKeys.Select(child => mapper.Map<PharmaceuticalGroup>(child)).ToList();

        await referenceRepository.SetRelation(parentEntity, childEntities);
    }
}