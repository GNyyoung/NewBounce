using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public enum EditorMode
    {
        Normal,
        Create,
        Move
    }
    // 아이템 추가할 때: CreateItem에 코드 추가, Player.UseItemEffect에 코드 추가, Prefab추가
    public class EditorManager : MonoBehaviour
    {
        /// <summary>
        /// 맵에 설치된 모든 타일 리스트
        /// </summary>
        public List<EditorObject> placedTileList = new List<EditorObject>();
        /// <summary>
        /// 맵에 설치된 모든 아이템 리스트
        /// </summary>
        public List<EditorObject> placedItemList = new List<EditorObject>();
        /// <summary>
        /// 설치하기 위해 선택한 오브젝트
        /// </summary>
        public GameObject objectPrefab;
        /// <summary>
        /// 생성한 오브젝트 보관용 오브젝트
        /// </summary>
        public GameObject tileFolder;
        public GameObject itemFolder;

        public int bounceCount = 3;
        public int twoStarTurnLimit = 99;
        public int threeStarTurnLimit = 99;
        public int minMoveTurn = 99;
        public int stageGroup;
        public int stageNum;
        public EditorMode currentMode; 
        public List<GameObject> selectedObjectList = new List<GameObject>();
        public GameObject playerObject;
        public string currentStageName;
        

        public void CreateObject(Vector2Int coordinate)
        {
            var newObject = Instantiate(objectPrefab);
            newObject.transform.position = new Vector3(coordinate.x, 0, coordinate.y);
            newObject.name = $"{objectPrefab.name} ({coordinate.x}, {coordinate.y})";
            newObject.GetComponent<EditorObject>().coordinate = coordinate;
            
            if (newObject.GetComponent<EditorObject>().objectType == ObjectType.Tile)
                CreateTile(newObject);
            else if (newObject.GetComponent<EditorObject>().objectType == ObjectType.Item)
                CreateItem(newObject);
            
            InfoPanel.Instance.Hide();
        }

        public void CreateObject(Vector2Int start, Vector2Int end)
        {
            int up, down, left, right;
            if (start.x <= end.x)
            {
                left = start.x;
                right = end.x;
            }
            else
            {
                left = end.x;
                right = start.x;
            }
            if (start.y <= end.y)
            {
                down = start.y;
                up = end.y;
            }
            else
            {
                down = end.y;
                up = start.y;
            }


            for (int x = 0; x <= right - left; x++)
            {
                for (int y = 0; y <= up - down; y++)
                {
                    CreateObject(new Vector2Int(left + x, down + y));
                }
            }
        }

        private void CreateTile(GameObject tileObject)
        {
            var tile = tileObject.GetComponent<EditorObject>();
            for (int i = placedTileList.Count - 1 ; i >= 0 ; i--)
            {
                if (placedTileList[i].coordinate == tile.coordinate)
                {
                    var removedTile = placedTileList[i];
                    placedTileList.Remove(removedTile);
                    Debug.Log(removedTile.gameObject.GetInstanceID());
                    Destroy(removedTile.gameObject);
                }
            }
            tileObject.transform.parent = tileFolder.transform;
            placedTileList.Add(tile);
        }

        private void CreateItem(GameObject itemObject)
        {
            var item = itemObject.GetComponent<EditorObject>();
            for (int i = placedItemList.Count - 1 ; i >= 0 ; i--)
            {
                if (placedItemList[i].coordinate == item.coordinate)
                {
                    var removedItem = placedItemList[i];
                    placedItemList.Remove(removedItem);
                    Debug.Log(removedItem.gameObject.GetInstanceID());
                    Destroy(removedItem.gameObject);
                }
            }
            itemObject.transform.parent = itemFolder.transform;
            placedItemList.Add(itemObject.GetComponent<EditorObject>());
        }

        /// <summary>
        /// 마우스로 선택한 오브젝트들을 처리합니다.
        /// </summary>
        public void SelectObject(List<GameObject> selectedObjectList)
        {
            DeselectAllObject();

            foreach (var selectedObject in selectedObjectList)
            {
                selectedObject.GetComponent<SpriteRenderer>().color = new Color32(180, 180, 180, 255);
            }

            this.selectedObjectList = selectedObjectList;

            var selectedObjComponentList = new List<EditorObject>();
            foreach (var selectedObject in selectedObjectList)
            {
                selectedObjComponentList.Add(selectedObject.GetComponent<EditorObject>());
            }

            if (selectedObjectList.Count == 0)
                InfoPanel.Instance.Hide();
            else
                InfoPanel.Instance.ShowInfoPanel(selectedObjComponentList.ToArray());
        }

        // 오브젝트 선택을 해제합니다.
        public void DeselectAllObject()
        {
            foreach (var selectedObject in selectedObjectList)
            {
                selectedObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
            selectedObjectList.Clear();
            InfoPanel.Instance.Hide();
        }

        public void RemoveObject(EditorObject editorObject)
        {
            if (editorObject.objectType == ObjectType.Tile)
                placedTileList.Remove(editorObject);
            else if (editorObject.objectType == ObjectType.Item)
                placedItemList.Remove(editorObject);

            editorObject.Remove();
        }
    }
}

