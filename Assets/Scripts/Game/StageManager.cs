using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구현할 것
// 각 타일 정보를 2차원 배열로 저장
// 타일 정보 불러오기
// 이동 횟수 체크
public class StageManager : MonoBehaviour
{
    public Player player;
    public GameInformation gameInformation;
    public CameraMove CameraMove;
    private Tile[,] _tileInfo;
    public List<Item> itemList = new List<Item>();
    public int twoStarLimit;
    public int threeStarLimit;
    public int minClearMoveTurn;
    private int currentMoveTurn = 0;
    private int initialStarNum = 0;
    private int currentStarNum = 0;

    public int CurrentMoveTurn
    {
        get => currentMoveTurn;
        set
        {
            currentMoveTurn = value;
            gameInformation.UpdateTurn();
        }
    }

    public int CurrentStarNum
    {
        get => currentStarNum;
        set
        {
            currentStarNum = value;
            gameInformation.UpdateStarCondition(initialStarNum, currentStarNum);
        }
    }

    void Start()
    {
        LoadStage();
        Control.isControllable = true;
    }

    private void LoadStage()
    {
        var stageInfo = JsonLoader.LoadStage(InformationProvider.Instance.stageDataset[InformationProvider.Instance.stageIndex]);
        
        // 타일 생성
        _tileInfo = new Tile[stageInfo.width, stageInfo.height];
        foreach (var tileData in stageInfo.tilesData)
        {
            var tilePrefab = ResourceLoader.Instance.GetTilePrefab(tileData.tileType);
            var tileObject = Instantiate(tilePrefab);
            // 타일 이름 : "x-y"
            tileObject.name = $"{tileData.tileType} {tileData.posX}-{tileData.posY}";
            var tileComponent = tileObject.GetComponent<Tile>(); 
            tileComponent.Initialize(tileData);
            _tileInfo[tileData.posX, tileData.posY] = tileComponent;
        }
        
        // 아이템 생성
        foreach (var itemData in stageInfo.itemsData)
        {
            Debug.Log($"{itemData.itemType}, {itemData.value}");
            var itemPrefab = ResourceLoader.Instance.GetItemPrefab(itemData.itemType, itemData.value);
            var itemObject = Instantiate(itemPrefab);
            itemObject.name = $"{itemData.itemType} {itemData.posX}-{itemData.posY}";
            var itemComponent = itemObject.GetComponent<Item>();
            itemComponent.Initialize(itemData, player, this);
            GetTile(new Vector2Int(itemData.posX, itemData.posY)).item = itemComponent;
            itemList.Add(itemComponent);

            if (itemData.itemType == ItemType.Star)
            {
                initialStarNum += 1;
            }
        }
        
        // // 장식 생성
        // foreach (var decorationData in stageInfo.decorationsData)
        // {
        //     var decorationObject = ResourceLoader.Instance.GetDecorationPrefab(decorationData.id);
        //     decorationObject.name = $"{decorationData.posX}-{decorationData.posY}";
        //     decorationObject.GetComponent<Decoration>().SetPosition(decorationData);
        // }
        
        // 플레이어 배치
        player.BounceCount = stageInfo.bounceCount;
        player.SetPosition(stageInfo.playerPosX, stageInfo.playerPosY);
        twoStarLimit = stageInfo.twoStarTurnLimit;
        threeStarLimit = stageInfo.threeStarTurnLimit;
        minClearMoveTurn = stageInfo.minMoveTurn;
        
        // 게임 정보 UI 초기화
        gameInformation.Initialize(stageInfo.bounceCount, initialStarNum);
        
        CameraMove.SetCameraSize(stageInfo.width);
    }

    public Tile GetTile(Vector2Int coordinate)
    {
        if (coordinate.x >= _tileInfo.GetLength(0) ||
            coordinate.x < 0 ||
            coordinate.y >= _tileInfo.GetLength(1) ||
            coordinate.y < 0)
        {
            return null;
        }
        else
        {
            return _tileInfo[coordinate.x, coordinate.y];   
        }
    }

    public Vector2Int GetMapSize()
    {
        return new Vector2Int(_tileInfo.GetLength(0), _tileInfo.GetLength(1));
    }

    public bool CheckClearCondition()
    {
        if (CurrentStarNum == initialStarNum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
