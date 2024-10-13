using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks;

public class PharmaceuticalGroupReferenceRepository : IReferenceRepository<PharmaceuticalGroupReference, int, int>
{
    private readonly ConcurrentDictionary<(int, int), PharmaceuticalGroupReference> _references = new();

    public Task<List<PharmaceuticalGroupReference>> GetAll()
    {
        return Task.FromResult(_references.Values.ToList());
    }

    public Task Delete(int pharmaceuticalGroupId, int positionId)
    {
        var key = (pharmaceuticalGroupId, positionId);
        var removed = _references.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No reference found with key ({key.Item1}, {key.Item2}).");
        }

        return Task.CompletedTask;
    }
}