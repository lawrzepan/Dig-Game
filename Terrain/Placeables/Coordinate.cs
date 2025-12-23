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

    /// <summary>
    /// Returns a new coordinate with X, Y, Z components of the first coordinate added onto the X, Y, Z components of
    /// the second coordinate. The layer component is taken directly from the first coordinate.
    /// </summary>
    /// <param name="coordinate1">The first coordinate to be added.</param>
    /// <param name="coordinate2">The second coordinate to be added.</param>
    /// <returns>A new coordinate using the result of addition.</returns>
    public static Coordinate operator +(Coordinate coordinate1, Coordinate coordinate2)
    {
        return new Coordinate(coordinate1.X + coordinate2.X, coordinate1.Y + coordinate2.Y,
            coordinate1.Z + coordinate2.Z, coordinate1.Layer);
    }

    public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2)
    {
        return coordinate1.X == coordinate2.X &&
               coordinate1.Y == coordinate2.Y &&
               coordinate1.Z == coordinate2.Z &&
               coordinate1.Layer == coordinate2.Layer;
    }
    
    public static bool operator !=(Coordinate coordinate1, Coordinate coordinate2)
    {
        return !(coordinate1 == coordinate2);
    }

    public Coordinate(int x, int y, int z, int layer)
    {
        X = x;
        Y = y;
        Z = z;
        Layer = layer;
    }
}