using System;
using UnityEngine;

namespace MapEditor
{
    public class EditorTile : EditorObject
    {
        public TileType tileType;

        private void Start()
        {
            objectName = tileType.ToString();
        }

        public override bool Remove()
        {
            Debug.Log("1");
            Destroy(this.gameObject);
            return true;
        }
    }
}