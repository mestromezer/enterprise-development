using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Application.Services;

public class PharmacyService(IRepository<Pharmacy, int> pharmacyRepository, IMapper mapper)
    : IEntityService<PharmacyDto, int>
{
    public async Task<List<PharmacyDto>> GetAsList()
    {
        var pharmacies = await pharmacyRepository.GetAsList();
        return mapper.Map<List<PharmacyDto>>(pharmacies);
    }

    public async Task<List<PharmacyDto>> GetAll(Func<PharmacyDto, bool> predicate)
    {
        var pharmacies = await pharmacyRepository.GetAsList();
        var pharmacyDtos = mapper.Map<List<PharmacyDto>>(pharmacies);
        return pharmacyDtos.Where(predicate).ToList();
    }

    public async Task<PharmacyDto?> GetByKey(int number)
    {
        var pharmacy = await pharmacyRepository.GetByKey(number);
        return mapper.Map<PharmacyDto>(pharmacy);
    }

    public async Task Add(PharmacyDto pharmacyDto)
    {
        var pharmacy = mapper.Map<Pharmacy>(pharmacyDto);
        await pharmacyRepository.Add(pharmacy);
    }

    public async Task Update(int number, PharmacyDto pharmacyDto)
    {
        var pharmacy = mapper.Map<Pharmacy>(pharmacyDto);
        await pharmacyRepository.Update(number, pharmacy);
    }

    public async Task Delete(int number) =>  await pharmacyRepository.Delete(number);
}
