using DepthCharts.Domain.ValueObjects;

namespace DepthCharts.Domain.Entities;

public record class Player(int PlayerId, string Name, Position PrimaryPosition);
