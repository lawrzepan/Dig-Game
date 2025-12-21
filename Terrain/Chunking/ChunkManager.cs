using System.Collections.Generic;
using System.Numerics;
using DigGame.Draw;
using DigGame.Terrain.Objects;

namespace DigGame.Terrain.Chunking;

public class ChunkManager
{
    public Vector3 Centre = Vector3.Zero;

    public List<Chunk> LoadedChunks = new List<Chunk>(10);

    public ChunkDrawer ChunkDrawer;
    
    public ChunkManager(ChunkDrawer chunkDrawer)
    {
        ChunkDrawer = chunkDrawer;
    }
    public void LoadChunk(Coordinate coordinate)
    {
        var chunk = new Chunk(coordinate);
        LoadedChunks.Add(chunk);
        ChunkDrawer.UploadChunk(chunk);
    }
}