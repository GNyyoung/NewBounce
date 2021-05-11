using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class EditorMouse : MonoBehaviour
    {
        //타일 생성 시 뜨는 미리보기
        public GameObject previewObject;
        // 화면 드래그 시 드래그한 공간 표시해줌.
        public GameObject dragObject;
        // 타일 생성 시 가로로 몇 칸 생성되는지 알려줌.
        public GameObject tileWidthObject;
        // 타일 생성 시 세로로 몇 칸 생성되는지 알려줌.
        public GameObject tileHeightObject;
        public EditorManager editorManager;
        public UnityEngine.EventSystems.EventSystem eventSystem;
        public Camera mainCamera;

        // 드래그가 시작된 위치
        Vector3 dragStartPosition;
        // 미리보기 기본 사이즈.
        Vector2 previewBaseSize;
        // 이전 프레임에 마우스가 있던 위치
        private Vector3 previousMovedMousePosition;

        // Start is called before the first frame update
        void Start()
        {
            previewBaseSize = previewObject.GetComponent<SpriteRenderer>().size;
            previewObject.SetActive(false);
            dragObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (eventSystem.IsPointerOverGameObject() == false)
            {
                switch (editorManager.currentMode)
                {
                    case EditorMode.Create:
                        CheckMouseOnCreate();
                        break;
                    case EditorMode.Normal:
                        CheckMouseOnNormal();
                        break;
                    case EditorMode.Move:
                        CheckMouseOnMove();
                        break;
                    default:
                        Debug.LogWarning("추가되지 않은 모드입니다. 새로 추가해주세요.");
                        break;
                }
            }   
        }

        /// <summary>
        /// 생성모드일 때 마우스 사용을 확인합니다.
        /// </summary>
        void CheckMouseOnCreate()
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) == true)
            {
                editorManager.selectedObjectList.Clear();
                Debug.Log(mousePosition);
                dragStartPosition = mousePosition;
                previewObject.SetActive(true);
                previewObject.GetComponent<SpriteRenderer>().size = previewBaseSize;
                tileWidthObject.SetActive(true);
                tileHeightObject.SetActive(true);
            }
            else if (Input.GetMouseButton(0) == true)
            {
                var dragStartIntPosition = Vector3Int.RoundToInt(dragStartPosition);
                int width = Mathf.RoundToInt(mousePosition.x - dragStartIntPosition.x);
                int height = Mathf.RoundToInt(mousePosition.z - dragStartIntPosition.z);
                previewObject.GetComponent<SpriteRenderer>().size = 
                    previewBaseSize + new Vector2(Mathf.Abs(width), Mathf.Abs(height));
                previewObject.transform.position = new Vector3(
                    dragStartIntPosition.x + width / 2.0f,
                    0,
                    dragStartIntPosition.z + height / 2.0f);
                
                // 크기 가이드 위치와 텍스트를 설정함.
                tileWidthObject.GetComponent<SizeGuide>().SetWidthGuide(dragStartIntPosition, width);
                tileHeightObject.GetComponent<SizeGuide>().SetHeightGuide(dragStartIntPosition, height);
                
                // 마우스가 화면 밖으로 나가면 카메라 이동
                if(Input.mousePosition.x < 0)
                {
                    mainCamera.transform.position += Vector3.left * Time.deltaTime * 5;
                }
                else if(Input.mousePosition.x > Screen.width)
                {
                    mainCamera.transform.position += Vector3.right * Time.deltaTime * 5;
                }
                if(Input.mousePosition.y < 0)
                {
                    mainCamera.transform.position += Vector3.back * Time.deltaTime * 5;
                }
                else if(Input.mousePosition.y > Screen.height)
                {
                    mainCamera.transform.position += Vector3.forward * Time.deltaTime * 5;
                }
            }
            else if (Input.GetMouseButtonUp(0) == true)
            {
                var startPosition = new Vector2Int(Mathf.RoundToInt(dragStartPosition.x), Mathf.RoundToInt(dragStartPosition.z));
                var endPosition = new Vector2Int(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.z));
                Debug.Log(startPosition);
                Debug.Log(endPosition);
                editorManager.CreateObject(startPosition, endPosition);
                previewObject.SetActive(false);
                tileWidthObject.SetActive(false);
                tileHeightObject.SetActive(false);

                editorManager.currentMode = EditorMode.Normal;
            }
        }

        /// <summary>
        /// 일반상태에서 마우스 사용을 확인합니다.
        /// </summary>
        private void CheckMouseOnNormal()
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var dragSprite = dragObject.GetComponent<SpriteRenderer>();

            if (Input.GetMouseButtonDown(0))
            {
                dragStartPosition = mousePosition;
                dragObject.SetActive(true);
                dragSprite.size = Vector2.zero;
                dragObject.transform.position = dragStartPosition;    
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 dragSize =
                    new Vector2(mousePosition.x - dragStartPosition.x, mousePosition.z - dragStartPosition.z);
                dragSprite.size = dragSize;
                dragObject.transform.position = new Vector3(
                    dragStartPosition.x + dragSize.x / 2.0f,
                    0,
                    dragStartPosition.z + dragSize.y / 2.0f);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                RaycastHit[] hits = null;
                if (Mathf.Abs(dragSprite.size.x) < 0.1f && Mathf.Abs(dragSprite.size.y) < 0.1f)
                {
                    Debug.Log(mousePosition);
                    Physics.Raycast(mousePosition, Vector3.down, out var hitObject);
                    if (hitObject.collider != null)
                    {
                        Debug.Log(hitObject.collider.gameObject.name);
                        hits = new[] {hitObject};   
                    }
                }
                else
                {
                    hits = Physics.BoxCastAll(
                        dragObject.transform.position,
                        new Vector3(
                            Mathf.Abs(dragSprite.size.x / 2.0f),
                            100,
                            Mathf.Abs(dragSprite.size.y) / 2.0f),
                        Vector3.up);    
                }
                
                if (hits != null)
                {
                    var selectedObjectList = new List<GameObject>();
                    foreach (var hit in hits)
                    {
                        selectedObjectList.Add(hit.collider.gameObject);
                    }
                    editorManager.SelectObject(selectedObjectList);    
                }
                else
                {
                    editorManager.DeselectAllObject();
                }
                dragObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// 오브젝트 이동상태에서 마우스 사용을 확인합니다.
        /// </summary>
        private void CheckMouseOnMove()
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            if (Input.GetMouseButtonDown(0) == true)
            {
                dragStartPosition = mousePosition;
                Physics.Raycast(new Ray(mousePosition, Vector3.down), out RaycastHit hit);
                if (hit.collider != null)
                {
                    // 드래그로 선택한 오브젝트들 중 클릭한 오브젝트가 없으면 선택 풀기
                    bool isSelectedObject = false;
                    Debug.Log(editorManager.selectedObjectList.Count);
                    foreach (var selectedObject in editorManager.selectedObjectList)
                    {
                        if (hit.collider.gameObject.Equals(selectedObject) == true)
                        {
                            isSelectedObject = true;
                            break;
                        }
                    }
            
                    if (isSelectedObject == false)
                    {
                        editorManager.DeselectAllObject();
                        if (hit.collider != null)
                        {
                            editorManager.selectedObjectList.Add(hit.collider.gameObject);    
                        }
                    }

                    previousMovedMousePosition = mousePosition;
                }
                else
                {
                    editorManager.DeselectAllObject();
                }
            }
            else if (Input.GetMouseButton(0) == true)
            {
                foreach (var selectedObject in editorManager.selectedObjectList)
                {
                    selectedObject.transform.position += mousePosition - previousMovedMousePosition;
                }

                previousMovedMousePosition = mousePosition;
            }
            else if(Input.GetMouseButtonUp(0) == true)
            {
                foreach (var selectedObject in editorManager.selectedObjectList)
                {
                    var positionInt = Vector3Int.RoundToInt(
                        selectedObject.transform.position + mousePosition - previousMovedMousePosition);
                    var selectedObjComponent = selectedObject.GetComponent<EditorObject>();
                    selectedObject.transform.position = positionInt;
                    selectedObject.GetComponent<EditorObject>().coordinate = new Vector2Int(positionInt.x, positionInt.z);
                    
                    // 좌표 중복검사
                    if (selectedObjComponent.objectType == ObjectType.Tile)
                        CheckTileOverlap(selectedObjComponent);
                    else if (selectedObjComponent.objectType == ObjectType.Item)
                        CheckItemOverlap(selectedObjComponent);
                }
            }
        }

        /// <summary>
        /// 선택한 좌표에 타일이 있으면 true를 반환합니다.
        /// </summary>
        private bool CheckTileOverlap(EditorObject editorObject)
        {
            foreach (var placedTile in editorManager.placedTileList)
            {
                if (placedTile.coordinate.Equals(editorObject.coordinate) == true && 
                    placedTile.Equals(editorObject) == false)
                {
                    Debug.LogWarning($"같은 좌표에 겹친 타일이 존재합니다. : {editorObject.coordinate}");
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 선택한 좌표에 아이템이 있으면 true를 반환합니다.
        /// </summary>
        private bool CheckItemOverlap(EditorObject editorObject)
        {
            foreach (var placedItem in editorManager.placedItemList)
            {
                if (placedItem.coordinate.Equals(editorObject.coordinate) == true && 
                    placedItem.Equals(editorObject) == false)
                {
                    Debug.LogWarning($"같은 좌표에 겹친 타일이 존재합니다. : {editorObject.coordinate}");
                    return true;
                }
            }

            return false;
        }
    }
}

