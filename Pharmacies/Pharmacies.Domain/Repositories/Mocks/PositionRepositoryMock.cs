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

    public Task<Position?> GetByKey(int key)
    {
        _positions.TryGetValue(key, out var position);
        return Task.FromResult(position);
    }

    public Task Add(Position newRecord)
    {
        var added = _positions.TryAdd(newRecord.Code, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A position with code {newRecord.Code} already exists.");
        }
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = _positions.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No position found with code {key}.");
        }
        return Task.CompletedTask;
    }

    public Task Update(int key, Position newValue)
    {
        if (!_positions.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No position found with code {key}.");
        }

        _positions[key] = newValue;
        return Task.CompletedTask;
    }
}