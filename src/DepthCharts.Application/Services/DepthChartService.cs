using DepthCharts.Domain.Entities;
using DepthCharts.Domain.Enums;
using DepthCharts.Domain.Interfaces;
using DepthCharts.Domain.ValueObjects;

namespace DepthCharts.Application.Services;

public class DepthChartService
{
    private readonly IDepthChart _depthChart;

    public DepthChartService(IDepthChart depthChart)
    {
        _depthChart = depthChart;
    }

    public void AddPlayer(Player player, Position position, SportType sport, int? depth = null)
    {
        ValidatePosition(position, sport);
        _depthChart.AddPlayer(player, position, depth);
    }

    public void RemovePlayer(Player player, Position position, SportType sport)
    {
        ValidatePosition(position, sport);
        _depthChart.RemovePlayer(player, position);
    }


    public IReadOnlyList<Player> GetPlayersUnder(Player player, Position position, SportType sport)
    {
        ValidatePosition(position, sport);
        return _depthChart.GetPlayersUnder(player, position);
    }

    public Dictionary<string, List<int>> GetFullDepthChart()
    {
        return _depthChart.GetFullDepthChart();
    }

    private static void ValidatePosition(Position position, SportType sport)
    {
        if (!PositionRegistry.IsValid(position.Name, sport))
            throw new ArgumentException($"'{position.Name}' is not a valid {sport} position.");
    }
}
