namespace Pharmacies.Interfaces;

public interface IReferenceRepository<TParent, TChild>
{
    public Task<IDictionary<TParent, IEnumerable<TChild>>> GetAllForAll();
    
    public Task<IEnumerable<TChild>> GetFor(TParent parentKey);

    public Task SetRelation(TParent parentKey, List<TChild> childKeys);
}