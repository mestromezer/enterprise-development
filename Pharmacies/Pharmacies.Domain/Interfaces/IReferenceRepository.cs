namespace Pharmacies.Interfaces;

public interface IReferenceRepository<TParent, TChild, TParentKey, TChildKey>
{
    public Task<IDictionary<TParent, IEnumerable<TChild>>> GetAllForAll();
    
    public Task<IEnumerable<TChild>> GetFor(TParentKey parentKey);

    public Task SetRelation(TParentKey parentKey, List<TChildKey> childKeys);
}