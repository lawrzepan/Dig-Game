using DigGame.Terrain.Chunking;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Draw;

public class ChunkDrawer
{
    public static int MaxSize = 10000;

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
            var _vertices = placeable.Vertices;

            for (int i = 0; i < _vertices.Length; i++)
            {
                if (vertexCount >= MaxSize) goto outsideLoop;
                
                vertices[vertexCount++] = _vertices[i];
            }
        }
        
        outsideLoop:

        endIndex = vertexCount > 0 ? vertexCount - 1 : 0;
        
        vertexBuffer.SetData(
            vertices,
            startIndex,
            endIndex - startIndex
        );
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