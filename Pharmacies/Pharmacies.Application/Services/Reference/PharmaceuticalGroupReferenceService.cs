using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services.Reference;


/// <summary>
/// Сервис на будущее, так как в базе я буду хранить допустимые фарм. группы.
/// Ими можно будет рулить, на них ссылаться.
/// Связь многие ко многим => нужна таблица где вся эта рабость будет храниться.
/// Через этот сервис будет происходить связка 
/// </summary>
public class PharmaceuticalGroupReferenceService(
    IRepository<PharmaceuticalGroupReference, int> repository,
    IMapper mapper)
    : IEntityService<PharmaceuticalGroupReferenceDto, int>
{
    public async Task<List<PharmaceuticalGroupReferenceDto>> GetAll()
    {
        var entities = await repository.GetAsList();
        return entities.Select(mapper.Map<PharmaceuticalGroupReferenceDto>).ToList();
    }
    
    public async Task<List<PharmaceuticalGroupReferenceDto>> GetAll(Func<PharmaceuticalGroupReferenceDto, bool> predicate)
    {
        var entities = await repository.GetAsList();
        return entities.Select(mapper.Map<PharmaceuticalGroupReferenceDto>)
                       .Where(predicate)
                       .ToList();
    }
    
    public async Task<PharmaceuticalGroupReferenceDto?> GetByKey(int key)
    {
        var entity = await repository.GetByKey(key);
        return entity == null ? null : mapper.Map<PharmaceuticalGroupReferenceDto>(entity);
    }
    
    public async Task Add(PharmaceuticalGroupReferenceDto entityDto)
    {
        var entity = mapper.Map<PharmaceuticalGroupReference>(entityDto);
        await repository.Add(entity);
    }
    
    public async Task Update(int key, PharmaceuticalGroupReferenceDto entityDto)
    {
        var entity = mapper.Map<PharmaceuticalGroupReference>(entityDto);
        await repository.Update(key, entity);
    }
    
    public async Task Delete(int key)
    {
        await repository.Delete(key);
    }
}