using Xunit;
using DepthCharts.Domain.Entities;
using DepthCharts.Domain.ValueObjects;
using DepthCharts.Domain.Interfaces;
using DepthCharts.Domain.Enums;
using DepthCharts.Application.Services;

namespace DepthCharts.Tests.Application;

public class DepthChartServiceTests
{
    private readonly DepthChartService _service;
    private readonly IDepthChart _chart;

    private static readonly Position _wr = new("WR"); // NFL - wide receiver
    private static readonly Position _kr = new("KR"); // NFL - kick returner
    private static readonly Position _qb = new("QB"); // not valid in MLB

    public DepthChartServiceTests()
    {
        _chart = new DepthChart();
        _service = new DepthChartService(_chart);
    }

    [Fact]
    public void AddPlayer_ToValidNFLPosition_ShouldSucceed()
    {
        // Arrange
        var player = new Player(1, "Bob", _wr);

        // Act
        _service.AddPlayer(player, _wr, SportType.NFL, 0);
        var result = _service.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 1 }, result["WR"]);
    }

    [Fact]
    public void AddPlayer_ToInvalidMLBPosition_ShouldThrow()
    {
        // Arrange
        var player = new Player(2, "Joe", _qb);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _service.AddPlayer(player, _qb, SportType.MLB, 0); // QB is not valid for MLB
        });

        Assert.Contains("not a valid", ex.Message);
    }

    [Fact]
    public void GetPlayersUnder_ShouldReturnCorrectResults()
    {
        // Arrange
        var bob = new Player(1, "Bob", _wr);
        var alice = new Player(2, "Alice", _wr);
        var charlie = new Player(3, "Charlie", _wr);

        _service.AddPlayer(bob, _wr, SportType.NFL, 0);
        _service.AddPlayer(alice, _wr, SportType.NFL, 0);
        _service.AddPlayer(charlie, _wr, SportType.NFL, 2);

        // Act
        var underAlice = _service.GetPlayersUnder(alice, _wr, SportType.NFL);

        // Assert
        Assert.Equal(new[] { 1, 3 }, underAlice.Select(p => p.PlayerId));
    }

    [Fact]
    public void RemovePlayer_ShouldRemoveFromCorrectPosition()
    {
        // Arrange
        var bob = new Player(1, "Bob", _wr);
        _service.AddPlayer(bob, _wr, SportType.NFL, 0);

        // Act
        _service.RemovePlayer(bob, _wr, SportType.NFL);
        var result = _service.GetFullDepthChart();

        // Assert
        Assert.DoesNotContain("WR", result.Keys);
    }
}
