using Microsoft.Xna.Framework;

namespace DigGame.Terrain.Objects;

public struct Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int Layer { get; set; }

    public static explicit operator Vector3(Coordinate coordinate) =>
        new Vector3(coordinate.X, coordinate.Y, coordinate.Z);
}