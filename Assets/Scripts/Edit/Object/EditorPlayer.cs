using UnityEngine;

namespace MapEditor
{
    public class EditorPlayer : EditorObject
    {
        public override bool Remove()
        {
            Debug.LogWarning("플레이어 오브젝트는 삭제할 수 없습니다.");
            return false;
        }
    }
}