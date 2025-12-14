using DigGame.Terrain.Tile;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Noise;

namespace DigGame.Terrain.Chunking;

public struct ChunkData()
{
    private TileType[] data = new TileType[Chunk.Size * Chunk.Size];

    public TileType this[int x, int y]
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
    
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }

    private ChunkData data = new ChunkData();

    public Chunk()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                data[x, y] = Noise.Perlin((X + x) * 0.1, (Y + y) * 0.1) > 0 ? TileType.Air : TileType.Stone;
            }
        }
    }
}