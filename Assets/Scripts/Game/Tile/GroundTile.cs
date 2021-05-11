using System.Collections;
using UnityEngine;

public class GroundTile : Tile
{
    private Item _item;

    public override void ActiveEffectByStop()
    {
        return;
    }

    public override TileType GetTileType()
    {
        return TileType.Ground;
    }
}