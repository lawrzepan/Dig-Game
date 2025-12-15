#nullable enable
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Terrain.Objects;

/// <summary>
/// An interface used as the base class for all vertex layouts for objects
/// </summary>
/// <typeparam name="TSelf">The derived class type.</typeparam>
public interface IVertexLayout<TSelf>
    where TSelf : IVertexLayout<TSelf>
{
    /// <summary>
    /// Generates the vertex layout for any object.
    /// </summary>
    /// <param name="origin">The origin of the vertices, for most blocks this is the low south-west corner.</param>
    /// <returns></returns>
    static abstract VertexPositionColorTexture[]? Generate(Vector3 origin);
}

/// <summary>
/// Used for objects which should not be drawn, e.g. air
/// </summary>
public abstract class NoneLayout : IVertexLayout<NoneLayout>
{
    public static VertexPositionColorTexture[]? Generate(Vector3 origin)
    {
        return null;
    }
}

/// <summary>
/// A layout containing the vertices of a cube, this is the usual layout for all blocks.
/// </summary>
public abstract class CubeLayout : IVertexLayout<CubeLayout>
{
    public static VertexPositionColorTexture[]? Generate(Vector3 origin)
    {
        return new[]
        {
            new VertexPositionColorTexture(new Vector3(0, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 0, 1) + origin, Color.Black, Vector2.Zero),

            new VertexPositionColorTexture(new Vector3(0, 0, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 1) + origin, Color.Black, Vector2.Zero),


            new VertexPositionColorTexture(new Vector3(1, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 0, 0) + origin, Color.Black, Vector2.Zero),

            new VertexPositionColorTexture(new Vector3(0, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 0) + origin, Color.Black, Vector2.Zero),


            new VertexPositionColorTexture(new Vector3(1, 0, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 0, 0) + origin, Color.Black, Vector2.Zero),

            new VertexPositionColorTexture(new Vector3(1, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 0) + origin, Color.Black, Vector2.Zero),


            new VertexPositionColorTexture(new Vector3(0, 0, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 0, 1) + origin, Color.Black, Vector2.Zero),

            new VertexPositionColorTexture(new Vector3(1, 0, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 1) + origin, Color.Black, Vector2.Zero),


            new VertexPositionColorTexture(new Vector3(0, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 1, 1) + origin, Color.Black, Vector2.Zero),

            new VertexPositionColorTexture(new Vector3(0, 1, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 1, 1) + origin, Color.Black, Vector2.Zero),


            new VertexPositionColorTexture(new Vector3(1, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(1, 0, 1) + origin, Color.Black, Vector2.Zero),

            new VertexPositionColorTexture(new Vector3(1, 0, 1) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 0, 0) + origin, Color.Black, Vector2.Zero),
            new VertexPositionColorTexture(new Vector3(0, 0, 1) + origin, Color.Black, Vector2.Zero),


        };
    }
}