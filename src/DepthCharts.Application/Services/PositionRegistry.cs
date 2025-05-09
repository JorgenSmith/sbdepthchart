using DepthCharts.Domain.ValueObjects;
using DepthCharts.Domain.Enums;
namespace DepthCharts.Application.Services;



public static class PositionRegistry
{
    private static readonly Dictionary<SportType, HashSet<string>> Positions = new()
    {
        [SportType.NFL] = new() { "QB", "WR", "RB", "TE", "K", "P", "KR", "PR" },
        [SportType.MLB] = new() { "SP", "RP", "C", "1B", "2B", "3B", "SS", "LF", "RF", "CF", "DH" }
    };

    public static bool IsValid(string code, SportType sport) =>
        Positions.TryGetValue(sport, out var set) && set.Contains(code);

    public static Position Create(string code, SportType sport)
    {
        if (!IsValid(code, sport))
            throw new ArgumentException($"'{code}' is not a valid {sport} position.");

        return new Position(code);
    }
}
