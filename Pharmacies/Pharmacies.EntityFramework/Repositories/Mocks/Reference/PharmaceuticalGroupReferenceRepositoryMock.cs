using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.Repositories.Mocks.Reference;

public class PharmaceuticalGroupReferenceRepository(
    IRepository<Position, int> positionRepository,
    IRepository<PharmaceuticalGroup, int> pharmaceuticalGroupRepository)
    : IReferenceRepository<Position, PharmaceuticalGroup, int ,int>
{
    private static readonly ConcurrentDictionary<int, List<int>> PositionPharmaceuticalGroupRelations = new();

    public async Task<IDictionary<Position, IEnumerable<PharmaceuticalGroup>>> GetAllForAll()
    {
        var result = new Dictionary<Position, IEnumerable<PharmaceuticalGroup>>();

        foreach (var positionId in PositionPharmaceuticalGroupRelations.Keys)
        {
            var position = await positionRepository.GetByKey(positionId);
            if (position == null) continue;

            var groupIds = PositionPharmaceuticalGroupRelations[positionId];
            var groups = new List<PharmaceuticalGroup>();
            foreach (var groupId in groupIds)
            {
                var group = await pharmaceuticalGroupRepository.GetByKey(groupId);
                if (group != null)
                {
                    groups.Add(group);
                }
            }

            result[position] = groups;
        }

        return result;
    }

    public async Task<IEnumerable<PharmaceuticalGroup>> GetFor(int parentKey)
    {
        if (!PositionPharmaceuticalGroupRelations.TryGetValue(parentKey, out var groupIds))
        {
            return [];
        }

        var groups = new List<PharmaceuticalGroup>();
        foreach (var groupId in groupIds)
        {
            var group = await pharmaceuticalGroupRepository.GetByKey(groupId);
            if (group != null)
            {
                groups.Add(group);
            }
        }

        return groups;
    }

    public Task SetRelation(int parentKey, List<int> childEntitiesKeys)
    {
        PositionPharmaceuticalGroupRelations[parentKey] = childEntitiesKeys;

        return Task.CompletedTask;
    }
}
