using System;

namespace MapEditor
{
    public class EditorItem : EditorObject
    {
        public ItemType itemType;

        private void Start()
        {
            objectName = itemType.ToString();
        }

        public override bool Remove()
        {
            Destroy(this.gameObject);
            return true;
        }
    }
}