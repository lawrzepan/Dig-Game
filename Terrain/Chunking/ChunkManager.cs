using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using DigGame.Draw;
using DigGame.Terrain.Objects;

namespace DigGame.Terrain.Chunking;


public class ChunkManager
{
    public Vector3 Centre = Vector3.Zero;

    public List<Chunk> LoadedChunks = new List<Chunk>(50);

    public ChunkDrawer ChunkDrawer;
    
    public ChunkManager(ChunkDrawer chunkDrawer)
    {
        ChunkDrawer = chunkDrawer;
    }
    public void LoadChunk(Coordinate coordinate)
    {
        var chunk = new Chunk(coordinate);
        LoadedChunks.Add(chunk);
        ChunkDrawer.LoadChunk(chunk);
    }
    
    public Chunk? FindChunkFromCoordinate(Coordinate coordinate)
    {
        foreach (var chunk in LoadedChunks)
        {
            if (chunk.Coordinate == coordinate) return chunk;
        }

        return null;
    }
    
    public bool UnloadChunk(Coordinate coordinate)
    {
        var chunk = FindChunkFromCoordinate(coordinate);

        if (chunk != null)
        {
            ChunkDrawer.UnloadChunk(chunk);
            LoadedChunks.Remove(chunk);

            return true;
        }
        else return false;
    }
}