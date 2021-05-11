using System;
using UnityEngine;

public class Door : Item
{
    private Tile originalTile;
    private void Start()
    {
        originalTile = stageManager.GetTile(coordinate);
        originalTile.isBlockHorz = true;
        originalTile.isBlockVert = true;
    }

    public override void ActiveEffectByStop(){}

    public override ItemType GetType()
    {
        return ItemType.Door;
    }

    private void OnDestroy()
    {
        originalTile.isBlockHorz = false;
        originalTile.isBlockVert = false;
    }
}