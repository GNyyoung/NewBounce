using UnityEngine;

[System.Serializable]
public struct TileData
{
    public TileData(int posX, int posY, TileType tileType)
    {
        this.posX = posX;
        this.posY = posY;
        this.tileType = tileType;
    }
    
    public int posX;
    public int posY;
    public TileType tileType;

    public void CorrectPosX(int correction)
    {
        posX -= correction;
    }

    public void CorrectPosY(int correction)
    {
        posY -= correction;
    }
}

[System.Serializable]
public struct ItemData
{
    public ItemData(int posX, int posY, ItemType itemType, int value)
    {
        this.posX = posX;
        this.posY = posY;
        this.itemType = itemType;
        this.value = value;
    }
    
    public int posX;
    public int posY;
    public ItemType itemType;
    public int value;
    
    public void CorrectPosX(int correction)
    {
        posX -= correction;
    }

    public void CorrectPosY(int correction)
    {
        posY -= correction;
    }
}

[System.Serializable]
public struct DecorationData
{
    public int posX;
    public int posY;
    public int id;
}

[System.Serializable]
public struct SaveData
{
    public int[] stageStarNum;
}

/// <summary>
/// Json 데이터를 게임에서 사용할 수 있는 데이터 타입으로 바꿔줍니다.
/// </summary>
public static class JsonLoader
{
    /// <summary>
    /// Json으로부터 가져온 스테이지 정보를 저장하는 클래스.
    /// </summary>
    [System.Serializable]
    public class JsonStage
    {
        public int width;
        public int height;
        public int playerPosX;
        public int playerPosY;
        public int bounceCount;
        public int twoStarTurnLimit;
        public int threeStarTurnLimit;
        public int minMoveTurn;
        public TileData[] tilesData;
        public ItemData[] itemsData;
        public DecorationData[] decorationsData;
        public TileType[] usedTileTypes;
        public ItemType[] usedItemTypes;
        public int[] usedDecorationIDs;
    }

    [System.Serializable]
    public class JsonTile
    {
        public string type;
        public string path;
    }

    /// <summary>
    /// 스테이지를 불러옵니다.
    /// </summary>
    /// <param name="group">스테이지 그룹</param>
    /// <param name="num">스테이지 번호</param>
    public static JsonStage LoadStage(int group, int num)
    {
        var stageJsonAsset = ResourceLoader.Instance.GetStageJsonData(group, num);
        var stageJson = JsonUtility.FromJson<JsonStage>(stageJsonAsset.text);
        return stageJson;
    }

    public static JsonStage LoadStage(TextAsset stageData)
    {
        return JsonUtility.FromJson<JsonStage>(stageData.text);
    }

    public static SaveData LoadSaveData()
    {
        var saveDataJsonAsset = ResourceLoader.Instance.GetSaveData();
        if (saveDataJsonAsset == null)
        {
            return new SaveData();
        }
        else
        {
            return JsonUtility.FromJson<SaveData>(saveDataJsonAsset);   
        }
    }
}         