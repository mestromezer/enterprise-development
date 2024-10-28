using System.Collections.Concurrent;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.EntityFramework.Repositories.Mocks;

public class PositionRepositoryMock : IRepository<Position, int>
{
    private static readonly ConcurrentDictionary<int, Position> Positions = new();

    public Task<List<Position>> GetAsList()
    {
        return Task.FromResult(Positions.Values.ToList());
    }

    public Task<Position?> GetByKey(int key)
    {
        Positions.TryGetValue(key, out var position);
        return Task.FromResult(position);
    }

    public Task Add(Position newRecord)
    {
        var added = Positions.TryAdd(newRecord.Code, newRecord);
        if (!added)
        {
            throw new InvalidOperationException($"A position with code {newRecord.Code} already exists.");
        }
        return Task.CompletedTask;
    }

    public Task Delete(int key)
    {
        var removed = Positions.TryRemove(key, out _);
        if (!removed)
        {
            throw new KeyNotFoundException($"No position found with code {key}.");
        }
        return Task.CompletedTask;
    }

    public Task Update(int key, Position newValue)
    {
        if (!Positions.ContainsKey(key))
        {
            throw new KeyNotFoundException($"No position found with code {key}.");
        }

        newValue.Code = key;
        Positions[key] = newValue;
        return Task.CompletedTask;
    }
}