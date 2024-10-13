using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services;

public class PositionService(
    IRepository<Position, int> positionRepository,
    IRepository<ProductGroup, int> productGroupRepository,
    IRepository<PharmaceuticalGroup, int> pharmaceuticalGroupRepository,
    IRepository<Pharmacy, int> pharmacyRepository,
    IRepository<Price, int> priceRepository,
    IMapper mapper)
    : IEntityService<PositionDto, int>
{
    public async Task<List<PositionDto>> GetAll()
    {
        var positions = await positionRepository.GetAsList();
        return mapper.Map<List<PositionDto>>(positions);
    }

    public async Task<List<PositionDto>> GetAll(Func<PositionDto, bool> predicate)
    {
        var positions = await positionRepository.GetAsList();
        var mapped = mapper.Map<List<PositionDto>>(positions);
        return mapped.Where(predicate).ToList();
    }

    public async Task<PositionDto?> GetByKey(int key)
    {
        var position = await positionRepository.GetByKey(key);
        return mapper.Map<PositionDto>(position);
    }

    public async Task Add(PositionDto entityDto)
    {
        var position = mapper.Map<Position>(entityDto);

        if (entityDto.ProductGroupId.HasValue)
        {
            position.ProductGroup = await productGroupRepository.GetByKey(entityDto.ProductGroupId.Value);
        }

        if (entityDto.PharmaceuticalGroupIds.Count != 0)
        {
            position.PharmaceuticalGroups = (await Task.WhenAll(entityDto.PharmaceuticalGroupIds.Select(pharmaceuticalGroupRepository.GetByKey)))
                .Where(pg => pg != null).ToList();
        }

        if (entityDto.PharmacyId.HasValue)
        {
            position.Pharmacy = await pharmacyRepository.GetByKey(entityDto.PharmacyId.Value);
        }

        if (entityDto.PriceId.HasValue)
        {
            position.Price = await priceRepository.GetByKey(entityDto.PriceId.Value);
        }

        await positionRepository.Add(position);
    }

    public async Task Update(int key, PositionDto entityDto)
    {
        var position = await positionRepository.GetByKey(key);
        if (position == null)
        {
            throw new Exception($"Position with key {key} not found.");
        }

        mapper.Map(entityDto, position);

        if (entityDto.ProductGroupId.HasValue)
        {
            position.ProductGroup = await productGroupRepository.GetByKey(entityDto.ProductGroupId.Value);
        }

        if (entityDto.PharmaceuticalGroupIds.Count != 0)
        {
            position.PharmaceuticalGroups = (await Task.WhenAll(entityDto.PharmaceuticalGroupIds.Select(id => pharmaceuticalGroupRepository.GetByKey(id))))
                .Where(pg => pg != null).ToList();
        }

        if (entityDto.PharmacyId.HasValue)
        {
            position.Pharmacy = await pharmacyRepository.GetByKey(entityDto.PharmacyId.Value);
        }

        if (entityDto.PriceId.HasValue)
        {
            position.Price = await priceRepository.GetByKey(entityDto.PriceId.Value);
        }

        await positionRepository.Update(key, position);
    }

    public async Task Delete(int key)
    {
        await positionRepository.Delete(key);
    }
}