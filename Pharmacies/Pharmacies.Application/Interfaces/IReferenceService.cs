namespace Pharmacies.Application.Interfaces;

public interface IReferenceService<TParentKey, TChildKey>
{
    public Task<IDictionary<TParentKey, IEnumerable<TChildKey>>> GetAllForAll();
    
    public Task<IEnumerable<TChildKey>> GetFor(TParentKey parentKey);

    public Task SetRelation(TParentKey parentKey, List<TChildKey> childKeys);
}