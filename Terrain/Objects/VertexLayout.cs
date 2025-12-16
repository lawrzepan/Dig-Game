#nullable enable
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Terrain.Objects;

public static partial class VertexLayout
{
    public static VertexPositionColorTexture[] CubeLayout
    {
        get
        {
            return
            [
                new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 0, 1), Color.Black, Vector2.Zero),

                new VertexPositionColorTexture(new Vector3(0, 0, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 1), Color.Black, Vector2.Zero),


                new VertexPositionColorTexture(new Vector3(1, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.Black, Vector2.Zero),

                new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 0), Color.Black, Vector2.Zero),


                new VertexPositionColorTexture(new Vector3(1, 0, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 0, 0), Color.Black, Vector2.Zero),

                new VertexPositionColorTexture(new Vector3(1, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.Black, Vector2.Zero),


                new VertexPositionColorTexture(new Vector3(0, 0, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 0, 1), Color.Black, Vector2.Zero),

                new VertexPositionColorTexture(new Vector3(1, 0, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 1), Color.Black, Vector2.Zero),


                new VertexPositionColorTexture(new Vector3(0, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 1, 1), Color.Black, Vector2.Zero),

                new VertexPositionColorTexture(new Vector3(0, 1, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 1, 1), Color.Black, Vector2.Zero),


                new VertexPositionColorTexture(new Vector3(1, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(1, 0, 1), Color.Black, Vector2.Zero),

                new VertexPositionColorTexture(new Vector3(1, 0, 1), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.Black, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(0, 0, 1), Color.Black, Vector2.Zero)
            ];
        }
    }
}