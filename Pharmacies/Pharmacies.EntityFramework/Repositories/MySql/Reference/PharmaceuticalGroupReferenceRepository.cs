using Microsoft.EntityFrameworkCore;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.Repositories.MySql.Reference;

public class PositionPharmaceuticalGroupRepository(PharmacyMySqlContext context)
    : IReferenceRepository<Position, PharmaceuticalGroup, int, int>
{
    public async Task<IDictionary<Position, IEnumerable<PharmaceuticalGroup>>> GetAllForAll()
    {
        return await context.PharmaceuticalGroupReferences
            .Include(pgr => pgr.Position)
            .Include(pgr => pgr.PharmaceuticalGroup)
            .Where(pgr => pgr.Position != null && pgr.PharmaceuticalGroup != null)
            .GroupBy(pgr => pgr.Position)
            .ToDictionaryAsync(
                g => g.Key!,
                g => g.Select(pgr => pgr.PharmaceuticalGroup!).Where(group => group != null)
            );
    }

    public async Task<IEnumerable<PharmaceuticalGroup>> GetFor(int parentKey)
    {
        return await context.PharmaceuticalGroupReferences
            .Where(pgr => pgr.PositionId == parentKey && pgr.PharmaceuticalGroup != null)
            .Select(pgr => pgr.PharmaceuticalGroup!)
            .ToListAsync();
    }

    public async Task SetRelation(int parentKey, List<int> childKeys)
    {
        var existingReferences = await context.PharmaceuticalGroupReferences
            .Where(pgr => pgr.PositionId == parentKey)
            .ToListAsync();

        var existingChildIds = existingReferences
            .Select(pgr => pgr.PharmaceuticalGroupId ?? 0)
            .ToHashSet();
        
        var referencesToAdd = childKeys
            .Where(childKey => !existingChildIds.Contains(childKey))
            .Select(childKey => new PharmaceuticalGroupReference
            {
                Id = -1,
                PositionId = parentKey,
                PharmaceuticalGroupId = childKey
            });

        await context.PharmaceuticalGroupReferences.AddRangeAsync(referencesToAdd);
        await context.SaveChangesAsync();
    }
}