namespace Pharmacies.Application.Interfaces;

public interface IReferenceService<TParentDto, TChildDto, TParentKey, TChildKey>
{
    public Task<IDictionary<TParentDto, IEnumerable<TChildDto>>> GetAllForAll();
    
    public Task<IEnumerable<TChildDto>> GetFor(TParentKey parentKey);

    public Task SetRelation(TParentKey parentKey, List<TChildKey> childKeys);
}