using DigGame.Terrain.Chunking;
using DigGame.Terrain.Objects;
using DigGame.Terrain.Placeables;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Draw;

public class ChunkDrawer
{
    public static int MaxSize = 20000;

    private int vertexCount = 0;
    private VertexPositionColorTexture[] vertices;

    private VertexBuffer vertexBuffer;

    private GraphicsDevice graphicsDevice;
    
    public ChunkDrawer(GraphicsDevice graphicsDevice)
    {
        vertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, MaxSize,
            BufferUsage.WriteOnly);

        vertices = new VertexPositionColorTexture[MaxSize];

        this.graphicsDevice = graphicsDevice;
    }

    public void UploadChunk(Chunk chunk)
    {
        int startIndex = vertexCount;
        int endIndex;
        
        foreach (var placeable in chunk.Placeables)
        {
            var _vertices = GetVertexLayoutFromType(placeable.VertexLayoutType);
            _vertices = placeable.SetTextures(placeable.AddCoordinate(_vertices));
            
            for (int i = 0; i < _vertices.Length; i++)
            {
                if (vertexCount >= MaxSize) goto outsideLoop;

                if ((placeable.CullDirection & CullDirection.West) != CullDirection.None && i is >= 0 and <= 5)
                {
                    i = 5;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.South) != CullDirection.None && i is >= 6 and <= 11)
                {
                    i = 11;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.East) != CullDirection.None && i is >= 12 and <= 17)
                {
                    i = 17;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.North) != CullDirection.None && i is >= 18 and <= 23)
                {
                    i = 23;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.High) != CullDirection.None && i is >= 24 and <= 29)
                {
                    i = 29;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.Low) != CullDirection.None && i is >= 30 and <= 35)
                {
                    i = 35;
                    continue;
                }
                
                vertices[vertexCount++] = _vertices[i];
            }
        }
        
        outsideLoop:

        endIndex = vertexCount > 0 ? vertexCount : 0;
        
        vertexBuffer.SetData(
            vertices,
            startIndex,
            endIndex - startIndex
        );
    }

    public VertexPositionColorTexture[] GetVertexLayoutFromType(VertexLayoutType vertexLayoutType)
    {
        switch (vertexLayoutType)
        {
            case VertexLayoutType.None:
                return [];
            case VertexLayoutType.Cube:
                return VertexLayout.CubeLayout;
            default:
                return [];
        }
    }
    
    public void DrawUploadedChunks(BasicEffect effect)
    {
        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexCount / 3);
        }
    }
}