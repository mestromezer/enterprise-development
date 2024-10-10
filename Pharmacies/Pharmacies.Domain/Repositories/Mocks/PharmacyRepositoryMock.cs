using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Repositories.Mocks;

public class PharmacyRepositoryMock : IRepository<Pharmacy, int>
{
    private readonly ConcurrentDictionary<int, Pharmacy> _pharmacies = new();

    public Task<List<Pharmacy>> GetAsList()
    {
        return Task.FromResult(_pharmacies.Values.ToList());
    }

    public Task<List<Pharmacy>> GetAsList(Func<Pharmacy, bool> predicate)
    {
        return Task.FromResult(_pharmacies.Values.Where(predicate).ToList());
    }

    public Task Add(Pharmacy newRecord)
    {
        _pharmacies.TryAdd(newRecord.Number, newRecord);
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        _pharmacies.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task Update(Pharmacy newValue)
    {
        if (_pharmacies.ContainsKey(newValue.Number))
        {
            _pharmacies[newValue.Number] = newValue;
        }
        return Task.CompletedTask;
    }
}