using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Terrain.Objects;

public interface IPlaceableVertices
{
    public abstract VertexPositionColorTexture[] Vertices { get; }
    static abstract VertexPositionColorTexture[] SetTextures(VertexPositionColorTexture[] vertices);
}

/// <summary>
/// The base class for anything placeable.
/// </summary>
public abstract class Placeable
{
    public abstract VertexPositionColorTexture[] Vertices { get; }
    public abstract Coordinate Coordinate { get; set; }

    public VertexPositionColorTexture[] AddCoordinate(VertexPositionColorTexture[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].Position += (Vector3)Coordinate;
        }

        return vertices;
    }
}

public sealed class Stone : Placeable, IPlaceableVertices
{
    public override Coordinate Coordinate { get; set; }

    public override VertexPositionColorTexture[] Vertices
    {
        get
        {
            return AddCoordinate(SetTextures(VertexLayout.CubeLayout));
        }
    }

    public static VertexPositionColorTexture[] SetTextures(VertexPositionColorTexture[] vertices)
    {
        float textureWidth = 1;
        for (int i = 0; i < vertices.Length; i += 6)
        {
            vertices[i].TextureCoordinate = new Vector2(0, textureWidth);
            vertices[i + 1].TextureCoordinate = new Vector2(0, 0);
            vertices[i + 2].TextureCoordinate = new Vector2(textureWidth, textureWidth);
            
            vertices[i + 3].TextureCoordinate = new Vector2(textureWidth, textureWidth);
            vertices[i + 4].TextureCoordinate = new Vector2(0, 0);
            vertices[i + 5].TextureCoordinate = new Vector2(textureWidth, 0);
        }

        return vertices;
    }
}