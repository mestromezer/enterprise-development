namespace Pharmacies.Application.Interfaces;

public interface IEntityService<TEntityDto, in TKey>
{
    Task<List<TEntityDto>> GetAsList();
    Task<List<TEntityDto>> GetAll(Func<TEntityDto, bool> predicate);
    Task<TEntityDto?> GetByKey(TKey key);
    Task Add(TEntityDto entityDto);
    Task Update(TKey key, TEntityDto entityDto);
    Task Delete(TKey key);
}
