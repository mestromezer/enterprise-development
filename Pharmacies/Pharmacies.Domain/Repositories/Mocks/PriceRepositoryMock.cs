using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Repositories.Mocks;

public class PriceRepositoryMock : IRepository<Price, int>
{
    private readonly ConcurrentDictionary<int, Price> _prices = new();

    public Task<List<Price>> GetAsList()
    {
        return Task.FromResult(_prices.Values.ToList());
    }

    public Task<List<Price>> GetAsList(Func<Price, bool> predicate)
    {
        return Task.FromResult(_prices.Values.Where(predicate).ToList());
    }

    public Task Add(Price newRecord)
    {
        _prices.TryAdd(newRecord.Id, newRecord);
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        _prices.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task Update(Price newValue)
    {
        if (_prices.ContainsKey(newValue.Id))
        {
            _prices[newValue.Id] = newValue;
        }
        return Task.CompletedTask;
    }
}