using DepthCharts.Domain.ValueObjects;
using DepthCharts.Domain.Interfaces;

namespace DepthCharts.Domain.Entities;

public class DepthChart : IDepthChart
{
    private readonly Dictionary<Position, List<Player>> _chart = new();

    public IReadOnlyDictionary<Position, IReadOnlyList<Player>> Chart =>
        _chart.ToDictionary(kvp => kvp.Key, kvp => (IReadOnlyList<Player>)kvp.Value.AsReadOnly());

    public void AddPlayer(Player player, Position position, int? depth = null)
    {
        if (!_chart.ContainsKey(position))
            _chart[position] = new List<Player>();

        var list = _chart[position];

        // Remove existing occurrence first
        list.RemoveAll(p => p.PlayerId == player.PlayerId);

        MovePlayerToDepth(list, player, depth);
    }

    private static void MovePlayerToDepth(List<Player> list, Player player, int? depth)
    {
        int insertAt = depth.HasValue ? Math.Clamp(depth.Value, 0, list.Count) : list.Count;
        list.Insert(insertAt, player);
    }


    public void RemovePlayer(Player player, Position position)
    {
        if (_chart.TryGetValue(position, out var list))
        {
            list.RemoveAll(p => p.PlayerId == player.PlayerId);
            if (list.Count == 0)
                _chart.Remove(position);
        }
    }

    public IReadOnlyList<Player> GetPlayersUnder(Player player, Position position)
    {
        if (!_chart.TryGetValue(position, out var list))
            return new List<Player>();

        int index = list.FindIndex(p => p.PlayerId == player.PlayerId);
        return index == -1 ? new List<Player>() : list.Skip(index + 1).ToList();
    }

    public Dictionary<string, List<int>> GetFullDepthChart()
    {
        return _chart.ToDictionary(
            kvp => kvp.Key.Name,
            kvp => kvp.Value.Select(p => p.PlayerId).ToList()
        );
    }
}
