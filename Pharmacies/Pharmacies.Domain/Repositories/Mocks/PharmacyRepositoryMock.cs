using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Repositories.Mocks;

public class PharmacyRepositoryMock : IRepository<Pharmacy, int>
{
    private static readonly ConcurrentDictionary<int, Pharmacy> Pharmacies = new();

    public Task<List<Pharmacy>> GetAsList()
    {
        return Task.FromResult(Pharmacies.Values.ToList());
    }

    public Task<Pharmacy?> GetByKey(int key)
    {
        Pharmacies.TryGetValue(key, out var pharmacy);
        return Task.FromResult(pharmacy);
    }

    public Task Add(Pharmacy newRecord)
    {
        var added = Pharmacies.TryAdd(newRecord.Number, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A pharmacy with number {newRecord.Number} already exists.");
        }
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = Pharmacies.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No pharmacy found with number {key}.");
        }
        return Task.CompletedTask;
    }

    public Task Update(int key, Pharmacy newValue)
    {
        if (!Pharmacies.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No pharmacy found with number {key}.");
        }

        Pharmacies[key] = newValue;
        return Task.CompletedTask;
    }
}