using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Repositories.Mocks;

public class PriceRepositoryMock : IRepository<Price, int>
{
    private readonly ConcurrentDictionary<int, Price> _prices = new();
    private int _currentId = 0;

    public Task<List<Price>> GetAsList()
    {
        return Task.FromResult(_prices.Values.ToList());
    }

    public Task<Price?> GetByKey(int key)
    {
        _prices.TryGetValue(key, out var price);
        return Task.FromResult(price);
    }

    public Task Add(Price newRecord)
    {
        if (newRecord.Id == -1)
        {
            newRecord.Id = ++_currentId;
        }
        
        var added = _prices.TryAdd(newRecord.Id, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A price with ID {newRecord.Id} already exists.");
        }
        
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = _prices.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No price found with ID {key}.");
        }
        
        return Task.CompletedTask;
    }

    public Task Update(int key, Price newValue)
    {
        if (!_prices.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No price found with ID {key}.");
        }

        _prices[key] = newValue;
        return Task.CompletedTask;
    }
}