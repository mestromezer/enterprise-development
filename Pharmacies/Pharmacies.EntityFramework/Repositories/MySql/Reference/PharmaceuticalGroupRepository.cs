using Microsoft.EntityFrameworkCore;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.Repositories.MySql.Reference;

public class PharmaceuticalGroupRepository(PharmacyMySqlContext context) : IRepository<PharmaceuticalGroup, int>
{
    public async Task<List<PharmaceuticalGroup>> GetAsList()
    {
        return await context.PharmaceuticalGroups.ToListAsync();
    }

    public async Task<PharmaceuticalGroup?> GetByKey(int key)
    {
        return await context.PharmaceuticalGroups.FindAsync(key);
    }

    public async Task Add(PharmaceuticalGroup newRecord)
    {
        await context.PharmaceuticalGroups.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var pharmaceuticalGroup = await context.PharmaceuticalGroups.FindAsync(key);
        if (pharmaceuticalGroup != null)
        {
            context.PharmaceuticalGroups.Remove(pharmaceuticalGroup);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(int key, PharmaceuticalGroup newValue)
    {
        var pharmaceuticalGroup = await context.PharmaceuticalGroups.FindAsync(key);
        if (pharmaceuticalGroup != null)
        {
            pharmaceuticalGroup.Name = newValue.Name;

            context.PharmaceuticalGroups.Update(pharmaceuticalGroup);
            await context.SaveChangesAsync();
        }
    }
}