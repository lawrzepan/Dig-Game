using DigGame.Terrain.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Terrain.Objects;

/// <summary>
/// The base class for anything placeable.
/// </summary>
public abstract class Placeable
{
    public abstract VertexLayoutType VertexLayoutType { get; }
    public abstract Coordinate Coordinate { get; set; }
    
    public abstract CullDirection CullDirection { get; set; }
        
    public VertexPositionColorTexture[] AddCoordinate(VertexPositionColorTexture[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].Position += (Vector3)Coordinate;
        }

        return vertices;
    }
    
    public abstract VertexPositionColorTexture[] SetTextures(VertexPositionColorTexture[] vertices);
}

public sealed class Stone : Placeable
{
    public override VertexLayoutType VertexLayoutType { get; } = VertexLayoutType.Cube;
    
    public override Coordinate Coordinate { get; set; }

    public override CullDirection CullDirection { get; set; }

    public override VertexPositionColorTexture[] SetTextures(VertexPositionColorTexture[] vertices)
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