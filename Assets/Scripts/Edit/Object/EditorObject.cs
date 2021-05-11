using UnityEngine;

namespace MapEditor
{
    public enum ObjectType
    {
        Tile,
        Item,
        Decoration,
        Player
    }
    
    /// <summary>
    /// 에디터에서 맵에 사용할 모든 오브젝트에 부착되는 클래스입니다.
    /// </summary>
    public abstract class EditorObject : MonoBehaviour
    {
        public string objectName;
        public ObjectType objectType;
        public Vector2Int coordinate;
        // 동일 타입의 오브젝트와 구분할 필요가 있을 때 사용
        public int value = 0;

        /// <summary>
        /// 오브젝트를 에디터에서 제거합니다. 제거된 경우 true를 반환합니다.
        /// </summary>
        public abstract bool Remove();
    }
}