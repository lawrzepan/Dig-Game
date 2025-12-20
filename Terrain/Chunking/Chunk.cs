using System;
using System.Collections.Generic;
using DigGame.Terrain.Objects;
using DigGame.Terrain.Tile;
using DigGame.Terrain.Objects;
using DigGame.Terrain.Placeables;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Noise;

namespace DigGame.Terrain.Chunking;

public struct ChunkData()
{
    private Int16[] data = new Int16[Chunk.Size * Chunk.Size * Chunk.Size];

    public int Length
    {
        get => data.Length;
    }
    
    public Int16 this[int x, int y, int z]
    {
        get => data[x + y * Chunk.Size + z * Chunk.Size * Chunk.Size];
        set
        {
            data[x + y * Chunk.Size + z * Chunk.Size * Chunk.Size] = value;
        }
    }
}

public class Chunk
{
    /// <summary>
    /// The square width of every chunk, in tiles.
    /// </summary>
    public const int Size = 8;
    
    public Coordinate Coordinate { get; private set; }

    private ChunkData data = new ChunkData();

    private bool placeablesInitialised = false;
    private List<Placeable> _placeables = new List<Placeable>(127);

    public List<Placeable> Placeables
    {
        get => placeablesInitialised ? _placeables : AsPlaceables();
    }

    private List<Placeable> AsPlaceables()
    {
        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                for (int z = 0; z < Chunk.Size; z++)
                {
                    switch (data[x, y, z])
                    {
                        case PlaceableType.Stone:
                            var cullDirection = CullDirection.None;

                            if (x > 0 && PlaceableType.IsBlock(data[x - 1, y, z])) cullDirection |= CullDirection.West;
                            if (x < Chunk.Size - 1 && PlaceableType.IsBlock(data[x + 1, y, z])) cullDirection |= CullDirection.East;
                            if (y > 0 && PlaceableType.IsBlock(data[x, y - 1, z])) cullDirection |= CullDirection.Low;
                            if (y < Chunk.Size - 1 && PlaceableType.IsBlock(data[x, y + 1, z])) cullDirection |= CullDirection.High;
                            if (z > 0 && PlaceableType.IsBlock(data[x, y, z - 1])) cullDirection |= CullDirection.South;
                            if (z < Chunk.Size - 1 && PlaceableType.IsBlock(data[x, y, z + 1])) cullDirection |= CullDirection.North;

                            _placeables.Add(new Stone()
                            {
                                Coordinate = new Coordinate(x, y, z, 0),
                                CullDirection = cullDirection
                            });

                            break;
                    }
                }
            }
        }

        return _placeables;
    }

    public Chunk()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int z = 0; z < Size; z++)
                {
                    data[x, y, z] = Noise.Perlin((Coordinate.X + x) * 0.1, (Coordinate.Z + z) * 0.1) * 4 + 4 < y
                            ? PlaceableType.Air
                            : PlaceableType.Stone;
                }
            }
        }
    }
}