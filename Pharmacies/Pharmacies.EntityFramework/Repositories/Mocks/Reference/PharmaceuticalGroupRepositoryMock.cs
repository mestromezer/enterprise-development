using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.Repositories.Mocks.Reference;

public class PharmaceuticalGroupRepositoryMock : IRepository<PharmaceuticalGroup, int>
{
    private static readonly ConcurrentDictionary<int, PharmaceuticalGroup> PharmaceuticalGroups = new();
    private static int _currentId = 1;

    public Task<List<PharmaceuticalGroup>> GetAsList() => Task.FromResult(PharmaceuticalGroups.Values.ToList());

    public Task<PharmaceuticalGroup?> GetByKey(int key)
    {
        PharmaceuticalGroups.TryGetValue(key, out var pharmaceuticalGroup);
        return Task.FromResult(pharmaceuticalGroup);
    }

    public Task Add(PharmaceuticalGroup newRecord)
    {
        newRecord.Id = _currentId++;

        var added = PharmaceuticalGroups.TryAdd(newRecord.Id, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A pharmaceutical group with ID {newRecord.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = PharmaceuticalGroups.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No pharmaceutical group found with ID {key}.");
        }

        return Task.CompletedTask;
    }

    public Task Update(int key, PharmaceuticalGroup newValue)
    {
        if (!PharmaceuticalGroups.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No pharmaceutical group found with ID {key}.");
        }

        newValue.Id = key;
        PharmaceuticalGroups[key] = newValue;
        return Task.CompletedTask;
    }
}