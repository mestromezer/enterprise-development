using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Repositories.Mocks;

public class PositionRepositoryMock : IRepository<Position, int>
{
    private readonly ConcurrentDictionary<int, Position> _positions = new();

    public Task<List<Position>> GetAsList()
    {
        return Task.FromResult(_positions.Values.ToList());
    }

    public Task<List<Position>> GetAsList(Func<Position, bool> predicate)
    {
        return Task.FromResult(_positions.Values.Where(predicate).ToList());
    }

    public Task Add(Position newRecord)
    {
        _positions.TryAdd(newRecord.Code, newRecord);
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        _positions.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task Update(Position newValue)
    {
        if (_positions.ContainsKey(newValue.Code))
        {
            _positions[newValue.Code] = newValue;
        }
        return Task.CompletedTask;
    }
}