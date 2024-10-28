using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.EntityFramework.Repositories.Mocks;

public class PriceRepositoryMock : IRepository<Price, int>
{
    private static readonly ConcurrentDictionary<int, Price> Prices = new();
    private static int _currentId = 1;

    public Task<List<Price>> GetAsList()
    {
        return Task.FromResult(Prices.Values.ToList());
    }

    public Task<Price?> GetByKey(int key)
    {
        Prices.TryGetValue(key, out var price);
        return Task.FromResult(price);
    }

    public Task Add(Price newRecord)
    {
        newRecord.Id = _currentId ++;

        var added = Prices.TryAdd(newRecord.Id, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A price with ID {newRecord.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = Prices.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No price found with ID {key}.");
        }

        return Task.CompletedTask;
    }

    public Task Update(int key, Price newValue)
    {
        if (!Prices.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No price found with ID {key}.");
        }

        newValue.Id = key;
        Prices[key] = newValue;
        return Task.CompletedTask;
    }
}