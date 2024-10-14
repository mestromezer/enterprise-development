namespace Pharmacies.Interfaces;

public interface IRepository<TEntity, in TKey>
{
    Task<List<TEntity>> GetAsList();
    Task<TEntity?> GetByKey(TKey key);
    Task Add(TEntity newRecord);
    Task Delete(TKey key);
    Task Update(TKey key,TEntity newValue);
}