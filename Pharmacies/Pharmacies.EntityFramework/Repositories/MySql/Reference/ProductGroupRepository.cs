using Microsoft.EntityFrameworkCore;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.Repositories.MySql.Reference;

public class ProductGroupRepository(PharmacyMySqlContext context) : IRepository<ProductGroup, int>
{
    public async Task<List<ProductGroup>> GetAsList()
    {
        return await context.ProductGroups.ToListAsync();
    }

    public async Task<ProductGroup?> GetByKey(int key)
    {
        return await context.ProductGroups.FindAsync(key);
    }

    public async Task Add(ProductGroup newRecord)
    {
        await context.ProductGroups.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var productGroup = await context.ProductGroups.FindAsync(key);
        if (productGroup != null)
        {
            context.ProductGroups.Remove(productGroup);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(int key, ProductGroup newValue)
    {
        var productGroup = await context.ProductGroups.FindAsync(key);
        if (productGroup != null)
        {
            productGroup.Name = newValue.Name;

            context.ProductGroups.Update(productGroup);
            await context.SaveChangesAsync();
        }
    }
}