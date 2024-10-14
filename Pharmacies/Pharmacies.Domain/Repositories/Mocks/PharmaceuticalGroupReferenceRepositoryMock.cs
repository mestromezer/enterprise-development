using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class PharmaceuticalGroupReferenceRepository : IRepository<PharmaceuticalGroupReference, int>
{
    private static readonly ConcurrentDictionary<int, PharmaceuticalGroupReference> References = new();
    private static int _currentId = 1;

    public Task<List<PharmaceuticalGroupReference>> GetAsList()
    {
        return Task.FromResult(References.Values.ToList());
    }

    public Task<PharmaceuticalGroupReference?> GetByKey(int key)
    {
        References.TryGetValue(key, out var reference);
        return Task.FromResult(reference);
    }

    public Task Add(PharmaceuticalGroupReference newRecord)
    {
        newRecord.Id = _currentId++;
        References.TryAdd(newRecord.Id, newRecord);
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        References.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task Update(int key, PharmaceuticalGroupReference newValue)
    {
        if (!References.TryGetValue(key, out var reference)) return Task.CompletedTask;
        newValue.Id = key;
        reference.PharmaceuticalGroupId = newValue.PharmaceuticalGroupId;
        reference.PositionId = newValue.PositionId;
        return Task.CompletedTask;
    }
}