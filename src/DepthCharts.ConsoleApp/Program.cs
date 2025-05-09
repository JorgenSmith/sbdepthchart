using DepthCharts.Domain.Entities;
using DepthCharts.Domain.Interfaces;
using DepthCharts.Domain.ValueObjects;
using DepthCharts.Domain.Enums;
using DepthCharts.Application.Services;

IDepthChart chart = new DepthChart();
var service = new DepthChartService(chart);

Position _wr = new("WR"); // NFL - Wide Receiver
Position _kr = new("KR"); // NFL - Kick Returner

// Create players
var bob = new Player(1, "Bob", _wr);
var alice = new Player(2, "Alice", _wr);
var charlie = new Player(3, "Charlie", _wr);

// Add players to WR depth chart (with depth control)
service.AddPlayer(bob, _wr, SportType.NFL, 0);       // Bob at depth 0
service.AddPlayer(alice, _wr, SportType.NFL, 0);     // Alice inserted at 0, Bob pushed to 1
service.AddPlayer(charlie, _wr, SportType.NFL, 2);   // Charlie added at depth 2
service.AddPlayer(bob, _kr, SportType.NFL);          // Bob also added to KR position (end)

// Display full depth chart
Console.WriteLine("Full Depth Chart:");
var chartData = service.GetFullDepthChart();
foreach (var position in chartData)
{
    Console.WriteLine($"{position.Key}: [{string.Join(", ", position.Value)}]");
}

// Show all players below Alice in the WR chart
Console.WriteLine("\nPlayers under Alice at WR:");
var playersUnder = service.GetPlayersUnder(alice, _wr, SportType.NFL);
foreach (var p in playersUnder)
{
    Console.WriteLine($" - {p.Name} (ID: {p.PlayerId})");
}

// Remove Bob from WR and show updated chart
Console.WriteLine("\nRemoving Bob from WR...");
service.RemovePlayer(bob, _wr, SportType.NFL);

Console.WriteLine("Updated Depth Chart after removal:");
chartData = service.GetFullDepthChart();
foreach (var position in chartData)
{
    Console.WriteLine($"{position.Key}: [{string.Join(", ", position.Value)}]");
}

// Demonstrate MLB positions
Console.WriteLine("\nMLB Depth Chart Example:");
var pitcher = new Player(10, "Pitcher Pete", new Position("SP")); // Starting Pitcher
var catcher = new Player(11, "Catcher Carla", new Position("C")); // Catcher

service.AddPlayer(pitcher, new Position("SP"), SportType.MLB, 0);
service.AddPlayer(catcher, new Position("C"), SportType.MLB, 0);

var mlbChart = service.GetFullDepthChart();
foreach (var position in mlbChart.Where(p => p.Key == "SP" || p.Key == "C"))
{
    Console.WriteLine($"{position.Key}: [{string.Join(", ", position.Value)}]");
}

// Attempt to add an invalid position for MLB (should fail)
Console.WriteLine("\nAttempting to add 'WR' position to MLB (should fail):");
try
{
    var outOfPlace = new Player(12, "WrongSport Wendy", new Position("WR"));
    service.AddPlayer(outOfPlace, new Position("WR"), SportType.MLB, 0);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught expected error: {ex.Message}");
}
