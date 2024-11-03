using AutoMapper;
using Pharmacies.Application.Interfaces;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.Application.Services.Reference
{
    public class PharmaceuticalGroupReferenceService(
        IMapper mapper,
        IReferenceRepository<Position, PharmaceuticalGroup, int, int> referenceRepository)
        : IReferenceService<int, int>
    {
        private readonly IMapper _mapper = mapper;

        public async Task<IDictionary<int, IEnumerable<int>>> GetAllForAll()
        {
            var allRelations = await referenceRepository.GetAllForAll();

            var result = allRelations.ToDictionary(
                kvp => kvp.Key.Code,
                kvp => kvp.Value.Select(child => child.Id)
            );

            return result;
        }

        public async Task<IEnumerable<int>> GetFor(int parentKey)
        {
            var relatedEntities = await referenceRepository.GetFor(parentKey);

            return relatedEntities.Select(child => child.Id); // Возвращаем Id для каждой PharmaceuticalGroup
        }

        public async Task SetRelation(int parentKey, List<int> childKeys)
        {
            await referenceRepository.SetRelation(parentKey, childKeys);
        }
    }
}