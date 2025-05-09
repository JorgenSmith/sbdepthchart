using DepthCharts.Domain.Entities;
using DepthCharts.Domain.ValueObjects;

namespace DepthCharts.Domain.Interfaces
{
    public interface IDepthChart
    {
        void AddPlayer(Player player, Position position, int? depth = null);
        void RemovePlayer(Player player, Position position);
        IReadOnlyList<Player> GetPlayersUnder(Player player, Position position);
        Dictionary<string, List<int>> GetFullDepthChart();
    }
}