using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class FileView : CategoryView
    {
        public GameObject buttonPrefab;
        public GameObject StageListPanel;
        public SavePanel savePanel;
        public ChangeStageNamePanel changeNamePanel;
        public static string SavePath;

        private void Awake()
        {
            SavePath = $"{Application.dataPath}/Resources/Data/Stage";
        }

        public override void Initialize()
        {
            savePanel.Initialize(editorManager);
            changeNamePanel.Initialize(editorManager);
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            StageListPanel.SetActive(false);
            savePanel.gameObject.SetActive(false);
            changeNamePanel.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void LoadStage(TextAsset jsonString)
        {
            ClearAllObject();
            
            var stageInfo =  JsonUtility.FromJson<JsonLoader.JsonStage>(jsonString.ToString());
            var tilePrefabDic = GetTilePrefabs();
            var itemPrefabDic = GetItemPrefabs();
            
            foreach (var tileData in stageInfo.tilesData)
            {
                var tilePrefab = tilePrefabDic[tileData.tileType.ToString()];
                var tileObject = Instantiate(tilePrefab);
                tileObject.name = $"{tileData.tileType} {tileData.posX}-{tileData.posY}";
                tileObject.transform.position = new Vector3(tileData.posX, 0, tileData.posY);
                tileObject.GetComponent<EditorObject>().coordinate = new Vector2Int(tileData.posX, tileData.posY);
                editorManager.placedTileList.Add(tileObject.GetComponent<EditorObject>());
            }
        
            // 아이템 생성
            foreach (var itemData in stageInfo.itemsData)
            {
                Debug.Log($"{itemData.itemType.ToString()}{itemData.value}");
                var itemPrefab = itemData.value == 0 ? 
                    itemPrefabDic[itemData.itemType.ToString()] : 
                    itemPrefabDic[$"{itemData.itemType.ToString()}{itemData.value}"];
                var itemObject = Instantiate(itemPrefab);
                itemObject.name = $"{itemData.itemType} {itemData.posX}-{itemData.posY}";
                itemObject.transform.position = new Vector3(itemData.posX, 0, itemData.posY);
                itemObject.GetComponent<EditorObject>().coordinate = new Vector2Int(itemData.posX, itemData.posY);
                editorManager.placedItemList.Add(itemObject.GetComponent<EditorObject>());
            }
        
            // 플레이어 배치
            editorManager.playerObject.transform.position = new Vector3(stageInfo.playerPosX, 0, stageInfo.playerPosY);
            editorManager.playerObject.GetComponent<EditorPlayer>().coordinate = 
                new Vector2Int((int)stageInfo.playerPosX, (int)stageInfo.playerPosY);
            editorManager.bounceCount = stageInfo.bounceCount;
            editorManager.twoStarTurnLimit = stageInfo.twoStarTurnLimit;
            editorManager.threeStarTurnLimit = stageInfo.threeStarTurnLimit;
            editorManager.minMoveTurn = stageInfo.minMoveTurn;
            editorManager.currentStageName = jsonString.name;
        }

        public void OpenSavePanel()
        {
            savePanel.Show();
        }

        public void OpenChangeNamePanel()
        {
            changeNamePanel.Show();
        }

        public void OpenLoadPanel()
        {
            foreach (Transform child in StageListPanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            var stageJsonArray =  Resources.LoadAll<TextAsset>("Data/Stage");
            foreach (var stageJson in stageJsonArray)
            {
                var buttonObject = Instantiate(buttonPrefab, StageListPanel.transform);
                var clickedEvent = new Button.ButtonClickedEvent();
                clickedEvent.AddListener(() => LoadStage(stageJson));
                var button = buttonObject.GetComponent<Button>(); 
                button.onClick = clickedEvent;
                button.transform.Find("Text").GetComponent<Text>().text = stageJson.name;
            }

            var incompleteStageJsonArray = Resources.LoadAll<TextAsset>("Data/IncompleteStage");
            foreach (var stageJson in incompleteStageJsonArray)
            {
                var buttonObject = Instantiate(buttonPrefab, StageListPanel.transform);
                var clickedEvent = new Button.ButtonClickedEvent();
                clickedEvent.AddListener(() => LoadStage(stageJson));
                var button = buttonObject.GetComponent<Button>(); 
                button.onClick = clickedEvent;
                button.transform.Find("Text").GetComponent<Text>().text = stageJson.name;
            }
            
            StageListPanel.SetActive(true);
        }

        private Dictionary<string, GameObject> GetTilePrefabs()
        {
            var tilePrefabs = Resources.LoadAll<GameObject>("Prefabs/Edit/Tile");
            var tilePrefabDic = new Dictionary<string, GameObject>();
            foreach (var tilePrefab in tilePrefabs)
            {
                tilePrefabDic.Add(tilePrefab.name, tilePrefab);
            }

            return tilePrefabDic;
        }

        private Dictionary<string, GameObject> GetItemPrefabs()
        {
            var itemPrefabs = Resources.LoadAll<GameObject>("Prefabs/Edit/Item");
            var itemPrefabDic = new Dictionary<string, GameObject>();
            foreach (var itemPrefab in itemPrefabs)
            {
                Debug.Log(itemPrefab.name);
                itemPrefabDic.Add(itemPrefab.name, itemPrefab);
            }

            return itemPrefabDic;
        }

        public void ClearAllObject()
        {
            editorManager.selectedObjectList.Clear();
            
            foreach (var tile in editorManager.placedTileList)
            {
                Destroy(tile.gameObject);   
            }

            foreach (var item in editorManager.placedItemList)
            {
                Destroy(item.gameObject);
            }
            
            editorManager.placedTileList.Clear();
            editorManager.placedItemList.Clear();
            editorManager.currentStageName = null;
        }

        public void SaveStage()
        {
            if (string.IsNullOrEmpty(editorManager.currentStageName))
            {
                Debug.LogWarning("저장할 파일이 없습니다.");
            }
            else
            {
                var jsonString = savePanel.ConvertStageToJson();
                File.WriteAllText($"{SavePath}/{editorManager.currentStageName}.json", jsonString);
                Debug.Log($"{SavePath}/{editorManager.currentStageName}.json");
                Debug.Log("저장 완료");    
            }
        }

        public void ChangeStageName(string newStageName)
        {
            if (File.Exists($"{SavePath}/{newStageName}.json"))
            {
                Debug.LogWarning("이미 해당 파일명이 존재합니다.");
            }
            else
            {
                File.Move($"{editorManager.currentStageName}.json", $"{newStageName}.json");   
            }
        }
    }
}