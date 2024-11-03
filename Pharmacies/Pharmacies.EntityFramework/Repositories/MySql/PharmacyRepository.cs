using Microsoft.EntityFrameworkCore;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.EntityFramework.Repositories.MySql;

public class PharmacyRepository(PharmacyMySqlContext context) : IRepository<Pharmacy, int>
{
    public async Task<List<Pharmacy>> GetAsList()
    {
        return await context.Pharmacies.ToListAsync();
    }

    public async Task<Pharmacy?> GetByKey(int key)
    {
        return await context.Pharmacies.FindAsync(key);
    }

    public async Task Add(Pharmacy newRecord)
    {
        await context.Pharmacies.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var pharmacy = await context.Pharmacies.FindAsync(key);
        if (pharmacy != null)
        {
            context.Pharmacies.Remove(pharmacy);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(int key, Pharmacy newValue)
    {
        var pharmacy = await context.Pharmacies.FindAsync(key);
        if (pharmacy != null)
        {
            pharmacy.Name = newValue.Name;
            pharmacy.Phone = newValue.Phone;
            pharmacy.Address = newValue.Address;
            pharmacy.DirectorFullName = newValue.DirectorFullName;
                
            context.Pharmacies.Update(pharmacy);
            await context.SaveChangesAsync();
        }
    }
}