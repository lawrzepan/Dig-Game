using System.Collections.Generic;
using DigGame.Terrain.Objects;
using DigGame.Terrain.Tile;
using DigGame.Terrain.Objects;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Noise;

namespace DigGame.Terrain.Chunking;

public struct ChunkData()
{
    private PlaceableType[] data = new PlaceableType[Chunk.Size * Chunk.Size];

    public int Length { get; } = Chunk.Size * Chunk.Size;
    
    public PlaceableType this[int x, int y]
    {
        get => data[x + y * Chunk.Size];
        set
        {
            data[x + y * Chunk.Size] = value;
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

    private bool PlaceablesInitialised = false;
    private List<Placeable> _Placeables = new List<Placeable>(127);

    public List<Placeable> Placeables
    {
        get => PlaceablesInitialised ? _Placeables : AsPlaceables();
    }

    private List<Placeable> AsPlaceables()
    {
        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                switch (data[x, y])
                {
                    case PlaceableType.Stone:
                        _Placeables.Add(new Stone()
                        {
                            Coordinate = new Coordinate(x, y, 0, 0)
                        });
                        break;
                }
            }
        }

        return _Placeables;
    }

    public Chunk()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                data[x, y] = Noise.Perlin((Coordinate.X + x) * 0.1, (Coordinate.Y + y) * 0.1) > 0 ? PlaceableType.Air : PlaceableType.Stone;
            }
        }
    }
}