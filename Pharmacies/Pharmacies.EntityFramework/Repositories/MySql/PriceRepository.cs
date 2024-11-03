using Microsoft.EntityFrameworkCore;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.EntityFramework.Repositories.MySql;

public class PriceRepository(PharmacyMySqlContext context) : IRepository<Price, int>
{
    public async Task<List<Price>> GetAsList()
    {
        return await context.Prices.ToListAsync();
    }

    public async Task<Price?> GetByKey(int key)
    {
        return await context.Prices.FindAsync(key);
    }

    public async Task Add(Price newRecord)
    {
        await context.Prices.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var price = await context.Prices.FindAsync(key);
        if (price != null)
        {
            context.Prices.Remove(price);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(int key, Price newValue)
    {
        var price = await context.Prices.FindAsync(key);
        if (price != null)
        {
            price.Manufacturer = newValue.Manufacturer;
            price.IfCash = newValue.IfCash;
            price.SellerOrganizationName = newValue.SellerOrganizationName;
            price.ProductionTime = newValue.ProductionTime;
            price.Cost = newValue.Cost;
            price.SellTime = newValue.SellTime;

            context.Prices.Update(price);
            await context.SaveChangesAsync();
        }
    }
}