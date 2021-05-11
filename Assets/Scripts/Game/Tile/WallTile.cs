using System.Collections;
using UnityEngine;

public class WallTile : Tile
{
    public override void ActiveEffectByStop()
    {
        Debug.LogError($"멈출 수 없는 타일 위에서 멈춰있습니다.");
    }

    public override TileType GetTileType()
    {
        return TileType.Wall;
    }
}