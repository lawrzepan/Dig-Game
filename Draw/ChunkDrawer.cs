using System;
using System.Collections.Generic;
using DigGame.Terrain.Chunking;
using DigGame.Terrain.Objects;
using DigGame.Terrain.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigGame.Draw;

public class ChunkDrawer
{
    public static int MaxSize = 200000;

    private int vertexCount = 0;
    private VertexPositionColorTexture[] vertices;

    private VertexBuffer vertexBuffer;

    private GraphicsDevice graphicsDevice;

    private VertexPager vertexPager;
    
    public ChunkDrawer(GraphicsDevice graphicsDevice)
    {
        vertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, MaxSize,
            BufferUsage.WriteOnly);

        vertices = new VertexPositionColorTexture[MaxSize];

        this.graphicsDevice = graphicsDevice;

        vertexPager = new VertexPager(MaxSize);
    }

    public void LoadChunk(Chunk chunk)
    {
        List<VertexPositionColorTexture[]> VertexArrayList = new List<VertexPositionColorTexture[]>(500);
        
        Vector3 nullVector = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        
        int numVertices = 0;
        foreach (var placeable in chunk.Placeables)
        {
            VertexArrayList.Add(placeable.SetTextures(placeable.AddCoordinate(GetVertexLayoutFromType(placeable.VertexLayoutType))));

            var array = VertexArrayList[^1];
            for (int i = 0; i < array.Length; i++)
            {
                if ((placeable.CullDirection & CullDirection.West) != CullDirection.None && i is >= 0 and <= 5)
                {
                    // set vectors to negative infinity to act as a null vector that is to be skipped in the next part
                    array[0].Position = nullVector;
                    array[1].Position = nullVector;
                    array[2].Position = nullVector;
                    array[3].Position = nullVector;
                    array[4].Position = nullVector;
                    array[5].Position = nullVector;
                    i = 5;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.South) != CullDirection.None && i is >= 6 and <= 11)
                {
                    // set vectors to negative infinity to act as a null vector that is to be skipped in the next part
                    array[6].Position = nullVector;
                    array[7].Position = nullVector;
                    array[8].Position = nullVector;
                    array[9].Position = nullVector;
                    array[10].Position = nullVector;
                    array[11].Position = nullVector;
                    i = 11;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.East) != CullDirection.None && i is >= 12 and <= 17)
                {
                    // set vectors to negative infinity to act as a null vector that is to be skipped in the next part
                    array[12].Position = nullVector;
                    array[13].Position = nullVector;
                    array[14].Position = nullVector;
                    array[15].Position = nullVector;
                    array[16].Position = nullVector;
                    array[17].Position = nullVector;
                    i = 17;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.North) != CullDirection.None && i is >= 18 and <= 23)
                {
                    // set vectors to negative infinity to act as a null vector that is to be skipped in the next part
                    array[18].Position = nullVector;
                    array[19].Position = nullVector;
                    array[20].Position = nullVector;
                    array[21].Position = nullVector;
                    array[22].Position = nullVector;
                    array[23].Position = nullVector;
                    i = 23;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.High) != CullDirection.None && i is >= 24 and <= 29)
                {
                    // set vectors to negative infinity to act as a null vector that is to be skipped in the next part
                    array[24].Position = nullVector;
                    array[25].Position = nullVector;
                    array[26].Position = nullVector;
                    array[27].Position = nullVector;
                    array[28].Position = nullVector;
                    array[29].Position = nullVector;
                    i = 29;
                    continue;
                }
                if ((placeable.CullDirection & CullDirection.Low) != CullDirection.None && i is >= 30 and <= 35)
                {
                    // set vectors to negative infinity to act as a null vector that is to be skipped in the next part
                    array[30].Position = nullVector;
                    array[31].Position = nullVector;
                    array[32].Position = nullVector;
                    array[33].Position = nullVector;
                    array[34].Position = nullVector;
                    array[35].Position = nullVector;
                    i = 35;
                    continue;
                }

                numVertices++;
            }
        }

        vertexCount += numVertices;
        
        Range? availableRange = vertexPager.FindNextAvailableBlock(numVertices);
        
        if (availableRange == null) return;

        chunk.AllocatedRange = availableRange.Value;
        
        vertexPager.AllocateRange(availableRange.Value, true);

        int vertex = availableRange.Value.Start;
        foreach (var array in VertexArrayList)
        {
            for (int v = 0; v < array.Length; v++)
            {
                if (array[v].Position != nullVector)
                {
                    vertices[vertex++] = array[v];
                }
            }
        }

        vertexBuffer.SetData(
            availableRange.Value.Start * 24, // 24 is the size of VertexPositionColorTexture in bytes, do not edit
            vertices,
            availableRange.Value.Start,
            numVertices,
            0
        );
    }

    public void UnloadChunk(Chunk chunk)
    {
        var range = chunk.AllocatedRange;
        range.RangeType = RangeType.Empty;

        var i = vertexPager.GetFromRange(chunk.AllocatedRange);
        if (i.HasValue)
        {
            vertexPager.Ranges[i.Value] = range;
        }
        vertexPager.MergeEmptyBlocks();

        for (int v = range.Start; v <= range.End; v++)
        {
            vertices[v] = new VertexPositionColorTexture(Vector3.Zero, Color.Black, Vector2.Zero);
        }
        
        vertexBuffer.SetData(
            range.Start * 24, // 24 is the size of VertexPositionColorTexture in bytes, do not edit
            vertices,
            range.Start,
            range.Length,
            0
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