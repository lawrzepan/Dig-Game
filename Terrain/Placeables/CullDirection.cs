using System;

namespace DigGame.Terrain.Placeables;

/// <summary>
/// Used to define how a placeable's vertices should be culled.
/// </summary>
[Flags]
public enum CullDirection
{
    None = 0,       // 0b0
    North = 1,      // 0b1
    East = 1 << 1,  // 0b10
    South = 1 << 2, // 0b100
    West = 1 << 3,  // 0b1000
    High = 1 << 4,  // 0b10000
    Low = 1 << 5,   // 0b100000
}