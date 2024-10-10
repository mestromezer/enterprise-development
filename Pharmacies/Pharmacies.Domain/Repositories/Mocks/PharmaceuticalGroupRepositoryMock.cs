using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class PharmaceuticalGroupRepositoryMock : IRepository<PharmaceuticalGroup, int>
{
    private readonly ConcurrentDictionary<int, PharmaceuticalGroup> _pharmaceuticalGroups = new();

    public Task<List<PharmaceuticalGroup>> GetAsList()
    {
        return Task.FromResult(_pharmaceuticalGroups.Values.ToList());
    }

    public Task<List<PharmaceuticalGroup>> GetAsList(Func<PharmaceuticalGroup, bool> predicate)
    {
        return Task.FromResult(_pharmaceuticalGroups.Values.Where(predicate).ToList());
    }

    public Task Add(PharmaceuticalGroup newRecord)
    {
        _pharmaceuticalGroups.TryAdd(newRecord.Id, newRecord);
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        _pharmaceuticalGroups.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task Update(PharmaceuticalGroup newValue)
    {
        if (_pharmaceuticalGroups.ContainsKey(newValue.Id))
        {
            _pharmaceuticalGroups[newValue.Id] = newValue;
        }
        return Task.CompletedTask;
    }
}