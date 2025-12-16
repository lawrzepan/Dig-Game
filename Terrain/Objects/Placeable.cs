using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Terrain.Objects;

/// <summary>
/// The base class for anything placeable.
/// </summary>
public abstract class Placeable
{
    public Coordinate Coordinate { get; set; }
    
    public static VertexPositionColorTexture[] Vertices { get; }
}

public abstract class Stone : Placeable
{
    public static VertexPositionColorTexture[] Vertices { get; } = SetTextures(VertexLayout.CubeLayout);

    public static VertexPositionColorTexture[] SetTextures(VertexPositionColorTexture[] vertices)
    {
        float textureWidth = 32f / 1024f;
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