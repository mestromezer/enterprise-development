namespace Pharmacies.Application.Interfaces;

public interface IReferenceService<TEntity, TKey1, TKey2>
{
    /// <summary>
    /// Получить все записи.
    /// </summary>
    Task<List<TEntity>> GetAll();

    /// <summary>
    /// Удалить запись по двум ключам.
    /// </summary>
    Task Delete(TKey1 key1, TKey2 key2);
}