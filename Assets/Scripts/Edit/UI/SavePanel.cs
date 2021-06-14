using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class SavePanel : MonoBehaviour
    {
        private EditorManager _editorManager;
        
        public Text errorText;
        public Text currentStageNameText;
        public InputField newStageNameInput;
        
        public void Initialize(EditorManager editorManager)
        {
            _editorManager = editorManager;
        }
        
        public void Show()
        {
            errorText.gameObject.SetActive(false);
            currentStageNameText.text = _editorManager.currentStageName;
            newStageNameInput.text = null;
            gameObject.SetActive(true);
        }

        public void ShowErrorText()
        {
            errorText.gameObject.SetActive(true);
        }
        
        public void SaveNewStage()
        {
            var jsonString = ConvertStageToJson();
            File.WriteAllText($"{FileView.SavePath}/{newStageNameInput.text}.json", jsonString);
            Debug.Log($"{FileView.SavePath}/{newStageNameInput.text}.json");
            Debug.Log("저장 완료");
        }

        public string ConvertStageToJson()
        {
            int leftBorder = int.MaxValue;
            int bottomBorder = int.MaxValue;
            int rightBorder = int.MinValue;
            int topBorder = int.MinValue;
            var jsonStage = new JsonLoader.JsonStage();
            
            // 타일 저장
            List<TileData> tileList = new List<TileData>();
            foreach (var tileObjComponent in _editorManager.placedTileList)
            {
                var tile = tileObjComponent as EditorTile;
                tileList.Add(new TileData(tile.coordinate.x, tile.coordinate.y, tile.tileType));
                
                if (tile.coordinate.x < leftBorder)
                    leftBorder = tile.coordinate.x;
                if (tile.coordinate.y < bottomBorder)
                    bottomBorder = tile.coordinate.y;
                if (tile.coordinate.x > rightBorder)
                    rightBorder = tile.coordinate.x;
                if (tile.coordinate.y > topBorder)
                    topBorder = tile.coordinate.y;
            }

            // 아이템 저장
            List<ItemData> itemList = new List<ItemData>();
            foreach (var itemObjComponent in _editorManager.placedItemList)
            {
                var item = itemObjComponent as EditorItem;
                itemList.Add(new ItemData(item.coordinate.x, item.coordinate.y, item.itemType, item.value));

                bool isOnTile = false;
                foreach (var tile in tileList)
                {
                    if (tile.posX == item.coordinate.x && tile.posY == item.coordinate.y)
                    {
                        isOnTile = true;
                        break;
                    }
                }
                
                if (isOnTile == false)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("타일이 없는 곳에 아이템이 존재합니다.");
                    stringBuilder.Append(item.itemType.ToString());
                    stringBuilder.Append($"{item.coordinate}");
                    Debug.LogWarning(stringBuilder);
                }
            }

            int correctionPosX = Mathf.Abs(leftBorder) > 0 ? leftBorder : 0;
            int correctionPosY = Mathf.Abs(bottomBorder) > 0 ? bottomBorder : 0;
            var player = _editorManager.playerObject.GetComponent<EditorPlayer>();
            
            Debug.Log($"{correctionPosX}, {correctionPosY}");

            if (correctionPosX != 0 || correctionPosY != 0)
            {
                for (int i = 0; i < tileList.Count; i++)
                {
                    tileList[i] = new TileData(
                        tileList[i].posX - correctionPosX,
                        tileList[i].posY - correctionPosY, 
                        tileList[i].tileType);
                }

                for (int i = 0; i < itemList.Count; i++)
                {
                    itemList[i] = new ItemData(
                        itemList[i].posX - correctionPosX, 
                        itemList[i].posY - correctionPosY, 
                        itemList[i].itemType,
                        itemList[i].value);
                }
                
                player.coordinate -= new Vector2Int(correctionPosX, correctionPosY);
            }

            jsonStage.width = rightBorder - leftBorder + 1;
            jsonStage.height = topBorder - bottomBorder + 1;
            jsonStage.tilesData = tileList.ToArray();
            jsonStage.itemsData = itemList.ToArray();
            jsonStage.bounceCount = _editorManager.bounceCount;
            jsonStage.twoStarTurnLimit = _editorManager.twoStarTurnLimit;
            jsonStage.threeStarTurnLimit = _editorManager.threeStarTurnLimit;
            jsonStage.minMoveTurn = _editorManager.minMoveTurn;
            jsonStage.playerPosX = player.coordinate.x;
            jsonStage.playerPosY = player.coordinate.y;

            var jsonString = JsonUtility.ToJson(jsonStage);
            return jsonString;
        }
    }
}