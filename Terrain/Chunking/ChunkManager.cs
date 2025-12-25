using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DigGame.Draw;
using DigGame.Terrain.Objects;

namespace DigGame.Terrain.Chunking;


public class ChunkManager
{
    public Vector3 Centre = Vector3.Zero;

    public List<(Chunk, bool)> LoadedChunks = new List<(Chunk, bool)>(50);
    private bool loadedChunksInUse = false;

    public ChunkDrawer ChunkDrawer;

    private Coordinate latestCoordinate = new Coordinate(0, 0, 0, 0);
    
    public ChunkManager(ChunkDrawer chunkDrawer)
    {
        ChunkDrawer = chunkDrawer;
    }
    public void LoadChunk(Coordinate coordinate)
    {
        var chunk = new Chunk(coordinate);
        
        WaitToUseLoadedChunks();
        loadedChunksInUse = true;
        LoadedChunks.Add((chunk, false));
        
        loadedChunksInUse = false;
        
        ChunkDrawer.LoadChunk(chunk);
    }

    public void LoadChunk()
    {
        LoadChunk(latestCoordinate);
    }
    
    public Chunk? FindChunkFromCoordinate(Coordinate coordinate)
    {
        WaitToUseLoadedChunks();
        loadedChunksInUse = true;
        foreach (var (chunk, _) in LoadedChunks)
        {
            if (chunk.Coordinate == coordinate)
            {
                loadedChunksInUse = false;
                return chunk;
            }
        }
        loadedChunksInUse = false;

        return null;
    }
    
    public int? FindChunkIndexFromCoordinate(Coordinate coordinate)
    {
        WaitToUseLoadedChunks();
        loadedChunksInUse = true;
        for (int i = 0; i < LoadedChunks.Count; i++)
        {
            if (LoadedChunks[i].Item1.Coordinate == coordinate)
            {
                loadedChunksInUse = false;
                return i;
            }
        }
        loadedChunksInUse = false;

        return null;
    }
    
    
    // when using, make sure you are allowed to use LoadedChunks!!! (threading)
    public bool UnloadChunk(Coordinate coordinate)
    {
        int? index = FindChunkIndexFromCoordinate(coordinate);

        if (index != null)
        {
            ChunkDrawer.UnloadChunk(LoadedChunks[index.Value].Item1);
            
            WaitToUseLoadedChunks();
            loadedChunksInUse = true;
            LoadedChunks.RemoveAt(index.Value);
            loadedChunksInUse = false;

            return true;
        }
        
        return false;
    }

    public void LoadChunksInRadius(float radius, Coordinate centreChunk)
    {
        for (int i = 0; i < LoadedChunks.Count; i++)
        {
            LoadedChunks[i] = (LoadedChunks[i].Item1, false);
        }
        
        int ceilRadius = (int)MathF.Ceiling(radius);

        Coordinate roundedCentreChunk = new Coordinate((int)Math.Floor(centreChunk.X / (float)Chunk.Size) * Chunk.Size, 0,
            (int)Math.Floor(centreChunk.Z / (float)Chunk.Size) * Chunk.Size, 0);
        
        for (int xChunk = roundedCentreChunk.X - ceilRadius; xChunk <= centreChunk.X + ceilRadius; xChunk++)
        {
            for (int zChunk = roundedCentreChunk.Z - ceilRadius; zChunk <= centreChunk.Z + ceilRadius; zChunk++)
            {
                Coordinate chunkCoordinate = new Coordinate((int)Math.Floor(xChunk / (float)Chunk.Size) * Chunk.Size, 0, 
                    (int)Math.Floor(zChunk / (float)Chunk.Size) * Chunk.Size, 0) + roundedCentreChunk;

                int? chunkIndex = FindChunkIndexFromCoordinate(chunkCoordinate);
                if (chunkIndex == null)
                {
                    latestCoordinate = chunkCoordinate;
                    
                    //Task.Run(LoadChunk);
                    LoadChunk();
                }
                else
                {
                    LoadedChunks[chunkIndex.Value] = (LoadedChunks[chunkIndex.Value].Item1, true);
                }
            }
        }

        for (int i = 0; i < LoadedChunks.Count; i++)
        {
            if (!LoadedChunks[i].Item2)
            {
                UnloadChunk(LoadedChunks[i].Item1.Coordinate);
            }
        }
    }

    private void WaitToUseLoadedChunks()
    {
        while (loadedChunksInUse)
        {
            
        }
    }
}