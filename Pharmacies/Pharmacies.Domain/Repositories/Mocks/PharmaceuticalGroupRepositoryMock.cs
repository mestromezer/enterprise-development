using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class PharmaceuticalGroupRepositoryMock : IRepository<PharmaceuticalGroup, int>
{
    private readonly ConcurrentDictionary<int, PharmaceuticalGroup> _pharmaceuticalGroups = new();
    private int _currentId = 0;

    public Task<List<PharmaceuticalGroup>> GetAsList()
    {
        return Task.FromResult(_pharmaceuticalGroups.Values.ToList());
    }

    public Task<PharmaceuticalGroup?> GetByKey(int key)
    {
        _pharmaceuticalGroups.TryGetValue(key, out var pharmaceuticalGroup);
        return Task.FromResult(pharmaceuticalGroup);
    }

    public Task Add(PharmaceuticalGroup newRecord)
    {
        if (newRecord.Id == -1)
        {
            newRecord.Id = ++_currentId;
        }
            
        var added = _pharmaceuticalGroups.TryAdd(newRecord.Id, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A pharmaceutical group with ID {newRecord.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = _pharmaceuticalGroups.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No pharmaceutical group found with ID {key}.");
        }

        return Task.CompletedTask;
    }

    public Task Update(int key, PharmaceuticalGroup newValue)
    {
        if (!_pharmaceuticalGroups.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No pharmaceutical group found with ID {key}.");
        }

        _pharmaceuticalGroups[key] = newValue;
        return Task.CompletedTask;
    }
}