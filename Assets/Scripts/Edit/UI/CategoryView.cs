using UnityEngine;

namespace MapEditor
{
    /// <summary>
    /// 메뉴에 사용하는 카테고리 패널들이 가지는 클래스
    /// </summary>
    public abstract class CategoryView : MonoBehaviour
    {
        public EditorManager editorManager;
        /// <summary>
        /// 각 뷰를 초기화합니다.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 뷰를 열고, 그 후 할 행동을 정의합니다.
        /// </summary>
        public abstract void Show();

        /// <summary>
        /// 뷰를 닫고, 그 후 할 행동을 정의합니다.
        /// </summary>
        public abstract void Hide();
    }
}