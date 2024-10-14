using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class ProductGroupRepositoryMock : IRepository<ProductGroup, int>
{
    private static readonly ConcurrentDictionary<int, ProductGroup> ProductGroups = new();
    private static int _currentId = 1;

    public Task<List<ProductGroup>> GetAsList()
    {
        return Task.FromResult(ProductGroups.Values.ToList());
    }

    public Task<ProductGroup?> GetByKey(int key)
    {
        ProductGroups.TryGetValue(key, out var productGroup);
        return Task.FromResult(productGroup);
    }

    public Task Add(ProductGroup newRecord)
    {
        newRecord.Id = _currentId++;

        var added = ProductGroups.TryAdd(newRecord.Id, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A product group with ID {newRecord.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = ProductGroups.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No product group found with ID {key}.");
        }

        return Task.CompletedTask;
    }

    public Task Update(int key, ProductGroup newValue)
    {
        if (!ProductGroups.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No product group found with ID {key}.");
        }

        newValue.Id = key;
        ProductGroups[key] = newValue;
        return Task.CompletedTask;
    }
}