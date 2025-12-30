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

    private List<Chunk> LoadedChunks = new List<Chunk>(50);
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
        LoadedChunks.Add(chunk);
        
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
        foreach (var chunk in LoadedChunks)
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
    
    public int? FindChunkIndexFromCoordinate(Coordinate coordinate, bool loadedChunksPermission)
    {
        if (!loadedChunksPermission)
        {
            WaitToUseLoadedChunks();
            loadedChunksInUse = true;
        }
        for (int i = 0; i < LoadedChunks.Count; i++)
        {
            if (LoadedChunks[i].Coordinate == coordinate)
            {
                if (!loadedChunksPermission) loadedChunksInUse = false;
                return i;
            }
        }

        if (!loadedChunksPermission) loadedChunksInUse = false;

        return null;
    }
    
    public bool UnloadChunk(Coordinate coordinate, bool loadedChunksPermission)
    {
        int? index = FindChunkIndexFromCoordinate(coordinate, loadedChunksPermission);
        
        if (!loadedChunksPermission)
        {
            WaitToUseLoadedChunks();
            loadedChunksInUse = true;
        }
        
        if (index.HasValue)
        {
            ChunkDrawer.UnloadChunk(LoadedChunks[index.Value]);
            
            LoadedChunks.RemoveAt(index.Value);

            if (!loadedChunksPermission) loadedChunksInUse = false;

            return true;
        }
        
        if (!loadedChunksPermission) loadedChunksInUse = false;
        return false;
    }

    public void LoadChunksInRadius(float radius, Coordinate centreChunk)
    {
        WaitToUseLoadedChunks();
        loadedChunksInUse = true;

        bool[] chunksInView = new bool[LoadedChunks.Count];
        for (int i = 0; i < chunksInView.Length; i++)
        {
            chunksInView[i] = false;
        }
        
        var chunksToLoad = new List<ChunkLoaderContext>();
        
        int ceilRadius = (int)MathF.Ceiling(radius);
        
        for (int xChunk = centreChunk.X - ceilRadius; xChunk <= centreChunk.X + ceilRadius; xChunk++)
        {
            for (int zChunk = centreChunk.Z - ceilRadius; zChunk <= centreChunk.Z + ceilRadius; zChunk++)
            {
                Coordinate chunkCoordinate = new Coordinate((int)Math.Floor(xChunk / (float)Chunk.Size) * Chunk.Size, 0, 
                    (int)Math.Floor(zChunk / (float)Chunk.Size) * Chunk.Size, 0);

                int? chunkIndex = FindChunkIndexFromCoordinate(chunkCoordinate, true);
                if (!chunkIndex.HasValue)
                {
                    bool found = false;
                    foreach (var chunk in chunksToLoad)
                    {
                        if (chunk.Coordinate == chunkCoordinate) found = true;
                    }

                    if (!found)
                    {
                        chunksToLoad.Add(new ChunkLoaderContext(chunkCoordinate, this));
                        Console.WriteLine($"loading chunk {chunkCoordinate}");
                    }
                }
                else
                {
                    chunksInView[chunkIndex.Value] = true;
                }
            }
        }
        for (int i = chunksInView.Length - 1; i >= 0; i--)
        {
            if (!chunksInView[i])
            {
                Console.WriteLine($"unload chunk {LoadedChunks[i].Coordinate}");
                UnloadChunk(LoadedChunks[i].Coordinate, true);
            }
        }
        
        loadedChunksInUse = false;
        
        foreach (var chunk in chunksToLoad)
        {
            Task.Run(chunk.Load);
        }
    }

    private void WaitToUseLoadedChunks()
    {
        while (loadedChunksInUse)
        {
            
        }
    }

    private class ChunkLoaderContext(Coordinate coordinate, ChunkManager chunkManager)
    {
        public ChunkManager ChunkManager { get; set; } = chunkManager;
        public Coordinate Coordinate { get; set; } = coordinate;

        public void Load()
        {
            ChunkManager.LoadChunk(Coordinate);
        }
    }
}