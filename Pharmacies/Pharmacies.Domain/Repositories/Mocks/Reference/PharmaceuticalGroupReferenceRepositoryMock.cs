using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.Repositories.Mocks.Reference
{
    public class PharmaceuticalGroupReferenceRepository (IRepository<PharmaceuticalGroup, int> pharmaceuticalGroupRepository,
        IRepository<Position, int> positionRepository
        ) : IRepository<PharmaceuticalGroupReference, int>
    {
        private static readonly ConcurrentDictionary<int, PharmaceuticalGroupReference> References = new();
        private static int _currentId = 1;

        public Task<List<PharmaceuticalGroupReference>> GetAsList() =>
            Task.FromResult(References.Values.ToList());

        public Task<PharmaceuticalGroupReference?> GetByKey(int key) =>
            References.TryGetValue(key, out var reference) ? Task.FromResult(reference) : null;

        public async Task Add(PharmaceuticalGroupReference newRecord)
        {
            if (newRecord == null)
            {
                throw new ArgumentNullException(nameof(newRecord), "New record cannot be null.");
            }

            await CheckBothExist(newRecord);

            newRecord.Id = _currentId++;
            if (!References.TryAdd(newRecord.Id, newRecord))
            {
                throw new InvalidOperationException("Failed to add new record.");
            }
        }

        public Task Delete(int key)
        {
            if (!References.TryRemove(key, out _))
            {
                throw new KeyNotFoundException($"No record found with key {key} to delete.");
            }

            return Task.CompletedTask;
        }

        public async Task Update(int key, PharmaceuticalGroupReference newValue)
        {
            if (newValue == null)
            {
                throw new ArgumentNullException(nameof(newValue), "Updated record cannot be null.");
            }

            if (!References.TryGetValue(key, out var reference))
            {
                throw new KeyNotFoundException($"No record found with key {key} to update.");
            }
            
            await CheckBothExist(newValue);

            newValue.Id = key;
            reference.PharmaceuticalGroupId = newValue.PharmaceuticalGroupId;
            reference.PositionId = newValue.PositionId;
        }

        private async Task CheckBothExist(PharmaceuticalGroupReference newRecord)
        {
            if (await pharmaceuticalGroupRepository.GetByKey(newRecord.PharmaceuticalGroupId) == null)
            {
                throw new ArgumentException("Invalid PharmaceuticalGroupId.");
            }
            
            if (await positionRepository.GetByKey(newRecord.PositionId) == null)
            {
                throw new ArgumentException("Invalid PositionId.");
            }
        }
    }
}
