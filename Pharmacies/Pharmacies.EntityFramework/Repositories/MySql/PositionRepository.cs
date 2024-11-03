using Microsoft.EntityFrameworkCore;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.EntityFramework.Repositories.MySql;

public class PositionRepository(PharmacyMySqlContext context) : IRepository<Position, int>
{
    public async Task<List<Position>> GetAsList()
    {
        return await context.Positions
            .Include(p => p.ProductGroup)
            .Include(p => p.Pharmacy)
            .Include(p => p.Price)
            .ToListAsync();
    }

    public async Task<Position?> GetByKey(int key)
    {
        return await context.Positions
            .Include(p => p.ProductGroup)
            .Include(p => p.Pharmacy)
            .Include(p => p.Price)
            .FirstOrDefaultAsync(p => p.Code == key);
    }

    public async Task Add(Position newRecord)
    {
        await context.Positions.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var position = await context.Positions.FindAsync(key);
        if (position != null)
        {
            context.Positions.Remove(position);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(int key, Position newValue)
    {
        var position = await context.Positions.FindAsync(key);
        if (position != null)
        {
            position.Name = newValue.Name;
            position.ProductGroupId = newValue.ProductGroupId;
            position.Quantity = newValue.Quantity;
            position.PharmacyId = newValue.PharmacyId;
            position.PriceId = newValue.PriceId;

            context.Positions.Update(position);
            await context.SaveChangesAsync();
        }
    }
}