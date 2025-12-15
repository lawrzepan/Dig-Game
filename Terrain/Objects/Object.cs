using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Terrain.Objects;

public struct textureArea(Vector2 corner1, Vector2 corner2)
{
    public Vector2 Corner1 { get; set; } = corner1;
    public Vector2 Corner2 { get; set; } = corner2;
}

public abstract class Object
{
    public Coordinate Coordinate { get; set; }
    
    public VertexLayout VertexLayout { get; }

    public abstract textureArea TextureArea { get; }
}

public abstract class Stone : Object
{
    public override VertexLayout VertexLayout { get; } = CubeLayout;
    public override textureArea TextureArea { get; } = new textureArea(new Vector2(0, 0), new Vector2(32f / 1024f, 32f / 1024f));
}