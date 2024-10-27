namespace Pharmacies.Application.Interfaces;

public interface IReferenceService<TParentDto, TChildDto>
{
    public Task<IDictionary<TParentDto, IEnumerable<TChildDto>>> GetAllForAll();
    
    public Task<IEnumerable<TChildDto>> GetFor(TParentDto parentKey);

    public Task SetRelation(TParentDto parentKey, List<TChildDto> childKeys);
}