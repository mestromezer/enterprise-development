using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class ProductGroupRepositoryMock : IRepository<ProductGroup, int>
{
    private readonly ConcurrentDictionary<int, ProductGroup> _productGroups = new();
    private int _currentId = 0;

    public Task<List<ProductGroup>> GetAsList()
    {
        return Task.FromResult(_productGroups.Values.ToList());
    }

    public Task<ProductGroup?> GetByKey(int key)
    {
        _productGroups.TryGetValue(key, out var productGroup);
        return Task.FromResult(productGroup);
    }

    public Task Add(ProductGroup newRecord)
    {
        if (newRecord.Id == -1)
        {
            newRecord.Id = ++_currentId;
        }
        
        var added = _productGroups.TryAdd(newRecord.Id, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A product group with ID {newRecord.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = _productGroups.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No product group found with ID {key}.");
        }

        return Task.CompletedTask;
    }

    public Task Update(int key, ProductGroup newValue)
    {
        if (!_productGroups.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No product group found with ID {key}.");
        }

        _productGroups[key] = newValue;
        return Task.CompletedTask;
    }
}