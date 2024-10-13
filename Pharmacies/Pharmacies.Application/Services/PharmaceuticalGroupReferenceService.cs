using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services;

public class PharmaceuticalGroupReferenceService(
    IReferenceRepository<PharmaceuticalGroupReference, int, int> repository,
    IMapper mapper)
{
    public async Task<List<PharmaceuticalGroupReferenceDto>> GetAll()
    {
        var references = await repository.GetAll();
        return mapper.Map<List<PharmaceuticalGroupReferenceDto>>(references);
    }
    
    public async Task Delete(int pharmaceuticalGroupId, int positionId)
    {
        await repository.Delete(pharmaceuticalGroupId, positionId);
    }
}