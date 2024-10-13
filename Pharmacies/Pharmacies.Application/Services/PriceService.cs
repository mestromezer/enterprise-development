using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Application.Services;

public class PriceService(IRepository<Price, int> priceRepository, IMapper mapper) : IEntityService<PriceDto, int>
{
    public async Task<List<PriceDto>> GetAll()
    {
        var prices = await priceRepository.GetAsList();
        return mapper.Map<List<PriceDto>>(prices);
    }

    public async Task<List<PriceDto>> GetAll(Func<PriceDto, bool> predicate)
    {
        var prices = await priceRepository.GetAsList();
        var mapped = mapper.Map<List<PriceDto>>(prices);
        return mapped.Where(predicate).ToList();
    }

    public async Task<PriceDto?> GetByKey(int key)
    {
        var price = await priceRepository.GetByKey(key);
        return mapper.Map<PriceDto>(price);
    }

    public async Task Add(PriceDto entityDto)
    {
        var price = mapper.Map<Price>(entityDto);
        await priceRepository.Add(price);
    }

    public async Task Update(int key, PriceDto entityDto)
    {
        var price = await priceRepository.GetByKey(key);
        if (price == null)
        {
            throw new Exception($"Price with key {key} not found.");
        }

        mapper.Map(entityDto, price);
        await priceRepository.Update(key, price);
    }

    public async Task Delete(int key)
    {
        var price = await priceRepository.GetByKey(key);
        if (price == null)
        {
            throw new Exception($"Price with key {key} not found.");
        }

        await priceRepository.Delete(key);
    }
}