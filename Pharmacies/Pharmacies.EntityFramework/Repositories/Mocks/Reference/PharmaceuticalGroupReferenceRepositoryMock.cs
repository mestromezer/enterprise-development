using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.Repositories.Mocks.Reference;

public class PharmaceuticalGroupReferenceRepository(
    PositionRepositoryMock positionRepository,
    PharmaceuticalGroupRepositoryMock pharmaceuticalGroupRepository)
    : IReferenceRepository<Position, PharmaceuticalGroup>
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

    public async Task<IEnumerable<PharmaceuticalGroup>> GetFor(Position parent)
    {
        if (!PositionPharmaceuticalGroupRelations.TryGetValue(parent.Code, out var groupIds))
        {
            return Enumerable.Empty<PharmaceuticalGroup>();
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

    public Task SetRelation(Position parent, List<PharmaceuticalGroup> childEntities)
    {
        var groupIds = childEntities.Select(group => group.Id).ToList();
        PositionPharmaceuticalGroupRelations[parent.Code] = groupIds;

        return Task.CompletedTask;
    }
}
