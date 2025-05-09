using Xunit;
using DepthCharts.Domain.Entities;
using DepthCharts.Domain.ValueObjects;
using System.Linq;

namespace DepthCharts.Tests.Domain;

public class DepthChartTests
{
    private static readonly Position _wr = new("WR"); // NFL - wide receiver
    private static readonly Position _kr = new("KR"); // NFL - kick returner

    private readonly Player _bob = new(1, "Bob", _wr);
    private readonly Player _alice = new(2, "Alice", _wr);
    private readonly Player _charlie = new(3, "Charlie", _wr);

    [Fact]
    public void AddPlayer_ShouldAddToEnd_WhenDepthNotSpecified()
    {
        // Arrange
        var chart = new DepthChart();

        // Act
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_charlie, _wr);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 1, 3 }, result["WR"]);
    }

    [Fact]
    public void AddPlayer_ShouldInsertAtSpecificDepth_AndPushOthersDown()
    {
        // Arrange
        var chart = new DepthChart();

        // Act
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_charlie, _wr);
        chart.AddPlayer(_alice, _wr, 0);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 2, 1, 3 }, result["WR"]);
    }

    [Fact]
    public void RemovePlayer_ShouldRemoveCorrectPlayer()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_alice, _wr);

        // Act
        chart.RemovePlayer(_bob, _wr);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Single(result["WR"]);
        Assert.Equal(2, result["WR"].First());
    }

    [Fact]
    public void GetPlayersUnder_ShouldReturnCorrectSubset()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_alice, _wr, 0);
        chart.AddPlayer(_charlie, _wr, 2);

        // Act
        var underAlice = chart.GetPlayersUnder(_alice, _wr);

        // Assert
        Assert.Equal(new[] { 1, 3 }, underAlice.Select(p => p.PlayerId));
    }

    [Fact]
    public void AddSamePlayerTwice_ShouldMoveAndNotDuplicate()
    {
        // Arrange
        var chart = new DepthChart();

        // Act
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_bob, _wr, 0);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Single(result["WR"]);
        Assert.Equal(1, result["WR"][0]);
    }

    [Fact]
    public void AddPlayerToMultiplePositions_ShouldTrackSeparately()
    {
        // Arrange
        var chart = new DepthChart();

        // Act
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_bob, _kr);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 1 }, result["WR"]);
        Assert.Equal(new[] { 1 }, result["KR"]);
    }

    [Fact]
    public void RemovePlayer_NotInChart_ShouldDoNothing()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr);
        var dave = new Player(99, "Dave", _wr);

        // Act
        chart.RemovePlayer(dave, _wr);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Single(result["WR"]);
        Assert.Equal(1, result["WR"].First());
    }

    [Fact]
    public void GetPlayersUnder_NonExistentPlayer_ShouldReturnEmpty()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_alice, _wr);
        var ghost = new Player(999, "Ghost", _wr);

        // Act
        var result = chart.GetPlayersUnder(ghost, _wr);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetFullDepthChart_EmptyPosition_ShouldNotIncludeInResult()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr);
        chart.RemovePlayer(_bob, _wr);

        // Act
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.DoesNotContain("WR", result.Keys);
    }

    [Fact]
    public void AddPlayer_AtOutOfBoundsDepth_ShouldAppend()
    {
        // Arrange
        var chart = new DepthChart();

        // Act
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_alice, _wr, 5);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 1, 2 }, result["WR"]);
    }

    [Fact]
    public void AddPlayer_AlreadyPresent_ShouldMovePosition()
    {
        // Arrange
        var chart = new DepthChart();

        // Act
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_alice, _wr);
        chart.AddPlayer(_bob, _wr, 1);
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 2, 1 }, result["WR"]);
    }

    [Fact]
    public void AddPlayer_ToMLBPosition_ShouldBeTracked()
    {
        // Arrange
        var chart = new DepthChart();
        var pitcher = new Player(4, "Pitcher Pete", new Position("SP"));
        var catcher = new Player(5, "Catcher Carla", new Position("C"));

        // Act
        chart.AddPlayer(pitcher, new Position("SP"));
        chart.AddPlayer(catcher, new Position("C"));
        var result = chart.GetFullDepthChart();

        // Assert
        Assert.Equal(new[] { 4 }, result["SP"]);
        Assert.Equal(new[] { 5 }, result["C"]);
    }
    [Fact]
    public void GetPlayersUnderPlayerInDepthChart_ShouldReturnAllPlayersBelow()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr, 0);
        chart.AddPlayer(_alice, _wr, 0);      // Bumps Bob
        chart.AddPlayer(_charlie, _wr, 2);

        // Act
        var underAlice = chart.GetPlayersUnder(_alice, _wr);

        // Assert
        Assert.Equal(new[] { 1, 3 }, underAlice.Select(p => p.PlayerId));
    }

    [Fact]
    public void GetPlayersUnderPlayerInDepthChart_WhenPlayerNotInChart_ShouldReturnEmpty()
    {
        // Arrange
        var chart = new DepthChart();
        chart.AddPlayer(_bob, _wr);
        chart.AddPlayer(_alice, _wr);
        var ghost = new Player(999, "Ghost", _wr);

        // Act
        var result = chart.GetPlayersUnder(ghost, _wr);

        // Assert
        Assert.Empty(result);
    }

}
