using System;

namespace DigGame.Terrain.Tile;

public static class PlaceableType
{
    public static bool IsBlock(Int16 PlaceableType)
    {
        switch (PlaceableType)
        {
            case 1:
                return true;
            default:
                return false;
        }
    }
    
    public const Int16 Air = 0;
    public const Int16 Stone = 1;
}