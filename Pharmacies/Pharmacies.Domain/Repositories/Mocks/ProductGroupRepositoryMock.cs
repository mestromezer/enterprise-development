using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class ProductGroupRepositoryMock : IRepository<ProductGroup, int>
{
    private readonly ConcurrentDictionary<int, ProductGroup> _productGroups = new();

    public Task<List<ProductGroup>> GetAsList()
    {
        return Task.FromResult(_productGroups.Values.ToList());
    }

    public Task<List<ProductGroup>> GetAsList(Func<ProductGroup, bool> predicate)
    {
        return Task.FromResult(_productGroups.Values.Where(predicate).ToList());
    }

    public Task Add(ProductGroup newRecord)
    {
        _productGroups.TryAdd(newRecord.Id, newRecord);
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        _productGroups.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task Update(ProductGroup newValue)
    {
        if (_productGroups.ContainsKey(newValue.Id))
        {
            _productGroups[newValue.Id] = newValue;
        }
        return Task.CompletedTask;
    }
}